using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement.Mvc;
using SecretsStorage.Constants;
using SecretsStorage.Models;
using System.Net.Mime;

namespace SecretsStorage.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ApiController]
    [Route("api/[controller]")]
    public class SecretsController : ControllerBase
    {
        private readonly ILogger<SecretsController> logger;
        private readonly ISecretsService secretsService;

        public SecretsController(ILogger<SecretsController> logger, ISecretsService secretsService)
        {
            this.logger = logger;
            this.secretsService = secretsService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Secrets Get() => secretsService.GetSecrets();

        [HttpGet(nameof(Feature))]
        [FeatureGate(FeatureManagement.FeatureEndpoint)]
        public ActionResult Feature() => Ok("Feature endpoint enabled");
    }
}
