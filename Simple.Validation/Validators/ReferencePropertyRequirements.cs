namespace Simple.Validation.Validators
{
    public class ReferencePropertyRequirements
    {
        public bool Required { get; set; }

        public bool Cascade { get; set; }

        public string Message { get; set; }

        public ValidationResultSeverity Severity { get; set; }

        public object Type { get; set; }

        public string[] RulesSets { get; set; }

        public ReferencePropertyRequirements()
        {
            Severity = ValidationResultSeverity.Error;
        }
    }
}