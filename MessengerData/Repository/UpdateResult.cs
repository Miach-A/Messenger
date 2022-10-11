namespace MessengerData.Repository
{
    public class UpdateResult<T> where T : class
    {
        public bool Result { get; set; }
        public T? Entity { get; set; }
        public List<string> ErrorMessage { get; set; } = new List<string>();
    }
}
