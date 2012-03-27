using System.Collections.Generic;

namespace Simple.Validation
{
    /// <summary>
    /// Provides a list of ValidationResults for a given object.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    public interface IValidator<in T>
    {
        /// <summary>
        /// Indicates whether or not the validator applies for a given rulesSet.
        /// </summary>
        /// <param name="rulesSet"></param>
        /// <returns></returns>
        bool AppliesTo(string rulesSet);

        /// <summary>
        /// Returns a list of errors and warnings for the object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IEnumerable<ValidationResult> Validate(T value);
    }

}