using Ninject.Modules;

namespace Simple.Validation.Ninject
{
    public class SimpleValidationNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IValidatorProvider>().To<NinjectValidatorProvider>();
            Bind<IValidationEngine>().To<DefaultValidationEngine>();
        }
    }
}
