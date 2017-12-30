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
    public class loginformTests
    {

        [TestMethod()]
        public void LoginTest()
        { 
            String password = "123";
            String username = "123";

            var login = new loginform();

            Assert.IsTrue(login.Login(username, password));
        }
    }
}