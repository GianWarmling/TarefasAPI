using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TarefasAPI.Data;
using TarefasAPI.Models;
using TarefasAPI.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TarefasAPI.Controllers
{
    [Authorize]
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
        public async Task<ActionResult> GetAll()
        {
            var tarefas = await _context.Tarefas.ToListAsync();
            return Ok(ApiResponse<IEnumerable<TaskModel>>.Ok(tarefas, "Lista de tarefas carregada com sucesso"));
        }

        // GET api/<TasksController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null)
                return NotFound(ApiResponse<string>.Falha(tarefa, "Tarefa não encontrada!"));
            
            return Ok(ApiResponse<TaskModel>.Ok(tarefa, "Tarefa encontrada com sucesso!"));
        }

        // POST api/<TasksController>
        [HttpPost]
        public async Task<ActionResult> Create(TaskModel novaTarefa)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.Falha("Dados Inválidos!"));

            _context.Tarefas.Add(novaTarefa);
            await _context.SaveChangesAsync();
            return Ok(ApiResponse<TaskModel>.Ok(novaTarefa, "Tarefa criada com sucesso!"));
        }

        // PUT api/<TasksController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TaskModel tarefaAtualizada)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);

            if (tarefa == null)
                return NotFound(ApiResponse<string>.Falha("Tarefa não encontrada!"));
            
            tarefa.Titulo = tarefaAtualizada.Titulo;
            tarefa.Descricao = tarefaAtualizada.Descricao;
            tarefa.Concluida = tarefaAtualizada.Concluida;

            await _context.SaveChangesAsync();

            return Ok(ApiResponse<TaskModel>.Ok(tarefa, "Tarefa atualizada com sucesso!"));
        }

        // DELETE api/<TasksController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null)
                return NotFound(ApiResponse<string>.Falha("Tarefa não encontrada!"));

            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();  
            return Ok(ApiResponse<string>.Ok("Tarefa removida com sucesso!"));
        }
    }
}
