﻿using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;

namespace Validate
{
    public class IsNullOrEmptyTargetMemberExpression<T, U> : TargetMemberValidationExpression<T, U> where U : IEnumerable
    {
        public IsNullOrEmptyTargetMemberExpression(Expression<Func<T, U>> targetMemberExpression, ValidationMessage message)
            : base(targetMemberExpression, message)
        {
        }

        public override ValidationMethod<T> GetValidationMethod()
        {
            var compiledSelector = targetMemberExpression.Compile();
            Func<Validator<T>, Validator<T>> validation = (v) =>
                                                              {
                                                                  var target = compiledSelector(v.Target);
                                                                  if (target != null && target.OfType<object>().FirstOrDefault() != null)
                                                                      v.AddError(new ValidationError(GetValidationMessage(), target, cause: GetValidationMessage()));
                                                                  return v;
                                                              };
            return new ValidationMethod<T>(validation, GetValidationMessage(), GetMethodAndMember().Key, GetMethodAndMember().Value);
        }
    }
}