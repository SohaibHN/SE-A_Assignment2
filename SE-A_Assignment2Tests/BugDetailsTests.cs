using Microsoft.VisualStudio.TestTools.UnitTesting;
using SE_A_Assignment2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_A_Assignment2.Tests
{
    [TestClass()]
    public class BugDetailsTests
    {
        [TestMethod()]
        public void PushCommitsTest()
        {
            string username = ""; // commented out for commiting folder up
            // auth code in username
            // need to be removed whenever committing to github
            ////
            // when running commit test, change values in actual file to not require bug ticket info!!!
            ////

            string password = string.Empty;
            String gitRepoUrl = "https://github.com/SohaibHN/Test.git";
            String localFolder = "C:\\Users\\Admin\\source\\repos\\Test";
            string path = Directory.GetCurrentDirectory();
            localFolder = Path.GetFullPath(Path.Combine(path, @"..\..\..\..\Test"));
            var BugDetails = new BugDetails();

            var folder = new DirectoryInfo(localFolder);
            BugDetails.GitDeploy2(username, password, gitRepoUrl, localFolder);
            BugDetails.CommitAllChanges();


            Assert.IsTrue(BugDetails.PushCommits("origin", "master"), "Unable to Push Commit");
        }
    }
}