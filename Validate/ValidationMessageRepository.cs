using System;

namespace Validate
{
    // TODO: Need to clean this up.
    public class ValidationMessageRepository
    {
        private ValidationMessageRepository(){}

        static ValidationMessageRepository _instance = new ValidationMessageRepository();

        public static ValidationMessageRepository Instance
        {
            get { return _instance; }
            set { _instance = value; }
        }

        public virtual ValidationMessage GetValidationMessageForIsNull()
        {
            return new ValidationMessage("{TargetType}.{TargetMember} should be null.", true);
        }

        public virtual ValidationMessage GetValidationMessageForIsNotNull()
        {
            return new ValidationMessage("{TargetType}.{TargetMember} should not be null.", true);
        }

        public virtual ValidationMessage GetValidationMessageForIsNotNullOrEmpty()
        {
            return new ValidationMessage("{TargetType}.{TargetMember} should not be null or empty.", true);
        }

        public virtual ValidationMessage GetValidationMessageForIsNullOrEmpty()
        {
            return new ValidationMessage("{TargetType}.{TargetMember} should be null or empty.", true);
        }

        public virtual ValidationMessage GetValidationMessageForIsEqualTo()
        {
            return new ValidationMessage("{TargetType}.{TargetMember} should be equal to {TargetValueEqualTo}.", true);
        }

        public virtual ValidationMessage GetValidationMessageForIsNotEqualTo()
        {
            return new ValidationMessage("{TargetType}.{TargetMember} should not be equal to {TargetValueNotEqualTo}.", true);
        }

        public virtual ValidationMessage GetValidationMessageForIsGreaterThan()
        {
            return new ValidationMessage("{TargetType}.{TargetMember} should be greater than {TargetValueGreaterThan}.", true);
        }

        public virtual ValidationMessage GetValidationMessageForIsLesserThan()
        {
            return new ValidationMessage("{TargetType}.{TargetMember} should be lesser than {TargetValueLesserThan}.", true);
        }

        public virtual ValidationMessage GetValidationMessageForIsBetween()
        {
            return new ValidationMessage("{TargetType}.{TargetMember} should be between \"{TargetValueGreaterThan}\" and \"{TargetValueLesserThan}\".", true);
        }

        public virtual ValidationMessage GetValidationMessageForIsNotBetween()
        {
            return new ValidationMessage("{TargetType}.{TargetMember} should not be between \"{TargetValueGreaterThan}\" and \"{TargetValueLesserThan}\".", true);
        }

        public virtual ValidationMessage GetValidationMessageForIsTrue()
        {
            return new ValidationMessage("{TargetType}.{TargetMember} should be true.", true);
        }

        public virtual ValidationMessage GetValidationMessageForIsFalse()
        {
            return new ValidationMessage("{TargetType}.{TargetMember} should be false.", true);
        }

        public virtual ValidationMessage GetValidationMessageForMatchesRegex()
        {
            return new ValidationMessage("{TargetType}.{TargetMember} should match pattern \"{TargetValueMatchesRegex}\".", true);
        }

        public virtual ValidationMessage GetValidationMessageForContains()
        {
            return new ValidationMessage("{TargetType}.{TargetMember} should contain { {TargetValueContains} }.", true);
        }

        public virtual ValidationMessage GetValidationMessageForIsOneOf()
        {
            return new ValidationMessage("{TargetType}.{TargetMember} should be one of { {TargetValueIsOneOf} }.", true);
        }
    }
}