namespace school_rest_api.Functions.Commands
{
    public abstract class ACommand<T>
    {
        public T Model { get; }

        public ACommand(T model)
        {
            Model = model;
        }
    }
}
