
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using App_a_matic.Helper;
using App_a_matic.Orm;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using verychefy.Models.Products;
using System.Collections.Generic; 
namespace verychefy.Tests
{
    
    
    /// <summary>
    ///This is a test class for DataExtensionsTest and is intended
    ///to contain all DataExtensionsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DataExtensionsTest {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext {
            get {
                return testContextInstance;
            }
            set {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion



        /// <summary>
        ///A test for Get
        ///</summary>
        [TestMethod]
        public void GetTest(){
                //Ingredients ingredient = (new Ingredients()).Get<Ingredients>(1);
                Ingredients obj = new Ingredients();
            //KeyValuePair<string, object> fltr =  new KeyValuePair<string, object>("CommonName", "Carrot");
            //<dynamic> fltr = ;
                var list = obj.GetCollection<Ingredients>(); 
                Assert.AreEqual("Carrot", list[0].CommonName);
        }

        [TestMethod]
        public void QueryTest1() {
            Ingredients obj = new Ingredients();
            var fltr = new DataExtensions.Parameter() { Name = "CommonName", Value = "Carrot" };
            var list = obj.Query<Ingredients>(fltr);
            Assert.AreEqual("Carrot", list[0].CommonName);
        }

        [TestMethod]
        public void QueryTest2() {
            Ingredients obj = new Ingredients();
            var fltr = new DataExtensions.Parameter("CommonName", "Carrot");
            var list = obj.Query<Ingredients>(fltr);
            Assert.AreEqual("Carrot", list[0].CommonName);
        }


        /*
        [TestMethod()]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("c:\\users\\dirlamg\\documents\\visual studio 2010\\Projects\\verychefy\\verychefy", "/")]
        [UrlToTest("http://localhost:62221/")]
        public void GetTest() {
            Assert.Inconclusive("No appropriate type parameter is found to satisfies the type constraint(s) of T. " +
                    "Please call GetTestHelper<T>() with appropriate type parameters.");
        }
*/
    }
}
