using System;
using System.Linq.Expressions;
using Validate.Extensions;

namespace Validate.ValidationExpressions
{
    public class PassesSavedValidation<T,U> : TargetMemberValidationExpression<T, U>
    {
        private readonly string _validationAlias;
        private readonly IValidationRepository _validationRepository;

        public PassesSavedValidation(Expression<Func<T, U>> targetMemberExpression, string validationAlias, IValidationRepository validationRepository) : base(targetMemberExpression)
        {
            _validationAlias = validationAlias;
            _validationRepository = validationRepository;
        }

        public override ValidationMethod<T> GetValidationMethod()
        {
            var compiledSelector = TargetMemberExpression.Compile();
            Func<Validator<T>, Validator<T>> validation = (v) =>
                                                              {   
                                                                  var target = compiledSelector(v.Target);
                                                                  var validationToRun = _validationRepository.Get<U>(_validationAlias);
                                                                  var validatorForU = validationToRun.RunAgainst(target);
                                                                  foreach (var validationError in validatorForU.Errors)
                                                                  {
                                                                      // TODO: Add validation also to Validations
                                                                      v.AddError(validationError);
                                                                  }
                                                                  return v;
                                                              };
            return new ValidationMethod<T>(validation, null, TargetMemberMetadata);
        }
    }
}