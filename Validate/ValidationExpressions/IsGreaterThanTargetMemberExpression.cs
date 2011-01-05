﻿using System;
using System.Linq.Expressions;
using Validate.Extensions;

namespace Validate.ValidationExpressions
{
    public class IsGreaterThanTargetMemberExpression<T, U> : TargetMemberValidationExpression<T, U> where U:IComparable
    {
        private readonly U _greaterThan;

        public IsGreaterThanTargetMemberExpression(Expression<Func<T, U>> targetMemberExpression, U greaterThan, ValidationMessage message)
            : base(targetMemberExpression, message)
        {
            _greaterThan = greaterThan;
        }

        public override ValidationMethod<T> GetValidationMethod()
        {
            var validationMessage = message.Populate(targetType: GetTargetTypeName(), targetMember: GetTargetMemberName(), targetValueGreaterThan: _greaterThan);
            var compiledSelector = targetMemberExpression.Compile();
            Func<Validator<T>, Validator<T>> validation = (v) =>
                                                              {
                                                                  var target = compiledSelector(v.Target);
                                                                  if (target.CompareTo(_greaterThan) <= 0)
                                                                      v.AddError(new ValidationError(validationMessage.Populate(targetValue: target).ToString(), target,
                                                                                 cause: "{{The target member {0}.{1} with value {2} was not greater than {3}.}}".WithFormat(GetTargetTypeName(), GetTargetMemberName(), target, _greaterThan)));
                                                                  return v;
                                                              };
            return new ValidationMethod<T>(validation, validationMessage, GetTargetTypeName(), GetTargetMemberName());
        }
    }
}