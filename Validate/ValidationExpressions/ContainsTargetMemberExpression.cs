﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Validate.Extensions;

namespace Validate.ValidationExpressions
{
    public class ContainsTargetMemberExpression<T, U, V> : TargetMemberValidationExpression<T, U> where U : IEnumerable<V>
    {
        private readonly V[] _values;

        public ContainsTargetMemberExpression(Expression<Func<T, U>> targetMemberExpression, ValidationMessage message, params V[] values)
            : base(targetMemberExpression, message)
        {
            _values = values ?? new V[0];
        }

        public override ValidationMethod<T> GetValidationMethod()
        {
            var containsDisplay = _values.Select(v => v.ToString()).Join(" | ");
            var validationMessage = Message.Populate(targetType: TargetMemberMetadata.Type.FriendlyName(), targetMember: TargetMemberMetadata.MemberName, targetValueContains: containsDisplay);
            var compiledSelector = TargetMemberExpression.Compile();
            Func<Validator<T>, Validator<T>> validation = (v) =>
                                                              {
                                                                  var target = compiledSelector(v.Target);
                                                                  var valuesNotContained = _values.Where(val => !target.Contains(val));
                                                                  if (valuesNotContained.Count() > 0)
                                                                      v.AddError(new ValidationError(validationMessage.Populate(targetValue: target).ToString(), target, TargetMemberMetadata,
                                                                                 cause: "{{ The target member {0}.{1} with value {2} did not contain value(s) {{ {3} }} }}".WithFormat(TargetMemberMetadata.Type.FriendlyName(), TargetMemberMetadata.MemberName, target, valuesNotContained.Select(val => val.ToString()).Join(" | "))));
                                                                  return v;
                                                              };
            return new ValidationMethod<T>(validation, validationMessage, TargetMemberMetadata);
        }
    }
}