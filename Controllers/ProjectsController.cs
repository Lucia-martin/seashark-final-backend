
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MyApp.Models;
using MyApp.Hubs;

namespace MyApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IHubContext<ProjectsHub> _hub;

        public ProjectsController(DatabaseContext context, IHubContext<ProjectsHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
          if (_context.Projects == null)
          {
              return NotFound();
          }
            return await _context.Projects.ToListAsync();
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
          if (_context.Projects == null)
          {
              return NotFound();
          }
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, Project project)
        {
            if (id != project.Id)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await _hub.Clients.All.SendAsync("EditProject", project);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


    // POST: api/Projects
            // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
            [HttpPost]
            public async Task<ActionResult<Project>> PostProject(Project project)
            {
              if (_context.Projects == null)
              {
                  return Problem("Entity set 'DatabaseContext.Projects'  is null.");
              }
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();
                await _hub.Clients.All.SendAsync("CreateProject", project);
                return CreatedAtAction("GetProject", new { id = project.Id }, project);
            }



        //POST: api/projects/5/todos
        [HttpPost("{projectId}/Todos")]
        public async Task<Todo> PostProjectTodo(int projectId, Todo Todo)
        {
            Todo.ProjectId = projectId;

            _context.Todos.Add(Todo);
            await _context.SaveChangesAsync();
            await _hub.Clients.Group(projectId.ToString()).SendAsync("ReceiveTodo", Todo);

            return Todo;
        }


        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            if (_context.Projects == null)
            {
                return NotFound();
            }
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            await _hub.Clients.All.SendAsync("DeleteProject", id);

            return NoContent();
        }

        private bool ProjectExists(int id)
        {
            return (_context.Projects?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
