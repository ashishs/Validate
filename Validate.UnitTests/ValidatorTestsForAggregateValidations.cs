using NUnit.Framework;

namespace Validate.UnitTests
{
    [TestFixture]
    public class ValidatorTestsForAggregateValidations
    {
        [Test]
        public void ShouldPassAndValidation()
        {
            var obj = new { Name = "Ashish", Goals = 15, Fouls = 100 };
            var validator = obj.Validate()
                .And("Some validation failed", 
                     o => o.Validate().IsNotNull(v => v, "Object should be null"), 
                     o => o.Validate().IsNotNull(v => v.Name, "Name should be null")
                    );

            Assert.IsTrue(validator.IsValid);
        }

        [Test]
        public void ShouldFailAndValidation()
        {
            var obj = new { Name = "Ashish", Goals = 15, Fouls = 100 };
            var validator = obj.Validate()
                .And("Some validation failed", 
                     o => o.Validate().IsNotNull(v => v, "Object should be null"), 
                     o => o.Validate().IsNull(v => v.Name, "Name should be null"),
                     o => o.Validate().IsGreaterThan(v => v.Goals, 20, "Should have more than 20 goals")
                    );

            Assert.IsFalse(validator.IsValid);
        }

        [Test]
        public void ShouldPassOrValidation()
        {
            var obj = new { Name = "Ashish", Goals = 15, Fouls = 100 };
            var validator = obj.Validate()
                .Or("Some validation failed", 
                    o => o.Validate().IsNotNull(v => v, "Object should not be null"), 
                    o => o.Validate().IsNull(v => v.Name, "Name should be null")
                   );

            Assert.IsTrue(validator.IsValid);
        }

        [Test]
        public void ShouldFailOrValidation()
        {
            var obj = new Person { Name = "Ashish", Goals = 15, Fouls = 100 };
            var validator = obj.Validate()
                .Or("Some validation failed", 
                    o => o.Validate().IsNull(v => v, "Object should not be null"), 
                    o => o.Validate().IsNull(v => v.Name, "Name should be null")
                   );

            Assert.IsFalse(validator.IsValid);
        }

        [Test]
        public void ShouldFailOrWithNestedAndValidation()
        {
            var obj = new Person { Name = "Ashish", Goals = 15, Fouls = 100 };
            var validator = obj.Validate()
                .Or("Or validation failed", 
                    o => o.Validate().IsNull(v => v, "Object should be null"),
                    o => o.Validate().And("And validations failed.", 
                                            x => x.Validate().IsNull(v => v.Name, "Name should be null"),
                                            x => x.Validate().IsGreaterThan(v => v.Age, 25,"Age should be greater than 25")),
                    o => o.Validate().IsNull(v => v.Name, "Name should be null")
                   );

            Assert.IsFalse(validator.IsValid);
        }

        [Test]
        public void ShouldFailIfThen()
        {
            var obj = new Person { Name = "Ashish", Goals = 15, Fouls = 100, Age = 26 };
            var validator = obj.Validate()
                .IfThen(p => p.Goals > 10, "Failed vallidation", p => p.Fouls < 5, p => p.Age > 0);

            Assert.IsFalse(validator.IsValid);
        }

        [Test]
        public void ShouldPassIfThen()
        {
            var obj = new Person { Name = "Ashish", Goals = 15, Fouls = 100, Age = 26 };
            var validator = obj.Validate()
                .IfThen(p => p.Goals < 10, "Failed validation", p => p.Fouls < 5, p => p.Age > 0);

            Assert.IsTrue(validator.IsValid);
        }
    }
}