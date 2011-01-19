using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Validate.Extensions;

namespace Validate.Mvc
{
    public class AttributedModelValidatorProvider : AssociatedValidatorProvider
    {
        protected override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context, IEnumerable<Attribute> attributes)
        {
            return new[] {new AttributedModelValidator(metadata, context)};
        }
    }
}
