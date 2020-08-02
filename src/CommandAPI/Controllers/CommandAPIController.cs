using System.Collections.Generic;
using System.Linq;
using CommandAPI.Data;
using CommandAPI.Models;
using CommandAPI.Repos;
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
        public ActionResult<IEnumerable<Command>> GetAllCommands()
        {
            // var commandItems = _repo.GetAppCommands();
            var commandItems = _context.Commands;
            // return Ok(commandItems);
            return commandItems;

        }

        //uri is GET api/commands/{id} api/commands/1
        [HttpGet("{id}")]
        public ActionResult<Command> GetCommandById(int id)
        {
            // var commandItem = _repo.GetCommandById(id);
            // return Ok(commandItem);

            // return _context.Commands
            //     .Where(cmd => cmd.Id == id)
            //     .FirstOrDefault();
            var command = _context.Commands.Find(id);
            if(command == null){
                return NotFound();
            }
            return command;
        }
        [HttpPost]
        // public ActionResult<Command> PostCommand(Command command)
         public ActionResult<Command> PostCommand(Command command)
        {
            // var cmd = _repo.PostCommand(command);
            // // return Ok(CreatedAtAction(nameof(GetCommandById), new {id = command.Id}, command));
            // return Ok(cmd + " row inserted.");
            _context.Commands.Add(command);
            try{
                _context.SaveChanges();
            }
            catch{
                return BadRequest();
            }
            return CreatedAtAction("GetCommandById", new Command{Id=command.Id}, command);
        }

        //PUT:      api/commands/{Id}
        [HttpPut("{id}")]
        public ActionResult<Command> PutCommandItem(int id, Command command){
            if(id != command.Id){
                return BadRequest();
            }
            _context.Entry(command).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }
    }
}