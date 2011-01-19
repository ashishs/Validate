using System;
using System.Reflection;
using Validate.Extensions;
using System.Linq;

namespace Validate
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class ValidateUsingAttribute : Attribute
    {
        private Type _abstractClassValidator;
        private string _validationAlias;

        private static MethodInfo _validateUsingAbstractValidator = typeof(GenericX).GetMethods(BindingFlags.Public | BindingFlags.Static)
                                                                    .Where(m => m.Name == "ValidateUsing" && m.GetParameters().Last().ParameterType == typeof(Type)).Single();
        private static MethodInfo _validateUsingValidation = typeof(GenericX).GetMethods(BindingFlags.Public | BindingFlags.Static)
                                                                    .Where(m => m.Name == "ValidateUsing" && m.GetParameters().Last().ParameterType == typeof(string)).Single();

        public ValidateUsingAttribute(string validationAlias)
        {
            _validationAlias = validationAlias;
        }

        public ValidateUsingAttribute(Type abstractClassValidator)
        {
            _abstractClassValidator = abstractClassValidator;
        }

        public virtual AbstractValidator Validate(object target, Type targetType)
        {
            var info = typeof (GenericX).GetMethods().Select(m => new {m.Name, Parameters = m.GetParameters().ToList()});
            if (_abstractClassValidator != null)
            {
                var method = GetValidateUsingClassValidatorMethodInfo(targetType);
                return (AbstractValidator)method.Invoke(null, new[] { target, _abstractClassValidator });
            }
            if(_validationAlias != null)
            {
                var method = GetValidateUsingStringMethodInfo(targetType);
                return (AbstractValidator) method.Invoke(null, new[] { target, _validationAlias });
            }
            throw new ArgumentException("One of AbstractClassValidator or ValidationAlias must be set.");
        }

        private MethodInfo GetValidateUsingStringMethodInfo(Type targetType)
        {   
            var genericMethod = _validateUsingValidation.MakeGenericMethod(new[] {targetType});
            return genericMethod;
        }

        private MethodInfo GetValidateUsingClassValidatorMethodInfo(Type targetType)
        {   
            var genericMethod = _validateUsingAbstractValidator.MakeGenericMethod(new[] { targetType });
            return genericMethod;
        }
    }
}