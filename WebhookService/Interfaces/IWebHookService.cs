using WebhookService.Records;

namespace WebhookService.Interfaces
{
    public interface IWebHookService
    {
        void Subscribe(Subscription subscription);
        Task PublishMessage(string topic, object message);
    }
}
