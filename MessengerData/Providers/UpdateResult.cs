namespace MessengerData.Providers
{
    public class UpdateResult<T> : SaveResult where T : class
    {
        public UpdateResult() : base()
        {

        }
        public UpdateResult(bool result, string errorMessage) : base(result, errorMessage)
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
