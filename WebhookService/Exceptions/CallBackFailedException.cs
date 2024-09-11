namespace WebhookService.Exceptions
{
    public class CallBackFailedException : Exception
    {
        public CallBackFailedException(string message) : base(message) { }
    }
}
