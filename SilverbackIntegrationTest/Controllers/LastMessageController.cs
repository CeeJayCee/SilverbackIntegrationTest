using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SilverbackIntegrationTest.Controllers {
    [ApiController]
    [Route("/")]
    public class LastMessageController : ControllerBase {

        private readonly ILogger<LastMessageController> _logger;
        private readonly MessageStore _store;

        public LastMessageController(
            ILogger<LastMessageController> logger,
            MessageStore store
        ) {
            _store = store;
            _logger = logger;
        }

        [HttpGet]
        public string Get() {
            return $"Last message was: {_store.Get()}";
        }
    }
}
