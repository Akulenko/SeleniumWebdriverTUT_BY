using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;


namespace TestTUTby
{
    [TestClass]
    public class SeleniumWebdriverSearchTest
    {
        FirefoxDriver driver;                
        string searchText = "специалист по тестированию";

        [TestInitialize()]
        public void SyncDriver()
        {            
            driver = new FirefoxDriver();
            driver.Manage().Window.Maximize();
        }

        [TestMethod]
        public void Test_Count_Result_Of_Search_Work()
        {
                        int counter = 0;         
            driver.Navigate().GoToUrl("http://tut.by/");
            StringAssert.Contains("Белорусский портал TUT.BY", driver.Title);
            Console.WriteLine("TUT.BY is opened");

            var linkWork = driver.FindElementByCssSelector(@"a[title='Работа']");
            linkWork.Click();
            Assert.IsTrue(driver.FindElement(By.CssSelector(".navi-logo.navi-logo_jobs-tut-by")).Displayed);
            Console.WriteLine("'Work' tab is opened");


            var searchField = driver.FindElementByCssSelector(".bloko-input.HH-Navi-SearchSelector-Tab-TextInput.HH-Navi-StickyMenu-MainSearch");
            searchField.SendKeys(searchText);
            var getValueFromSearchField = driver.FindElement(By.CssSelector(".bloko-input.HH-Navi-SearchSelector-Tab-TextInput.HH-Navi-StickyMenu-MainSearch")).GetAttribute("value");            
            Assert.AreEqual(searchText, getValueFromSearchField);
            Console.WriteLine("The text '{0}' was written to the 'Search' field on the 'Work' page", searchText);

            var searchButton = driver.FindElementByCssSelector(".bloko-button.bloko-button_primary.bloko-button_stretched");
            searchButton.Click();

            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            IWebElement searchTable = driver.FindElementByCssSelector(".search-result");
            List <string> searchResultItems = new List<string>();
            ReadOnlyCollection <IWebElement> links = driver.FindElementsByCssSelector(".search-result-item__name");
            /*
            Regex regex = new Regex("[Сс]пециалист(\\s+(([А-Яа-я]|\\w)+)?){1,}\\s?по(\\s+(([А-Яа-я]|\\w)+)?){1,}\\s?тестированию");
            foreach (IWebElement link in links)
            {
                string text = link.Text;
                if (regex.IsMatch(text))
                    counter++;               
            }*/
            Regex regex = new Regex(searchText, RegexOptions.IgnoreCase);
            foreach (IWebElement link in links)
            {
                string text = link.Text;
                if (regex.IsMatch(text))
                    counter++;
            }
            if (counter == 0)
                Assert.Fail("no matches found");
            Console.WriteLine("{0} matches were found", counter);
        }

        // разрушение объекта драйвера после окончание теста.
        [TestCleanup]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
