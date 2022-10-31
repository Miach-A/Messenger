namespace MessengerData.Providers
{
    public class SaveResult
    {
        protected bool _result = false;
        public SaveResult()
        {

        }
        public SaveResult(string errorMessage)
        {
            ErrorMessage.Add(errorMessage);
        }
        //public bool Result { get { return _result; } }
        public List<string> ErrorMessage { get; set; } = new List<string>();         
        public SaveResult SetResultTrue()
        {
            _result = true;
            return this;
        }

        public static implicit operator bool(SaveResult result)
        {
            return result._result;
        }
    }
}
