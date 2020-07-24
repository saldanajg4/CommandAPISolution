using System.Collections.Generic;
using System.Linq;
using CommandAPI.Data;
using CommandAPI.Models;
using CommandAPI.Repos;

namespace CommandAPI.Repos
{
    public class DbCommandAPIRepo : ICommandAPIRepo
    {
        private readonly CommandAPIContext _context;
        public DbCommandAPIRepo(CommandAPIContext ctx)
        {
            _context = ctx;
        }
        public IEnumerable<Command> GetAppCommands()
        {
            return _context.Commands;
        }

        public Command GetCommandById(int id)
        {
            return _context.Commands
                .Where(cmd => cmd.Id == id)
                .FirstOrDefault();
                
        }

        public int PostCommand(Command command)
        {
            _context.Commands.Add(command);
            return _context.SaveChanges();
        }
    }
}