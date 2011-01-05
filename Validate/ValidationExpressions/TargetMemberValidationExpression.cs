using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Validate.Extensions;

namespace Validate.ValidationExpressions
{
    public abstract class TargetMemberValidationExpression<T,U>
    {
        protected readonly Expression<Func<T, U>> targetMemberExpression;
        protected readonly ValidationMessage message;

        public TargetMemberValidationExpression(Expression<Func<T,U>> targetMemberExpression, ValidationMessage message = null)
        {
            this.targetMemberExpression = targetMemberExpression;
            this.message = message;

            VerifyValidationExpression();
        }

        public TargetMemberValidationExpression(ValidationMessage message = null)
        {
            this.message = message;
        }

        protected virtual void VerifyValidationExpression()
        {
            if (message == null && IsNotMemberExpression() && IsNotMethodCallExpression() && IsNotParameterExpression())
                throw new InvalidValidationException("The validation expression does not point to a property, field or method.") { ValidationExpression = targetMemberExpression };
        }

        private bool IsNotParameterExpression()
        {
            var pe = targetMemberExpression.Body as ParameterExpression;
            return pe == null || pe.Type != typeof (T);
        }

        private bool IsNotMethodCallExpression()
        {
            var mce = targetMemberExpression.Body as MethodCallExpression;
            return mce == null || mce.Object == null || mce.Object.Type != typeof(T);
        }

        private bool IsNotMemberExpression()
        {
            return ! (targetMemberExpression.Body is MemberExpression);
        }

        protected virtual List<string> GetCauses(IEnumerable<Validator> validators)
        {
            return validators.SelectMany(v => v.Errors.Select(e => "{{{0}}}".WithFormat(e.Cause))).ToList();
        }

        private string[] GetMethodAndMember()
        {
            if(targetMemberExpression.Body is MemberExpression)
            {
                var me = (MemberExpression) targetMemberExpression.Body;
                return new string[2] { GetFriendlyTypeName(me.Member.DeclaringType), me.Member.Name };
            }
            else if(targetMemberExpression.Body is MethodCallExpression)
            {
                var mce = (MethodCallExpression) targetMemberExpression.Body;
                return new string[2] { GetFriendlyTypeName(mce.Object.Type), mce.Method.Name };
            }
            else if (targetMemberExpression.Body is ParameterExpression)
            {
                var pe = (ParameterExpression)targetMemberExpression.Body;
                return new string[2] { GetFriendlyTypeName(pe.Type), "Value" };
            }
            Trace.Write("Type information could not be auto discovered. Please specify the TargetType and TargetMember information.");
            return new string[2] { "{{Target Type could not be determined, guessed as {0}}}".WithFormat(GetFriendlyTypeName(typeof(T))), "{{Target Member could not be determined, guessed as {0}}}".WithFormat(targetMemberExpression.Body.ToString()) };
        }

        private string GetFriendlyTypeName(Type type)
        {
            if(type.IsGenericType)
            {
                return "{0}[{1}]".WithFormat(type.Name, type.GetGenericArguments().Select(t => t.Name).Join(", "));
            }
            return type.Name;
        }
        
        protected virtual string GetTargetTypeName()
        {
            return GetMethodAndMember()[0];
        }

        protected virtual string GetTargetMemberName()
        {
            return GetMethodAndMember()[1];
        }


        protected virtual string GetValidationMessage()
        {
            var messageFormat = message.ToString();
            var validationMessage = targetMemberExpression == null ? messageFormat.WithFormat(typeof(T).Name, "{{Target Member could not be determined}}") : messageFormat.WithFormat(GetTargetTypeName(), GetTargetMemberName());
            return validationMessage;
        }

        private ValidationMethod<T> validationMethod;
        public ValidationMethod<T> ValidationMethod
        {
            get
            {
                if(validationMethod == null)
                    validationMethod = GetValidationMethod();
                return validationMethod;
            }
        }

        public abstract ValidationMethod<T> GetValidationMethod();
    }
}