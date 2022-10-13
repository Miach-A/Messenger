namespace MessengerData.Providers
{
    public class SaveResult
    {
        public SaveResult()
        {

        }
        public SaveResult(string errorMessage)
        {
            ErrorMessage.Add(errorMessage);
        }
        public bool Result { get; set; } = false;
        public List<string> ErrorMessage { get; set; } = new List<string>();
    }
}
