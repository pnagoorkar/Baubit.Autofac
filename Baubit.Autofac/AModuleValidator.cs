using Baubit.Validation;

namespace Baubit.Autofac
{
    public abstract class AModuleValidator<TModule> : AValidator<TModule> where TModule : AModule
    {
        protected AModuleValidator()
        {

        }
    }
}
