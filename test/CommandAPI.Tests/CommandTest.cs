using System;
using CommandAPI.Models;
using Xunit;

namespace CommandAPI.Tests
{
    public class CommandTest : IDisposable
    {
        private Command commandTest;
        public CommandTest()
        {
            commandTest = new Command
            {
                HowTo = "Create first test",
                Platform = "xUnit",
                Line = "[Fact]"
            };
        }
        [Fact]
        public void CanChangeHowTo(){
            //Arrange

            //Act
            commandTest.HowTo = "Changing how to";
            //Assert
            Assert.Equal("Changing how to", commandTest.HowTo);
        }
        [Fact]
        public void CanChangeLine(){
            //arrange
 
            //act
            commandTest.Line = "[Fact]";
            //assert
            Assert.Equal("[Fact]",commandTest.Line);
        }

        public void Dispose()
        {
            if(commandTest != null){
                Console.WriteLine("disposing commandTest object");
                commandTest = null;
            }
                
        }
    }
}