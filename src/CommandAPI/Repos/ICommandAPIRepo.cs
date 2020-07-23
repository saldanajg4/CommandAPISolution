using System.Collections.Generic;
using CommandAPI.Models;

namespace CommandAPI.Repos{
    public interface ICommandAPIRepo{
 IEnumerable<Command> GetAppCommands();
        Command GetCommandById(int id);
        int PostCommand(Command command);
    }

}