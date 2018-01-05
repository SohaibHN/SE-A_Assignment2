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
    public class MainAppTests
    {
        [TestMethod()]
        public void GridViewDataTest()
        {

            var mainapp = new MainApp();
            int count = mainapp.GridViewData(); // gets amount of rows in ticket table

            Assert.IsTrue(count > 0, "Row count is not greater than 0");


        }

        [TestMethod()]
        public void UpdatePasswordTest()
        {

            var mainapp = new MainApp();
            String password = "1";
            String password2 = "1";
            String username = "dev";

            Assert.IsTrue(mainapp.UpdatePassword(username, password, password2));
        }
    }
}