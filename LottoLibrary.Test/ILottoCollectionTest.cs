using LotteryLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LottoLibrary.Test
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///This is a test class for ILottoCollectionTest and is intended
    ///to contain all ILottoCollectionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ILottoCollectionTest
    {
        const string FullFileName = @"C:\Users\ben.crinion\Documents\Visual Studio 2010\Projects\LotteryConsoleApp\LotteryConsoleApp\All Lottery Draws.txt";
        const string MediumSetFilename = @"C:\Users\ben.crinion\Documents\Visual Studio 2010\Projects\LotteryConsoleApp\LotteryConsoleApp\Subset.txt";

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
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

        internal virtual ILottoCollection CreateILottoCollection()
        {
            ILottoCollection target = new LottoCollectionEnumerableImplementation();
            return target;
        }

        /// <summary>
        ///A test for LoadAllDraws
        ///</summary>
        [TestMethod()]
        public void LoadAllDrawsTest()
        {
            var target = CreateILottoCollection();
            target.LoadAllDraws(FullFileName);

            Assert.IsNotNull(target.AllDraws);
            Assert.IsTrue(target.AllDraws.Count() > 0);
        }

        /// <summary>
        ///A test for WinsJackpot
        ///</summary>
        [TestMethod()]
        public void JackpotWinningNumbers()
        {
            ILottoCollection target = CreateILottoCollection();
            
            target.LoadAllDraws(FullFileName);

            bool expected = true; 
            bool actual;

            actual = target.WinsJackpot(new List<short> {1, 22, 35, 39, 42, 48});

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void WinsAnythingWithJackpotWinningNumbers()
        {
            // need to create a smaller dataset for this test
            ILottoCollection target = CreateILottoCollection();

            target.LoadAllDraws(MediumSetFilename);

            bool expected = true;
            bool actual;

            actual = target.WinsAnything(new List<short> { 1, 2, 3, 39, 42, 48 });

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void WinsAnythingWithLoosingNumbers()
        {
            // need to create a smaller dataset for this test
            var target = CreateILottoCollection();

            target.LoadAllDraws(MediumSetFilename);

            bool expected = false;
            bool actual;

            actual = target.WinsAnything(new List<short> { 1, 2, 7, 8, 14, 15 });

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NoJackpotNumbers()
        {
            ILottoCollection target = CreateILottoCollection();
            
            target.LoadAllDraws(FullFileName);

            bool expected = false; 
            bool actual;

            actual = target.WinsJackpot(new List<short> { 1, 2, 3, 4, 5, 6 });

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TooManyNumbers()
        {
            ILottoCollection target = CreateILottoCollection();

            var input = new List<short> { 1, 2, 3, 4, 5, 6, 7 };
            target.WinsJackpot(input);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NotEnoughNumbers()
        {
            ILottoCollection target = CreateILottoCollection();

            var input = new List<short> { 1, 2, 3, 4, 5 };
            target.WinsJackpot(input);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NotUniqueNumbers()
        {
            ILottoCollection target = CreateILottoCollection();

            var input = new List<short> { 1, 2, 3, 4, 5, 5 };
            target.WinsJackpot(input);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NumbersTooHigh()
        {
            ILottoCollection target = CreateILottoCollection();

            var input = new List<short> { 51, 52, 53, 54, 55, 50 };
            target.WinsJackpot(input);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NumbersTooLow()
        {
            ILottoCollection target = CreateILottoCollection();

            var input = new List<short> { -1, -2, -3, -4, -5, 0 };
            target.WinsJackpot(input);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NotInitialised()
        {
            ILottoCollection target = CreateILottoCollection();

            var input = new List<short> { 1,2,3,4,5,6 };
            target.WinsJackpot(input);
        }
    }
}
