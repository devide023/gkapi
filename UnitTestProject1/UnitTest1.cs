using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GK.Utils;
using GK.Model;
using GK.Model.Parms;
using GK.Model.public_db;
using GK.DAO;
using GK.Service;
using GoldKeyWebApi.Controllers.UserManager;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            UserMgrController c = new UserMgrController();
            var j = c.GetUserInfo("0DDJHIaKTsphHL1sWi1mriXJaj74esytgrNbFEkh5CivJQnFg9rfAn+oc2eSDKgkFA6VQnveYKz77bUE7QbOIERMqLPtoPLwPdNoek9sqPq0aOD80bKDzxc4+y8GN4hhK8MWbG08Ygeo3ijKO/GYo+u/qrxW/ZRVAfEhwiRDJ5g=");
        }
    }
}
