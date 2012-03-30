namespace Simple.Validation
{
    public class ValidationResult
    {
        public string PropertyName { get; set; }

        public object Type { get; set; }

        public ValidationResultSeverity Severity { get; set; }

        public string Message { get; set; }

        public object Context { get; set; }

        public ValidationResult()
        {
            this.Severity = ValidationResultSeverity.Error;
        }
    }
}
