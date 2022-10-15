namespace MessengerData.Providers
{
    public class UpdateResult<T> : SaveResult where T : class
    {
        private T? _entity;
        public UpdateResult() : base()
        {

        }
        public UpdateResult(string errorMessage) : base(errorMessage)
        {
            
        }
        public UpdateResult(T entity, SaveResult saveResult)
        {
            _entity = entity;
            _result = saveResult.Result;
            ErrorMessage = saveResult.ErrorMessage;
        }
    
        public T? Entity { get { return _entity; } }

        public UpdateResult<T> SetResultTrue(T entity)
        {
            SetResultTrue();
            _entity = entity;
            return this;
        }
    }
}
