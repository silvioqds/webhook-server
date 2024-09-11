using WebhookService.Exceptions;
using WebhookService.Interfaces;
using WebhookService.Records;

namespace WebhookService.Service
{
    public class WebHookService : IWebHookService
    {
        private readonly List<Subscription> _subscriptions = new(); //Inscritos na mémoria, o legal seria criar um portal para cadastrar seus topicos e callbacks
        private readonly HttpClient _httpClient = new();

        //Metodo que serve para adicionar seus "inscritos" no seu webhook
        public void Subscribe(Subscription subscription)
        {
            if(_subscriptions.Contains(subscription)) return; //Não deixo adicionar o mesmo topico para o mesmo callback duas vezes
            _subscriptions.Add(subscription);
        }

        //Metodo que devolve a solicitacao a partir do topico informado pelo cliente webhook 
        public async Task PublishMessage(string topic, object message)
        {           
            var subscribeWebHooks = _subscriptions.Where(x => x.Topic == topic).ToList();

            if (subscribeWebHooks is null || !subscribeWebHooks.Any())
            {
                throw new NotFoundTopicException("Não foi possível encontrar o topico especificado");
            }

            foreach (var webhook in subscribeWebHooks)
            {
                try
                {
                    await _httpClient.PostAsJsonAsync(webhook.Callback, message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro no retorno da chamada ao endpoint - {webhook.Callback}");
                }
            }

            
        }

    }
}
