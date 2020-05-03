using covidSim.Services;
using Microsoft.AspNetCore.Mvc;

namespace covidSim.Controllers
{
    [Route("api/restart")]
    
    public class RestartController : Controller
    {
        [HttpPost]
        public IActionResult Restart()
        {
            Game.Instance.Restart();
            var game = Game.Instance;
            return Ok(game);
        }
    }
}