using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiCore.Models;
using WebApiCore.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiCore.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        public ITodoRepository TodoRepository { get; }

        public TodoController(ITodoRepository repository)
        {
            TodoRepository = repository;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<TodoItem> Get()
        {
            return TodoRepository.GetAll();
        }

        //Name = "GetTodo" creates a named route and allows you to link to this route in an HTTP Response
        // GET api/values/5
        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult Get(string id)
        {
            var item = TodoRepository.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]TodoItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            TodoRepository.Add(item);
            return CreatedAtRoute("GetTodo", new { id = item.Key }, item);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody]TodoItem item)
        {
            if (item == null || item.Key != id)
            {
                return BadRequest();
            }

            var todo = TodoRepository.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            TodoRepository.Update(item);
            return new NoContentResult();
        }

        //This overload is similar to the previously shown Update, but uses HTTP PATCH. The response is 204 (No Content).
        [HttpPatch("{id}")]
        public IActionResult Update([FromBody] TodoItem item, string id)
        {
            if (item == null)
            {
                return BadRequest();
            }

            var todo = TodoRepository.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            item.Key = todo.Key;

            TodoRepository.Update(item);
            return new NoContentResult();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var todo = TodoRepository.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            TodoRepository.Remove(id);
            return new NoContentResult();
        }
    }
}
