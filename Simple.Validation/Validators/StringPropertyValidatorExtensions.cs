using System;
using System.Text.RegularExpressions;

namespace Simple.Validation.Validators
{
    public static class StringPropertyValidatorExtensions
    {



        public static StringPropertyValidator<T> Length<T>(this StringPropertyValidator<T> self,  int? minLength, int? maxLength = null)
        {
            if (minLength.HasValue)
                self.Assert((t, s) => s.Length >= minLength);

            if (maxLength.HasValue)
                self.Assert((t, s) => s.Length <= maxLength);

            return self;
        }

        public static StringPropertyValidator<T> Matches<T>(this StringPropertyValidator<T> self, string regularExpression, RegexOptions options = RegexOptions.None)
        {
            self.Assert((t, s) =>
                            {
                                var result = Regex.IsMatch(s, regularExpression, options);

                                return result;

                            });

            return self;
        }
    }
}