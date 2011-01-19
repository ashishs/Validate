using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Validate.Extensions;

namespace Validate.Mvc
{
    public class AttributedModelValidator : ModelValidator
    {
        public AttributedModelValidator(ModelMetadata metadata, ControllerContext context) : base(metadata, context)
        {
            
        }

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {   
            var validationAttributes = Metadata.ModelType.GetCustomAttributes(typeof(ValidateUsingAttribute), true);
            foreach (ValidateUsingAttribute validationAttribute in validationAttributes)
            {
                var validator = validationAttribute.Validate(Metadata.Model, Metadata.ModelType);
                foreach (var validationError in validator.Errors)
                {
                    yield return Convert(validationError, Metadata.ModelType);
                }
            }
        }

        private ModelValidationResult Convert(ValidationError validationError, Type modelType)
        {
            return new ModelValidationResult {MemberName = validationError.TargetMemberMetadata.GetRootRelativePath(modelType), Message = validationError.Message};
        }
    }
}