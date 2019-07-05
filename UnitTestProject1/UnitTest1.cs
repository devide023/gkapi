using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GK.Utils;
using GK.Model;
using GK.Model.Parms;
using GK.Model.public_db;
using GK.DAO;
using GK.Service;
namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Tool tool = new Tool();
            string pwd = tool.Md5("123456");
            Console.WriteLine(pwd);
        }
    }
}
