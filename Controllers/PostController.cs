using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WEB_API_DI_Test.Models;


using System.Collections.Generic;




namespace WEB_API_DI_Test.Controllers
{


    [ApiController]
    [Route("post")]
    public class PostController : ControllerBase
    {

        private readonly IDatabase DATABASE;

        public PostController(IDatabase database)
        {
            DATABASE = database;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = DATABASE.GetPost();
            return Ok(result);
            
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int Id)
        {
            var result = DATABASE.GetPostByID(Id);
            return Ok(result);

        }

        [HttpPost]
        public IActionResult PostAdd(Post post)
        {

            return Ok(DATABASE.PostPost(post));
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePost(int Id)
        {
            return Ok(DATABASE.DeletePost(Id));
        }


        //[HttpPatch("{id}")]
        //public IActionResult PatchAuthor([FromBody]JsonPatchDocument<Post> patch, int Id)
        //{

        //}
    }
}