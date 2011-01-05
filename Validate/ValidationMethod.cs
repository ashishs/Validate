﻿using System;
using System.Diagnostics;
using System.Reflection;
using Validate.Extensions;

namespace Validate
{
    public interface IValidationMethod
    {
        string TargetMemberOrMethod {get;}
        string TargetType {get;}
    }

    public class ValidationMethod<T> : IValidationMethod
    {
        public Func<Validator<T>, Validator<T>> ExecutableValidationMethod  { get; private set; }
        public ValidationMessage Message { get; private set; }

        public string TargetType { get; private set; }
        public string TargetMemberOrMethod { get; private set; }

        public ValidationMethod(Func<Validator<T>, Validator<T>> executableValidationMethod, ValidationMessage message, string targetType = null, string targetMemberOrMethod = null)
        {
            ExecutableValidationMethod = executableValidationMethod;
            Message = message;
            TargetType = targetType;
            TargetMemberOrMethod = targetMemberOrMethod;
        }

        public Validator<T> RunAgainst(Validator<T> validator)
        {
            try
            {
                validator.AddValidation(this);
                return validator.ValidateFurther ? ExecutableValidationMethod(validator) : validator;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                validator.AddError(new ValidationError(Message.Populate(targetValue: validator.Target).ToString(), validator.Target, cause: "{{{0} : Exception : {1}}}".WithFormat(Message, ex.ToString())));
            }
            return validator;
        }

    }
}