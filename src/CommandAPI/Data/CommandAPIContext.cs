using CommandAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandAPI.Data{
    public class CommandAPIContext : DbContext{


        public CommandAPIContext(DbContextOptions<CommandAPIContext> options) : 
            base(options){

            }
             public DbSet<Command> Commands { get; set; }
    }
}