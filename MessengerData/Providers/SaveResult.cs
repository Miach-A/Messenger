namespace MessengerData.Providers
{
    public class SaveResult
    {
        public SaveResult()
        {

        }
        public SaveResult(bool result, string errorMessage)
        {
            Result = result;
            ErrorMessage.Add(errorMessage);
        }
        public bool Result { get; set; } = false;
        public List<string> ErrorMessage { get; set; } = new List<string>();
    }
}
