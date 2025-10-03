using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMS.App.Dtos;
using TMS.App.Interfaces;
using TMS.App.Services;
namespace TMS.Apis.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController: ControllerBase
    {
        private readonly ITaskService _taskService; 

        public TasksController ( ITaskService userService)
        {
            _taskService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _taskService.GetAllTasks();
            return Ok(tasks);
        }

        [HttpGet("id")]
        public async Task <IActionResult> GetById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound(); // 404
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
        {
            var newTask = await _taskService.CreateTaskAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = newTask.Id }, newTask);
            // 201 Created + Location Header + Task
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _taskService.DeleteTaskAsync(id);
            if (!deleted)
                return NotFound(); // 404
            return NoContent(); // 204 No Content
        }
    }
}