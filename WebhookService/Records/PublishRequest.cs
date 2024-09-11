namespace WebhookService.Records
{
    public record PublishRequest(string Topic, object Message);
    
}
