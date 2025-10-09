using Microsoft.AspNetCore.Mvc;
using TarefasAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TarefasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private static List<TaskModel> tarefas = new List<TaskModel>();
        private static int contador = 1;

        // GET: api/<TasksController>
        [HttpGet]
        public ActionResult<IEnumerable<TaskModel>> GetAll()
        {
            return Ok(tarefas);
        }

        // GET api/<TasksController>/5
        [HttpGet("{id}")]
        public ActionResult<TaskModel> GetById(int id)
        {
            var tarefa = tarefas.FirstOrDefault(x => x.Id == id);
            if (tarefa == null) return NotFound();
            return Ok(tarefas);
        }

        // POST api/<TasksController>
        [HttpPost]
        public ActionResult<TaskModel> Create(TaskModel novaTarefa)
        {
            novaTarefa.Id = contador++;
            tarefas.Add(novaTarefa);
            return CreatedAtAction(nameof(GetById), new {id = novaTarefa.Id}, novaTarefa);
        }

        // PUT api/<TasksController>/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, TaskModel tarefaAtualizada)
        {
            var tarefa = tarefas.FirstOrDefault(t => t.Id == id);
            if (tarefa == null) return NotFound();

            tarefa.Titulo = tarefaAtualizada.Titulo;
            tarefa.Descricao = tarefaAtualizada.Descricao;
            tarefa.Concluida = tarefaAtualizada.Concluida;

            return NoContent();
        }

        // DELETE api/<TasksController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var tarefa = tarefas.FirstOrDefault(t => t.Id == id);
            if (tarefa == null) return NotFound();

            tarefas.Remove(tarefa);
            return NoContent();
        }
    }
}
