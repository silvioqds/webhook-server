namespace WebhookService.Exceptions
{
    public class NotFoundTopicException : Exception
    {
        public NotFoundTopicException(string message):
            base(message) { }
    }
}
