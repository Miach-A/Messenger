namespace MessengerData.Providers
{
    public class UpdateResult<T> : SaveResult where T : class
    {
        public UpdateResult(T entity) : base()
        {
            Entity = entity;
        }
        public UpdateResult(T entity, bool result, string errorMessage) : base(result, errorMessage)
        {
            Entity = entity;
        }
        public UpdateResult(T entity, SaveResult saveResult)
        {
            Entity = entity;
            Result = saveResult.Result;
            ErrorMessage = saveResult.ErrorMessage;
        }
        public T Entity { get; set; } = null!;
    }
}
