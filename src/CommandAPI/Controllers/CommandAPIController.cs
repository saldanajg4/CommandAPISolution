using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandAPI.Data;
using CommandAPI.Models;
using CommandAPI.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommandAPI.Controllers
{
    //api/commands for Route definition below  
    [Route("api/[controller]")]
    [ApiController]
    public class CommandAPIController : ControllerBase
    {
        private readonly ICommandAPIRepo _repo;
        //dependency injection from the dbContext class into 
        //our command api controller
        private readonly CommandAPIContext _context;

        //dependency injected value
        // public CommandAPIController(ICommandAPIRepo repo)
        // {
        //     _repo = repo;
        // }
        public CommandAPIController(CommandAPIContext ctx)
        {
            _context = ctx;
        }
        //api/commands
        [HttpGet]
        public async Task<IEnumerable<Command>> GetAllCommands()
        {   
                return await _context.Commands.ToListAsync();
        }

        [Authorize]
        //uri is GET api/commands/{id} api/commands/1
        [HttpGet("{id}")]
        public ActionResult<Command> GetCommandById(int id)
        {
            var command = _context.Commands.Find(id);
            if(command == null){
                return NotFound();
            }
            return command;
        }
        [HttpPost]
        //  public ActionResult<Command> PostCommand(Command command)
        public ActionResult<Command> PostCommand(Command command)
        {
            _context.Commands.Add(command);
            try{
                 _context.SaveChangesAsync();
            }
            catch{
                return BadRequest();
            }
            return CreatedAtAction("GetCommandById", new Command{Id=command.Id}, command);
        }

        //PUT:      api/commands/{Id}
        [HttpPut("{id}")]
        public ActionResult<Command> PutCommandItem(int id, Command command){
            if(command == null || id != command.Id){
                return BadRequest();
            }
            _context.Entry(command).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public ActionResult<Command> DeleteCommandItem(int id){
            var commandToDelete = _context.Commands.Find(id);
            if(commandToDelete == null){
                return NotFound();
            }
            _context.Commands.Remove(commandToDelete);
            _context.SaveChanges();

            return commandToDelete;
        }
    }
}