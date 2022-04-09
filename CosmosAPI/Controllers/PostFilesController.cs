using CosmosAPI.Models;
using CosmosAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CosmosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PostFilesController : ControllerBase
    {

        private readonly ICosmosDbService _cosmosDbService;
        public PostFilesController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{Postid}")]
        public string Get(string Postid)
        {
            Guid GuidID = Guid.Parse(Postid);
            var results = _cosmosDbService.GetFilesAsync("SELECT f.id, f.Title, f.DateCreated, f.Excerpt, f.URL, f.LightBoxURL FROM Files f join p in f.PostId where p.id = '" + Postid + "'").Result;
            return results;
        }
    }
}
