using System;
using System.Collections.ObjectModel;
using Validate;
using System.Linq;

namespace Validate
{
    /// <summary>
    /// Interface to be implemented by custom validators which do not use fluent validations
    /// </summary>
    public interface IValidator
    {
        bool IsValid { get; }
        ReadOnlyCollection<ValidationError> Errors { get; }
        void Throw();
    }
}