using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using Validate.Extensions;
using Validate.ValidationExpressions;
using System.Collections.Generic;

namespace Validate.UnitTests.Examples.CustomValidationExtensions
{
    public static class MyValidationExtensions
    {
        // Declare the extension method
        public static Validator<T> IsLengthGreaterThan<T,U>(this Validator<T> validator, Expression<Func<T,U>> selector, int lengthGreaterThan, string message = null) where U:IEnumerable
        {
            var validationMessage = message == null ? new ValidationMessage("{TargetType}.{TargetMember} had length greater than the speciefied value."): new ValidationMessage(message);
            var validationExpression = new IsLengthGreaterThanTargetMemberExpression<T, U>(selector, validationMessage, lengthGreaterThan);
            return validationExpression.ValidationMethod.RunAgainst(validator);
        }
    }

    // This class is used to implement the custom validation
    public class IsLengthGreaterThanTargetMemberExpression<T, U> : TargetMemberValidationExpression<T,U> where U:IEnumerable
    {
        private readonly int _lengthGreaterThan;

        public IsLengthGreaterThanTargetMemberExpression(Expression<Func<T, U>> selector, ValidationMessage message, int lengthGreaterThan) : base(selector, message)
        {
            _lengthGreaterThan = lengthGreaterThan;
        }

        public override ValidationMethod<T> GetValidationMethod()
        {
            var validationMessage = Message.Populate(targetType: GetTargetTypeName(TargetMemberExpression), targetMember: GetTargetMemberName(TargetMemberExpression));
            var compiledSelector = TargetMemberExpression.Compile();
            Func<Validator<T>, Validator<T>> validation = (v) =>
            {
                var target = compiledSelector(v.Target);
                if (target == null || target.OfType<object>().Count() <= _lengthGreaterThan)
                    v.AddError(new ValidationError(validationMessage.Populate(targetValue: target).ToString(), target,
                              "{{ The target member {0}.{1} did not have length greater than {2} }}".WithFormat(GetTargetTypeName(TargetMemberExpression), GetTargetMemberName(TargetMemberExpression), _lengthGreaterThan)));
                return v;
            };
            return new ValidationMethod<T>(validation, validationMessage, GetTargetTypeName(TargetMemberExpression), GetTargetMemberName(TargetMemberExpression));
        }
    }

    [TestFixture]
    public class ValidationExamples_WritingCustomValidationExtensions
    {
        [Test]
        public void ShouldPassValidateUsingCustomValidationExtension()
        {
            var values = new List<string>{"One", "Two", "Three"};
            var validator = values.Validate().IsLengthGreaterThan(v => v,2);
            Assert.IsTrue(validator.IsValid);
        }

        [Test]
        public void ShouldFailValidateUsingCustomValidationExtension()
        {
            var values = new List<string> { "One", "Two", "Three" };
            var validator = values.Validate().IsLengthGreaterThan(v => v, 4);
            Assert.IsFalse(validator.IsValid);
            Assert.AreEqual(validator.Errors[0].Message, "List`1[String].Value had length greater than the speciefied value.");
        }
    }
}