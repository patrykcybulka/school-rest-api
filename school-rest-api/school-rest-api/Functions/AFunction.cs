namespace school_rest_api.Functions
{
    public abstract class AFunction<T>
    {
        public T Model { get; }

        public AFunction(T model)
        {
            Model = model;
        }
    }
}
