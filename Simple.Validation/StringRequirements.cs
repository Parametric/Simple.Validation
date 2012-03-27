using System;
using System.Text.RegularExpressions;

namespace Simple.Validation
{
    public class StringRequirements
    {
        public int? MinLength { get; set; }

        public int? MaxLength { get; set; }

        public bool Required { get; set; }

        public bool IgnoreWhiteSpace { get; set; }

        private string _regularExpression;
        public string RegularExpression
        {
            get { return _regularExpression; }
            set
            {
                _regularExpression = value;
                _regex = null;
                if (! string.IsNullOrWhiteSpace(value))
                    _regex = new Regex(value);
            }
        }

        private Regex _regex;

        public bool IsRegExMatch(string value)
        {
            if (_regex == null)
                throw new InvalidOperationException();

            return _regex.IsMatch(value);
        }

        internal string GetValueToValidate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                if (IgnoreWhiteSpace)
                    return null;

            if (IgnoreWhiteSpace)
                return value.Trim();

            return value;

        }
    }
}
