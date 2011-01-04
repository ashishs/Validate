using System.Collections.Generic;
using NUnit.Framework;
using Validate.Extensions;

namespace Validate.UnitTests
{
    [TestFixture]
    public class ValidatorTestsForValidationExceptions
    {
        [Test]
        public void ShouldThrowValidationExceptionForIsNotNull()
        {
            List<string> values = null;
            TestDelegate validationCode = () => values.Validate(new ValidationOptions
                                                                    {
                                                                        ThrowValidationExceptionOnValidationError = true,
                                                                        ValidationResultToExceptionTransformer = new ValidationResultToValidationExceptionTransformer()
                                                                    })
                                                    .IsNotNull(v => v, "Values canot be null");

            Assert.Throws<ValidationException>(validationCode);
        }

        [Test]
        public void ShouldNotThrowValidationExceptionForIsNotNull()
        {
            List<string> values = null;
            TestDelegate validationCode = () => values.Validate(new ValidationOptions
                                                                    {
                                                                        ThrowValidationExceptionOnValidationError = false,
                                                                        ValidationResultToExceptionTransformer = new ValidationResultToValidationExceptionTransformer()
                                                                    })
                                                    .IsNotNull(v => v, "Values canot be null");

            Assert.DoesNotThrow(validationCode);
        }

        [Test]
        public void ShouldNotThrowValidationExceptionForIsNotNullWhenStopOnFistErrorIsFalse()
        {
            List<string> values = null;
            TestDelegate validationCode = () => values.Validate(new ValidationOptions
                                                                    {
                                                                        StopOnFirstError = false,
                                                                        ThrowValidationExceptionOnValidationError = true,
                                                                        ValidationResultToExceptionTransformer = new ValidationResultToValidationExceptionTransformer()
                                                                    })
                                                    .IsNotNull(v => v, "Values canot be null");

            Assert.DoesNotThrow(validationCode);
        }

        [Test]
        public void ShouldThrowValidationExceptionForIsNotNullWhenStopOnFistErrorIsFalseWhenThrowIsCalled()
        {
            List<string> values = null;
            TestDelegate validationCode = () => values.Validate(new ValidationOptions
                                                                    {
                                                                        StopOnFirstError = false,
                                                                        ThrowValidationExceptionOnValidationError = true,
                                                                        ValidationResultToExceptionTransformer = new ValidationResultToValidationExceptionTransformer()
                                                                    })
                                                    .IsNotNull(v => v, "Values canot be null")
                                                    .Throw();

            Assert.Throws<ValidationException>(validationCode);
        }
    }
}