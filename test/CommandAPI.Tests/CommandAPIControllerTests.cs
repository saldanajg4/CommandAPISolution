using System;
using System.Collections.Generic;
using CommandAPI.Controllers;
using CommandAPI.Data;
using CommandAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CommandAPI.Tests
{
    public class CommandAPIControllerTests : IDisposable
    {
        DbContextOptionsBuilder<CommandAPIContext> optionsBuilder;
        private CommandAPIController controller;
        private CommandAPIContext dbContext;
        public CommandAPIControllerTests()
        {
            optionsBuilder = new DbContextOptionsBuilder<CommandAPIContext>();
            optionsBuilder.UseInMemoryDatabase("UnitTestInMemDB");
            dbContext = new CommandAPIContext(optionsBuilder.Options);
            //create an instance of controller passing the dbContext created
            controller = new CommandAPIController(dbContext);
        }
        [Fact]
        public void getCommandsSize_ShoulbeZero_IfEmpty()
        {
            // arrange

            //act
            var actual = controller.GetAllCommands();
            //assert
            Assert.Empty(actual.Value);

        }
        [Fact]
        public void addOneItem_ReturnOne_WhenPostingValidData()
        {
            //arrange

            var cmd = new Command
            {
                HowTo = "Run xUnit Tests now using dbContext in controller constructor",
                Line = "dotnet test",
                Platform = ".net Framework"
            };
            dbContext.Commands.Add(cmd);
            dbContext.SaveChanges();
            //act
            var actual = controller.GetAllCommands();
            //assert
            Assert.Single(actual.Value);
        }
        [Fact]
        public void GetNCommands_ReturnNCommandsWhenDbHasNObjs(){
            //arrange
            var cmd = new Command
            {
                HowTo = "Run xUnit Tests now using dbContext in controller constructor",
                Line = "dotnet test",
                Platform = ".net Framework"
            };
            var cmd2 = new Command
            {
                HowTo = "Run app",
                Line = "dotnet run",
                Platform = ".net Framework"
            };
            dbContext.Commands.Add(cmd);
            dbContext.Commands.Add(cmd2);
            dbContext.SaveChanges();
            //act
            var actual = controller.GetAllCommands();
            int size = 0;
            foreach(var c in actual.Value){
                size++;
            }

            //Assert
            Assert.Equal(2, size);
        }

        [Fact]
        public void GetCommandsReturnsTheCorrectType(){
            //Arrange
            //Act
            var actual = controller.GetAllCommands();

            //Assert
            Assert.IsType<ActionResult<IEnumerable<Command>>>(actual);
        }
        /****
        * I need to test getCommandById() for Null, 404 in invalid cases
        * Correct return type, Correct resource returned for valid cases   
        *****/

        [Fact]
        public void getCommandReturnsNullWhenInvalidId(){
            
            var actual = controller.GetCommandById(0);
            //Assert
            Assert.Null(actual.Value);
        }
        [Fact]
        public void getCommandReturn404NotFound(){
            //arrange nothing to arrange so it can be 404
            //act
            var actual = controller.GetCommandById(0);
            //Assert
            Assert.IsType<NotFoundResult>(actual.Result);
            // Assert.IsType<OkResult>(actual.Result);
        }
        [Fact]
        public void getCommandReturnCorrectType(){
            //arrange
            var cmd = new Command
             {
                HowTo = "Run app",
                Line = "dotnet run",
                Platform = ".net Framework"
            };
            dbContext.Commands.Add(cmd);
            dbContext.SaveChanges();
            var cmdId = cmd.Id;
            //act
            var actual = controller.GetCommandById(cmdId);
            //Assert
            Assert.IsType<ActionResult<Command>>(actual);
        }
        [Fact]
        public void getCommandReturnCorrectSource(){
             //arrange
            var cmd = new Command
             {
                HowTo = "Run app",
                Line = "dotnet run",
                Platform = ".net Framework"
            };
            dbContext.Commands.Add(cmd);
            dbContext.SaveChanges();
            var cmdId = cmd.Id;
            //Act
            var actual = controller.GetCommandById(cmdId);
            //Assert
            Assert.Equal(cmdId, actual.Value.Id);
        }
        [Fact]
        public void postCommandAndCountIncrementWhenValidObject(){
             //arrange
            var cmd = new Command
             {
                HowTo = "Run app",
                Line = "dotnet run",
                Platform = ".net Framework"
            };
            var oldCount = dbContext.Commands.CountAsync();
            //act
            var actual = controller.PostCommand(cmd);
            //Assert
            Assert.Equal(oldCount.Result+1, dbContext.Commands.CountAsync().Result);
        }
        [Fact]
        public void postCommandReturns201IfValidObject(){
              //arrange
            var cmd = new Command
             {
                HowTo = "Run app",
                Line = "dotnet run",
                Platform = ".net Framework"
            };
            //act
            var actual = controller.PostCommand(cmd);
            //Assert
            Assert.IsType<CreatedAtActionResult>(actual.Result);
        }
        [Fact]
        public void PutCommandItem_AttributeUpdated_WhenValidObject(){
            //Arrange
            var cmd = new Command{
                HowTo = "Do Something",
                Platform = "Some platform",
                Line = "Some Command"
            };
            dbContext.Commands.Add(cmd);
            dbContext.SaveChanges();
            var cmdId = cmd.Id;
            cmd.HowTo = "Updating Something";
            //Act
            controller.PutCommandItem(cmdId, cmd);
            var result = dbContext.Commands.Find(cmdId);
            //Assert
            Assert.Equal(cmd.HowTo, result.HowTo);
        }
        [Fact]
        public void PutCommandItem_204ReturnCode_WhenValidObject(){
            //Arrange
            var cmd = new Command{
                HowTo = "Some item",
                Platform = "some platform",
                Line = "some line"
            };
            dbContext.Commands.Add(cmd);
            dbContext.SaveChanges();
            var cmdId = cmd.Id;
            cmd.HowTo = "updating some item";
            //Act
            var result = controller.PutCommandItem(cmdId, cmd);
            //Assert
            Assert.IsType<NoContentResult>(result.Result);
        }
        [Fact]
        public void PutCommandItem_BadRequest_WhenInvalidObject(){
            //arrange
            var cmd = new Command{
                HowTo = "some item",
                Platform = "some platform",
                Line = "some line"
            };
            dbContext.Commands.Add(cmd);
            dbContext.SaveChanges();
            var cmdId = cmd.Id;
            cmd.HowTo = "updating some item";
            //act
            var result = controller.PutCommandItem(cmdId+1, cmd);
            //assert
            Assert.IsType<BadRequestResult>(result.Result);
        }
        [Fact]
        public void PutCommandItem_AttributeUnchanged_WhenOtherObjectInvalid(){
            //arrange
            var cmd = new Command{
                HowTo = "some item",
                Platform = "some platform",
                Line = "some line"
            };
            dbContext.Commands.Add(cmd);
            dbContext.SaveChanges();
            var cmd2 = new Command{
                Id = cmd.Id,
                HowTo = "updating some item",
                Platform = "updating some platform",
                Line = "updating some Line"
            };
            //Act
            controller.PutCommandItem(cmd.Id+1,cmd2);
            var result = dbContext.Commands.Find(cmd.Id);
            //Assert
            Assert.Equal(cmd.HowTo, result.HowTo);
        }
        [Fact]
        public void PutCommandItem_BadRequest_WhenNullObject(){
            //arrange 
            var cmd = new Command{
                HowTo = "some item",
                Platform = "some platform",
                Line = "some line"
            };
            dbContext.Commands.Add(cmd);
            dbContext.SaveChanges();
            var cmdId = cmd.Id;
            //act
            var result = controller.PutCommandItem(cmdId,null);
            //assert
            Assert.IsType<BadRequestResult>(result.Result);
        }
        [Fact]
        public void DeleteCommandItem_DecrementItems_WhenValidObject(){
            //arrange
            var cmd = new Command{
                HowTo = "some item",
                Platform = "some platform",
                Line = "some line"
            };
            dbContext.Commands.Add(cmd);
            dbContext.SaveChanges();
            var cmdId = cmd.Id;
            var countBefore = dbContext.Commands.CountAsync();
            //act
            controller.DeleteCommandItem(cmdId);
            //assert
            Assert.Equal(countBefore.Result-1, dbContext.Commands.CountAsync().Result);
        }
        [Fact]
        public void DeleteCommandItem_200Return_WhenValidObject(){
             //arrange
            var cmd = new Command{
                HowTo = "some item",
                Platform = "some platform",
                Line = "some line"
            };
            dbContext.Commands.Add(cmd);
            dbContext.SaveChanges();
            var cmdId = cmd.Id;
            //act
            var result = controller.DeleteCommandItem(cmdId);
            //assert here result is the command deleted
            Assert.Null(result.Result);
        }
        [Fact]
        public void DeleteCommandItem_404Return_WhenInvalidObject(){
            //arrange
            //act
            var result = controller.DeleteCommandItem(-1);
            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
        [Fact]
        public void DeleteCommandItem_CountNotDecremented_WhenInvalidID(){
            //arrange
            var cmd = new Command{
                HowTo = "some item",
                Platform = "some platform",
                Line = "some line"
            };
            dbContext.Commands.Add(cmd);
            dbContext.SaveChanges();
            var cmdId = cmd.Id;
            var count = dbContext.Commands.CountAsync();
            //act
            controller.DeleteCommandItem(cmdId +1);
            //assert
            Assert.Equal(count.Result,dbContext.Commands.CountAsync().Result);
        }

        public void Dispose()
        {
            foreach(var cmd in dbContext.Commands){
                dbContext.Commands.Remove(cmd);
            }
            dbContext.SaveChanges();
            dbContext.Dispose();
            if (controller != null)
            {
                Console.WriteLine("disposing apiController object");
                controller = null;
            }
        }
    }
}