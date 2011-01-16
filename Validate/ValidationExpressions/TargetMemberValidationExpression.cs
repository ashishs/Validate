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
        protected readonly TargetMemberMetadata TargetMemberMetadata;

        public TargetMemberValidationExpression(Expression<Func<T,U>> targetMemberExpression, ValidationMessage message = null)
        {
            this.TargetMemberExpression = targetMemberExpression;
            this.Message = message;

            VerifyValidationExpression(message, targetMemberExpression);
            TargetMemberMetadata = new TargetMemberExpression<T>(targetMemberExpression).GetTargetMemberMetadata();
        }

        public TargetMemberValidationExpression(ValidationMessage message)
        {
            this.Message = message;
        }

        protected virtual void VerifyValidationExpression(ValidationMessage message, Expression<Func<T, U>> expression)
        {
            var targetMemberExpression = new TargetMemberExpression<T>(expression);
            if (message == null && !targetMemberExpression.Verify())
                throw new InvalidValidationException("The validation expression does not point to a property, field or method.") { ValidationExpression = expression };
        }

        protected virtual List<string> GetCauses(IEnumerable<IValidator> validators)
        {
            return validators.SelectMany(v => v.Errors.Select(e => "{{{0}}}".WithFormat(e.Cause))).ToList();
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