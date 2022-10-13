namespace MessengerData.Providers
{
    public class UpdateResult<T> : SaveResult where T : class
    {
        public UpdateResult() : base()
        {

        }
        public UpdateResult(string errorMessage) : base(errorMessage)
        {

        }
        public UpdateResult(SaveResult saveResult)
        {
            Result = saveResult.Result;
            ErrorMessage = saveResult.ErrorMessage;
        }
        public T? Entity { get; set; }
    }
}
