namespace AspCore.WebApi.Authentication.General
{
    public class CustomError
    {
        public string Error { get; }

        public CustomError(string message)
        {
            Error = message;
        }
    }
}
