namespace Simple.Validation.Validators
{
    public static class BooleanPropertyExtensions
    {
        public static PropertyValidator<TContext, bool> IsTrue<TContext>(this PropertyValidator<TContext, bool>  self)
        {
            self.Assert((t, p) => p);
            return self;
        }

        public static PropertyValidator<TContext, bool> IsFalse<TContext>(this PropertyValidator<TContext, bool> self)
        {
            self.Assert((t, p) => !p);
            return self;
        }

        public static PropertyValidator<TContext, bool?> IsTrue<TContext>(this PropertyValidator<TContext, bool?> self)
        {
            self.Assert((t, p) => p.GetValueOrDefault());
            return self;
        }

        public static PropertyValidator<TContext, bool?> IsFalse<TContext>(this PropertyValidator<TContext, bool?> self)
        {
            self.Assert((t, p) => p.HasValue && !p.Value);
            return self;
        } 
    }
}