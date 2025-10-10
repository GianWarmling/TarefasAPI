using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TarefasAPI.Data;
using TarefasAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TarefasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/<TasksController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskModel>>> GetAll()
        {
            return await _context.Tarefas.ToListAsync();
        }

        // GET api/<TasksController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<TaskModel>>> GetById(int id)
        {
            var tarefa = await _context.Tarefas.FirstOrDefaultAsync(t => t.Id == id);
            if (tarefa == null)
                return BadRequest("Id não existe!");
            
            return Ok(tarefa);
        }

        // POST api/<TasksController>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<TaskModel>>> Create(TaskModel novaTarefa)
        {
            _context.Tarefas.Add(novaTarefa);
            await _context.SaveChangesAsync();
            return Ok(new { mensagem = "Tarefa criada com sucesso!", novaTarefa });
        }

        // PUT api/<TasksController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TaskModel tarefaAtualizada)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);

            if (tarefa == null)
            {
                return BadRequest("Tarefa não encontrada!");
            }

            tarefa.Titulo = tarefaAtualizada.Titulo;
            tarefa.Descricao = tarefaAtualizada.Descricao;
            tarefa.Concluida = tarefaAtualizada.Concluida;

            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Tarefa atualizada com sucesso!", tarefa });
        }

        // DELETE api/<TasksController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tarefa = await _context.Tarefas.FirstOrDefaultAsync(t => t.Id == id);
            if (tarefa == null)
            {
                return BadRequest("Tarefa não existe!");
            }

            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();  
            return NoContent();
        }
    }
}
