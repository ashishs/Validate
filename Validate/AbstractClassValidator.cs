namespace Validate
{
    public abstract class AbstractClassValidator<T>
    {
        protected readonly T Target;

        public AbstractClassValidator(T target)
        {
            Target = target;
        }

        public abstract Validator<T> Validate();
    }
}