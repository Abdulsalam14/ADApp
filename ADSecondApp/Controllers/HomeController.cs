using ADSecondApp.Models;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Diagnostics;
using System.Text;

namespace ADSecondApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConnectionMultiplexer _cache;

        public HomeController(IConnectionMultiplexer cache)
        {
            _cache=cache;
        }

        public async Task<IActionResult> Index()
        {
            var db = _cache.GetDatabase();
            var movie = await db.ListGetByIndexAsync("movies", 0);
            if (movie.ToString()!=null) ViewBag.Movie=movie.ToString();
            return View();
        }

        public async Task<IActionResult> GetAD()
        {

            var db = _cache.GetDatabase();
            var movie =await db.ListGetByIndexAsync("movies", 0);

            return Ok(movie.ToString());
        }
        public async Task<IActionResult> RemoveAD()
        {
            try
            {
                var db = _cache.GetDatabase();
                await db.ListLeftPopAsync("movies");
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }


    }
}
