using ADFirstApp.Models;
using ADFirstApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;

namespace ADFirstApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddMessageToQueue(string message)
        {
            try
            {
                AzQueueService queue = new AzQueueService("movienames");
                string base64tring=Convert.ToBase64String(Encoding.UTF8.GetBytes(message));
                await queue.SendMessageAsync(base64tring);;
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> DeleteMessageFromQueue()
        {
            try
            {
                AzQueueService queue = new AzQueueService("movienames");
                var result = await queue.RetrieveNextMessageAsync();
                await queue.DeleteMessage(result.MessageId, result.PopReceipt);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
