﻿using System;
using System.Linq;
using Validate.Extensions;

namespace Validate.ValidationExpressions
{
    public class OrTargetMemberExpression<T> : TargetMemberValidationExpression<T, object>
    {
        private readonly Predicate<T>[] _predicates;
        private readonly Func<T, IValidator>[] _nestedValidators;
        private bool _useNestedValidators;
        
        public OrTargetMemberExpression(ValidationMessage message, Func<T, IValidator>[] nestedValidators)
            : base(message)
        {
            _nestedValidators = nestedValidators;
            _useNestedValidators = true;
        }

        public OrTargetMemberExpression(ValidationMessage message, Predicate<T>[] predicates)
            : base(message)
        {
            _predicates = predicates;
            _useNestedValidators = false;
        }

        public override ValidationMethod<T> GetValidationMethod()
        {
            var validationMessage = Message.Populate(targetType: typeof(T).FriendlyName());
            if (_useNestedValidators)
            {
                Func<Validator<T>, Validator<T>> validation = (v) =>
                {
                    var validators = _nestedValidators.Select(valFunc => valFunc(v.Target));
                    var match = validators.Any(val => val.IsValid);
                    if (!match)
                        v.AddError(new ValidationError(validationMessage.Populate(targetValue: v.Target).ToString(), v.Target, TargetMemberMetadata,
                                    "{{The OR validation for target member {0}.{1} with value {2} failed because {{{3}}} }}".WithFormat(typeof(T).FriendlyName(), "{{ Target member could not be determined }}", v.Target, GetCauses(validators).Join(" And "))));      
                    return v;
                };
                return new ValidationMethod<T>(validation, validationMessage, TargetMemberMetadata);
            }
            else
            {
                Func<Validator<T>, Validator<T>> validation = (v) =>
                {
                    var match = _predicates.Any(p => p(v.Target));
                    if (!match)
                        v.AddError(new ValidationError(validationMessage.Populate(targetValue: v.Target).ToString(), v.Target, TargetMemberMetadata,
                                                                                     "{{The OR validation for target member {0}.{1} with value {2} failed because {{ All of the predicates failed. }} }}".WithFormat(typeof(T).FriendlyName(), "{{ Target member could not be determined }}", v.Target)));      
                    return v;
                };
                return new ValidationMethod<T>(validation, validationMessage, TargetMemberMetadata);
            }
        }
    }
}