﻿using System;
using System.Linq.Expressions;
using Validate.Extensions;

namespace Validate.ValidationExpressions
{
    public class IsNotEqualToTargetMemberExpression<T, U> : TargetMemberValidationExpression<T, U>
    {
        private readonly U _notEqualTo;

        public IsNotEqualToTargetMemberExpression(Expression<Func<T, U>> targetMemberExpression, U notEqualTo, ValidationMessage message)
            : base(targetMemberExpression, message)
        {
            _notEqualTo = notEqualTo;
        }

        public override ValidationMethod<T> GetValidationMethod()
        {
            var validationMessage = Message.Populate(targetType: TargetMemberMetadata.Type.FriendlyName(), targetMember: TargetMemberMetadata.MemberName, targetValueNotEqualTo: _notEqualTo);
            var compiledSelector = TargetMemberExpression.Compile();
            Func<Validator<T>, Validator<T>> validation = (v) =>
                                                              {
                                                                  var target = compiledSelector(v.Target);
                                                                  if (target.Equals(_notEqualTo))
                                                                      v.AddError(new ValidationError(validationMessage.Populate(targetValue: target).ToString(), target, TargetMemberMetadata,
                                                                                                     cause: "{{The target member {0}.{1} with value {2} was equal to {3}.}}".WithFormat(TargetMemberMetadata.Type.FriendlyName(), TargetMemberMetadata.MemberName, target, _notEqualTo)));
                                                                  return v;
                                                              };
            return new ValidationMethod<T>(validation, validationMessage, TargetMemberMetadata);
        }
    }
}