using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZeusAutomationSuite.Helper;
using ZeusAutomationSuite.DataProviders;
using OpenQA.Selenium;
using System.Collections.Generic;
using OpenQA.Selenium.Chrome;
using ZeusAutomationSuite.Extensions;

namespace SampleUnitTest
{
    [TestClass]
    public class SampleUnitTest
    {

        ZeusAutomationSuite.Helper.TestCaseHelper TestCase = null;
        RunAutomation.Helpers.PopulateConfigurations Config = new RunAutomation.Helpers.PopulateConfigurations();
        CommonUtilities Utilities = new CommonUtilities();
        SQLDataProvider SQLProvider = new SQLDataProvider();
        TestCaseHelper testCase = null;
        TestExecutions execution = null;
        string downloadDirectory = "";
        public SampleUnitTest()
        {
        }

        [TestInitialize]
        public void SetBrowserAsPerCOnfiguration()
        {
            var chromeOptions = new ChromeOptions();
            downloadDirectory = @"C:\Temp";
            chromeOptions.AddUserProfilePreference("download.default_directory", downloadDirectory);
            chromeOptions.AddUserProfilePreference("download.prompt_for_download", false);
            chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");
            if (Convert.ToString(TestContext.Properties["Module"]) == "TestData")
            {
                testCase = new TestCaseHelper();
                execution = new TestExecutions();
            }
            else
            {
                string ModuleName = Convert.ToString(TestContext.Properties["Module"]);
                testCase = new TestCaseHelper(ModuleName);
                execution = new TestExecutions(ModuleName, chromeOptions);
            }
            /*This code block will get executed while debugging/runing test locally or in case
                 Connection to TFS fails due any unknown reason*/
            //ConfigHelper.Environment = "INT";
            ConfigHelper.TestingTool = "selenium"; ConfigHelper.TestRunID = -1;
        }

        [TestCleanup]
        public void Cleanup()
        {
            //Playback.PlaybackSettings.WaitForReadyLevel = WaitForReadyLevel.Disabled;
        }


       [TestMethod]
        public void VerifyUWPCalculatorAddFunctionality()
        {
            string LoginPage = "";
            ConfigHelper.Browser = "uwp";
            ConfigHelper.IsWebTest = "true";
            Assert.IsTrue(TestCase.ExecuteTestCase("VerifyUwpCalculatorAddFeature.xlsx", ConfigHelper.TestingTool, LoginPage, testContextInstance));

        }

        [TestMethod]
        public void WebTestforDesktopHeadless()
        {
            string LoginPage = "";
            ConfigHelper.Browser = "headless";
            ConfigHelper.IsWebTest = "true";
            Assert.IsTrue(TestCase.ExecuteTestCase("WebTestforMobileAndPC.xlsx", ConfigHelper.TestingTool, LoginPage, testContextInstance));
        }

        [TestMethod]
        public void WebTestforDesktop()
        {
            string LoginPage = "";
            bool result = true;
            ConfigHelper.Browser = "chrome";
            ConfigHelper.IsWebTest = "true";
            IWebDriver driver = null;
            List<TestStep> testSteps = execution.RetrieveTestSteps("WebTestforMobileAndPC.csv", ConfigHelper.TestingTool, LoginPage, testContextInstance);
            foreach (TestStep step in testSteps)
            {
                driver = step.Driver;
                if (execution.Keyword.Contains(step.Keyword))
                {
                    bool stepresult = step.ExecuteTestStep();
                    result = result & stepresult;
                }
                else
                {
                    step.Driver.FindElement(By.Name("q")).SendKeys(step.Value);
                    testContextInstance.WriteLine("FOUND CUSTOM Element");
                }
            }
            driver.Dispose();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void WebTestforDesktopPass()
        {
            string LoginPage = "";
            bool result = true;
            ConfigHelper.Browser = "chrome";
            ConfigHelper.IsWebTest = "true";
            IWebDriver driver = null;
            List<TestStep> testSteps = execution.RetrieveTestSteps("WebTestforMobileAndPCPass.csv", ConfigHelper.TestingTool, LoginPage, testContextInstance);
            foreach (TestStep step in testSteps)
            {
                driver = step.Driver;
                if (execution.Keyword.Contains(step.Keyword))
                {
                    bool stepresult = step.ExecuteTestStep();
                    result = result & stepresult;
                }
                else
                {
                    step.Driver.FindElement(By.Name("q")).SendKeys(step.Value);
                    testContextInstance.WriteLine("FOUND CUSTOM Element");
                }
            }
            driver.Dispose();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void WebTestforDesktopIE()
        {
            string LoginPage = "";
            bool result = true;
            ConfigHelper.Browser = "ie";
            ConfigHelper.IsWebTest = "true";
            IWebDriver driver = null;
            List<TestStep> testSteps = execution.RetrieveTestSteps("WebTestforMobileAndPC.csv", ConfigHelper.TestingTool, LoginPage, testContextInstance);
            foreach (TestStep step in testSteps)
            {
                driver = step.Driver;
                if (execution.Keyword.Contains(step.Keyword))
                {
                    bool stepresult = step.ExecuteTestStep();
                    result = result & stepresult;
                }
                else
                {
                    step.Driver.FindElement(By.Name("q")).SendKeys(step.Value);
                    testContextInstance.WriteLine("FOUND CUSTOM Element");
                }
            }
            driver.Dispose();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void WebTestforDesktopHeadlessCSV()
        {
            string LoginPage = "";
            bool result = true;
            ConfigHelper.Browser = "headless";
            ConfigHelper.IsWebTest = "true";
            IWebDriver driver = null;
            List<TestStep> testSteps = execution.RetrieveTestSteps("WebTestforMobileAndPC.csv", ConfigHelper.TestingTool, LoginPage, testContextInstance);
            foreach (TestStep step in testSteps)
            {
                driver = step.Driver;
                if (execution.Keyword.Contains(step.Keyword))
                {
                    bool stepresult = step.ExecuteTestStep();
                    result = result & stepresult;
                }
                else
                {
                    step.Driver.FindElement(By.Name("q")).SendKeys(step.Value);
                    testContextInstance.WriteLine("FOUND CUSTOM Element");
                }
            }
            driver.Dispose();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void WebTestforMobile()
        {
            string LoginPage = "";
            ConfigHelper.Browser = "android";
            ConfigHelper.IsWebTest = "true";
            Assert.IsTrue(TestCase.ExecuteTestCase("WebTestforMobileAndPC.xlsx", ConfigHelper.TestingTool, LoginPage, testContextInstance));
        }

        [TestMethod]
        public void AndroidNativeAppTestForCalculator()
        {
            string LoginPage = "";
            ConfigHelper.Browser = "android";
            ConfigHelper.IsWebTest = "false";
            Assert.IsTrue(TestCase.ExecuteTestCase("VerifyAndroidNativeCalApp.xlsx", ConfigHelper.TestingTool, LoginPage, testContextInstance));
        }

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
        private TestContext testContextInstance;

    }
}
