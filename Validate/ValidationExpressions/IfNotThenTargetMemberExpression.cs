using System;

namespace Validate.ValidationExpressions
{
    public class IfNotThenTargetMemberExpression<T> : IfThenTargetMemberExpression<T>
    {
        public IfNotThenTargetMemberExpression(Predicate<T> ifThis, ValidationMessage message, Func<T, Validator>[] nestedValidators) : base(ifThis, message, nestedValidators)
        {
        }

        public IfNotThenTargetMemberExpression(Predicate<T> ifThis, ValidationMessage message, Predicate<T>[] predicates) : base(ifThis, message, predicates)
        {
        }

        protected override bool EvaluateIfPredicate(T target)
        {
            return !base.EvaluateIfPredicate(target);
        }
    }
}