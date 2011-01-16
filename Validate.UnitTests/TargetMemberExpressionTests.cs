using System;
using System.Linq.Expressions;
using NUnit.Framework;

namespace Validate.UnitTests
{
    [TestFixture]
    public class TargetMemberExpressionTests
    {
        [Test]
        public void ShouldBeAbleToDetermineTypeMemberAndPath()
        {
            Expression<Func<Person, string>> pe1 = (p) => p.Name;
            var metadata = new TargetMemberExpression<Person>(pe1).GetTargetMemberMetadata();
            Assert.AreEqual(typeof(Person), metadata.Type);
            Assert.AreEqual(typeof(Person).GetProperty("Name"), metadata.Member);
            Assert.AreEqual("Person.Name", metadata.Path);
        }

        [Test]
        public void ShouldBeAbleToDetermineTypeMemberAndPathForClass()
        {
            Expression<Func<Person, Person>> pe1 = (p) => p;
            var metadata = new TargetMemberExpression<Person>(pe1).GetTargetMemberMetadata();
            Assert.AreEqual(typeof(Person), metadata.Type);
            Assert.IsNull(metadata.Member);
            Assert.AreEqual("Person", metadata.Path);
        }

        [Test]
        public void ShouldBeAbleToDetermineTypeMemberAndPathForMethod()
        {
            Expression<Func<Person, string >> pe1 = (p) => p.ToString();
            var metadata = new TargetMemberExpression<Person>(pe1).GetTargetMemberMetadata();
            Assert.AreEqual(typeof(Person), metadata.Type);
            Assert.AreEqual("Person.ToString", metadata.Path);
        }
    }
}