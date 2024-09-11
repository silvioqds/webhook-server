using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using WebhookService.Exceptions;
using WebhookService.Interfaces;
using WebhookService.Records;
using WebhookService.Service;

namespace WebhookService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebHookController : ControllerBase
    {
        private readonly ILogger<WebHookController> _logger;
        private readonly IWebHookService _ws;
        public WebHookController(ILogger<WebHookController> logger, IWebHookService ws)
        {
            _logger = logger;
            _ws = ws;
        }

        /// <summary>
        /// Método que recebe os inscritos
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="sub"></param>
        /// <returns></returns>
        [HttpPost("subscribe")]
        public IActionResult Subscribe(Subscription sub)
        {
            try
            {
                _ws.Subscribe(sub);
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao se inscrever no webhook");
            }
        }

        /// <summary>
        /// Método que publica o retorno da solicitacão no callback do inscrito
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("publish")]
        public async Task<IActionResult> Publish(PublishRequest request)
        {
            // Adiciona um log para indicar o recebimento de uma requisição
            _logger.LogInformation($"Recebendo publicação para o tópico: {request.Topic}");

            try
            {
                await _ws.PublishMessage(request.Topic, request.Message);

                _logger.LogInformation($"Publicação bem-sucedida para o tópico: {request.Topic}");
                return StatusCode(200);
            }
            catch (NotFoundTopicException nftex)
            {
                _logger.LogError(nftex, nftex.Message);
                return StatusCode(500, nftex.Message);
            }
            catch (CallBackFailedException cbfex)
            {
                _logger.LogError(cbfex, cbfex.Message);
                return StatusCode(500, cbfex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro não detectado, contate o administrador");
            }
        }

    }
}
