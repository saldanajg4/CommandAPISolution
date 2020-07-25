using System.Collections.Generic;
using CommandAPI.Models;

namespace CommandAPI.Repos
{
    public class MockCommandRepo : ICommandAPIRepo
    {
        public List<Command> commands;

        public MockCommandRepo()
        {
            commands = new List<Command>();
        }
        public void setCommandList()
        {
            List<Command> commands = new List<Command>{
                new Command{Id=0, HowTo="Boil an egg", Line="Boil water", Platform="Kettle and Pan"},
                new Command{Id=1, HowTo="Rezar el Rosario", Line="Juntar gente", Platform="La recompenza lo vale todo"},
                new Command{Id=2, HowTo="Viajar a Mex", Line="Checa la Van", Platform="Documents"}
            };
        }

        public IEnumerable<Command> GetAppCommands()
        {
            return commands;
        }

        public Command GetCommandById(int id)
        {
            return commands.Find(cmd => cmd.Id == id);
        }

        public int PostCommand(Command command)
        {
            commands.Add(command);


            return 1;

        }
    }
}