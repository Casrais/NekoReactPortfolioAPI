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
    public class PostController : ControllerBase
    {

        private readonly ICosmosDbService _cosmosDbService;
        public PostController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }
        // GET: api/<ValuesController>
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public string Get()
        {
            var results = _cosmosDbService.GetPostsAsync("SELECT * FROM c").Result;
            return results;
        }

        // GET api/<ValuesController>/5
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{id}")]
        public string Get(string id)
        {
            try
            {
                Guid GuidID = Guid.Parse(id);
            var result = _cosmosDbService.GetPostsAsync("SELECT top 1 * FROM c WHERE c.id = '"+id+"'").Result;
            return result;
            }
            catch
            {
                return "Error";
            }
        }

        // POST api/<ValuesController>
        [HttpPost]
        public string Post([FromBody] Post value)
        {
            
            if (value != null)
            {
                value.id = Guid.NewGuid();
                try
                {
                    _cosmosDbService.AddPostAsync(value);
                    return value.id.ToString();
                }
                catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine(ex);
                }

            }
            return "Error: did not post.";
            
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public string Put(string id, [FromBody] Post value)
        {
                string response = Get(id);
                if (response != "" & response != null)
                {
                    try
                    {
                        value.id = Guid.Parse(id);
                        _cosmosDbService.UpdatePostAsync(Guid.Parse(id), value);
                        return "Successfully updated Post " + id;
                    }
                    catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        Console.WriteLine(ex);
                        return "Error: did not update Post " + id;
                    }

                }
                else
                {
                    return "Error: Post " + id + " not found.";
                }
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public string Delete(string id)
        {
                string response = Get(id);
                if (response != "" & response != null)
                {
                    try
                    {
                        _cosmosDbService.DeletePostAsync(Guid.Parse(id));
                        return "Successfully deleted Post " + id;
                    }
                    catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        Console.WriteLine(ex);
                        return "Error: did not delete Post " + id;
                    }

                }
                else
                {
                    return "Error: Post " + id + " not found.";
                }
            }
    }
}
