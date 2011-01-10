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
        protected readonly Expression<Func<T, U>> TargetMemberExpression;
        protected readonly ValidationMessage Message;

        public TargetMemberValidationExpression(Expression<Func<T,U>> targetMemberExpression, ValidationMessage message = null)
        {
            this.TargetMemberExpression = targetMemberExpression;
            this.Message = message;

            VerifyValidationExpression(message, targetMemberExpression);
        }

        public TargetMemberValidationExpression(ValidationMessage message)
        {
            this.Message = message;
        }

        protected virtual void VerifyValidationExpression(ValidationMessage message, Expression<Func<T, U>> expression)
        {
            if (message == null && IsNotMemberExpression(expression) && IsNotMethodCallExpression(expression) && IsNotParameterExpression(expression))
                throw new InvalidValidationException("The validation expression does not point to a property, field or method.") { ValidationExpression = expression };
        }

        private bool IsNotParameterExpression(LambdaExpression expression)
        {
            var pe = expression.Body as ParameterExpression;
            return pe == null || pe.Type != typeof (T);
        }

        private bool IsNotMethodCallExpression(LambdaExpression expression)
        {
            var mce = expression.Body as MethodCallExpression;
            return mce == null || mce.Object == null || mce.Object.Type != typeof(T);
        }

        private bool IsNotMemberExpression(LambdaExpression expression)
        {
            return !(expression.Body is MemberExpression);
        }

        protected virtual List<string> GetCauses(IEnumerable<IValidator> validators)
        {
            return validators.SelectMany(v => v.Errors.Select(e => "{{{0}}}".WithFormat(e.Cause))).ToList();
        }

        private string[] GetMethodAndMember(LambdaExpression expression)
        {
            if (expression == null)
                return new string[2] { "{{Target Type could not be determined, guessed as {0}}}".WithFormat(GetFriendlyTypeName(typeof(T))), "{{Target Member could not be determined}}" };

            if (expression.Body is MemberExpression)
            {
                var me = (MemberExpression)expression.Body;
                return new string[2] { GetFriendlyTypeName(me.Member.DeclaringType), me.Member.Name };
            }
            else if (expression.Body is MethodCallExpression)
            {
                var mce = (MethodCallExpression)expression.Body;
                return new string[2] { GetFriendlyTypeName(mce.Object.Type), mce.Method.Name };
            }
            else if (expression.Body is ParameterExpression)
            {
                var pe = (ParameterExpression)expression.Body;
                return new string[2] { GetFriendlyTypeName(pe.Type), "Value" };
            }
            Trace.Write("Type information could not be auto discovered. Please specify the TargetType and TargetMember information.");
            return new string[2] { "{{Target Type could not be determined, guessed as {0}}}".WithFormat(GetFriendlyTypeName(typeof(T))), "{{Target Member could not be determined, guessed as {0}}}".WithFormat(expression.Body.ToString()) };
        }

        private string GetFriendlyTypeName(Type type)
        {
            if(type.IsGenericType)
            {
                return "{0}[{1}]".WithFormat(type.Name, type.GetGenericArguments().Select(t => t.Name).Join(", "));
            }
            return type.Name;
        }
        
        protected virtual string GetTargetTypeName(LambdaExpression expression)
        {
            return GetMethodAndMember(expression)[0];
        }

        protected virtual string GetTargetMemberName(LambdaExpression expression)
        {
            return GetMethodAndMember(expression)[1];
        }

        private ValidationMethod<T> validationMethod;
        public virtual ValidationMethod<T> ValidationMethod
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