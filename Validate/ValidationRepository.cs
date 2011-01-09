using System;
using System.Collections.Generic;
using System.Linq;
using Validate.Extensions;

namespace Validate
{
    public class ValidationRepository : IValidationRepository
    {
        List<object> _validations = new List<object>();
        private readonly object _lock = new object();

        public void Save<T>(Validation<T> validationToSave)
        {
            IValidationMetadata metadata = validationToSave;
            lock (_lock)
            {
                if(_validations.Select(v => (IValidationMetadata)v).Any(m => StringX.EqualsIgnoreCase(m.Alias, metadata.Alias) && m.ValidationTargetType == metadata.ValidationTargetType))
                {
                    throw new ArgumentException("A validation for same type with the same alias already exists. | Type: {0} | Alias: {1} ".WithFormat(metadata.ValidationTargetType.FullName, metadata.Alias), "validationToSave");
                }
                
                _validations.Add(validationToSave);
            }
        }

        public Validation<T> Get<T>(string validationAlias)
        {
            var type = typeof (T);
            for(int i = 0; i< _validations.Count; i++)
            {
                var metadata = (IValidationMetadata) _validations[i];
                if(metadata.ValidationTargetType == type && metadata.Alias.EqualsIgnoreCase(validationAlias))
                    return (Validation<T>)_validations[i];
            }
            throw new ArgumentException("Could not find any validation matching the given type and alias. | Type: {0} | Alias {1}".WithFormat(type.FullName, validationAlias));
        }

        public void Reset()
        {
            _validations = new List<object>();
        }
    }
}