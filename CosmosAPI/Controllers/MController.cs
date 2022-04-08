using CosmosAPI.Models;
using CosmosAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CosmosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MController : Controller
        {
       /*     private readonly ICosmosDbService _cosmosDbService;
            public MediumController(ICosmosDbService cosmosDbService)
            {
                _cosmosDbService = cosmosDbService;
            }

            [ActionName("Index")]
            public async Task<IActionResult> Index()
            {
                return View(await _cosmosDbService.GetMediumsAsync("SELECT * FROM c"));
            }

            [ActionName("Create")]
            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            [ActionName("Create")]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> CreateAsync([Bind("Id,MediumType")] Medium item)
            {
                if (ModelState.IsValid)
                {
                    item.Id = Guid.NewGuid().ToString();
                    await _cosmosDbService.AddMediumAsync(item);
                    return RedirectToAction("Index");
                }

                return View(item);
            }

            [HttpPost]
            [ActionName("Edit")]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> EditAsync([Bind("Id,MediumType")] Medium item)
            {
                if (ModelState.IsValid)
                {
                    await _cosmosDbService.UpdateMediumAsync(item.Id, item);
                    return RedirectToAction("Index");
                }

                return View(item);
            }

            [ActionName("Edit")]
            public async Task<ActionResult> EditAsync(string id)
            {
                if (id == null)
                {
                    return BadRequest();
                }

                Medium item = await _cosmosDbService.GetMediumAsync(id);
                if (item == null)
                {
                    return NotFound();
                }

                return View(item);
            }

            [ActionName("Delete")]
            public async Task<ActionResult> DeleteAsync(string id)
            {
                if (id == null)
                {
                    return BadRequest();
                }

                Medium item = await _cosmosDbService.GetMediumAsync(id);
                if (item == null)
                {
                    return NotFound();
                }

                return View(item);
            }

            [HttpPost]
            [ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> DeleteConfirmedAsync([Bind("Id")] string id)
            {
                await _cosmosDbService.DeleteMediumAsync(id);
                return RedirectToAction("Index");
            }

            [ActionName("Details")]
            public async Task<ActionResult> DetailsAsync(string id)
            {
                return View(await _cosmosDbService.GetMediumAsync(id));
            }
       */
    }
}
