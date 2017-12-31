using Microsoft.VisualStudio.TestTools.UnitTesting;
using SE_A_Assignment2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_A_Assignment2.Tests
{
    [TestClass()]
    public class AddUserTests
    {
        [TestMethod()]
        public void registerTest()
        {
            String username = "122"; 
            String password = "123";
            String category = "Admin";

            var register = new AddUser(); //creates new add user context

            Assert.IsTrue(register.register(username, password, category), "user could not be added");
            // runs register method located in AddUser.cs with the strings above.
            
        }
    }
}