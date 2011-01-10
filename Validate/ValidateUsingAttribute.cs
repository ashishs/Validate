using System;
using Validate.Extensions;

namespace Validate
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class ValidateUsingAttribute : Attribute
    {
        private Type _abstractClassValidator;
        private string _validationAlias;

        public ValidateUsingAttribute(string validationAlias)
        {
            _validationAlias = validationAlias;
        }

        public ValidateUsingAttribute(Type abstractClassValidator)
        {
            _abstractClassValidator = abstractClassValidator;
        }

        public virtual Validator<T> Validate<T>(T target)
        {
            if (_abstractClassValidator != null)
            {   
                return target.ValidateUsing(_abstractClassValidator);
            }
            if(_validationAlias != null)
            {
                return target.ValidateUsing(_validationAlias);
            }
            throw new ArgumentException("One of AbstractClassValidator or ValidationAlias must be set.");
        }
    }
}