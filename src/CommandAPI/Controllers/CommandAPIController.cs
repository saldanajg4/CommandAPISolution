using System.Collections.Generic;
using CommandAPI.Models;
using CommandAPI.Repos;
using Microsoft.AspNetCore.Mvc;

namespace CommandAPI.Controllers{
     //api/commands for Route definition below  
    [Route("api/[controller]")]
    [ApiController]
    public class CommandAPIController : ControllerBase{
private readonly ICommandAPIRepo _repo;
        //dependency injected value
        public CommandAPIController(ICommandAPIRepo repo){
            _repo = repo;
        }
        //api/commands
        [HttpGet]
        public ActionResult <IEnumerable<Command>> GetAllCommands(){
            var commandItems = _repo.GetAppCommands();
            return Ok(commandItems);
            
        }

        //uri is GET api/commands/{id} api/commands/1
        [HttpGet("{id}")]
        public ActionResult <Command> GetCommandById(int id){
            var commandItem = _repo.GetCommandById(id);
            return Ok(commandItem);
        }
        [HttpPost]
        public ActionResult<Command> PostCommand(Command command){
            var cmd =  _repo.PostCommand(command);
            // return Ok(CreatedAtAction(nameof(GetCommandById), new {id = command.Id}, command));
            return Ok(cmd + " row inserted.");
        }
    }
}