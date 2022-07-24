using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;


namespace ShortUrls.WebDriverTests
{
    public class SeleniumTests_ShortUrls
    {
        //private const string base_url = "https://shorturl.ivaylogodu.repl.co/";
        private const string base_url = "https://shorturl.nakov.repl.co/";

        private WebDriver driver;

        [OneTimeSetUp]
        public void OpenBrowser()
        {
            this.driver = new ChromeDriver();
            driver.Navigate().GoToUrl(base_url);
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [OneTimeTearDown]
        public void CloseBrowser()
        {
            this.driver.Quit();
        }

        [Test]
        public void Open_ShortUrls_Page()
        {
            
            var shortUrls_button = driver.FindElement(By.CssSelector("header > a:nth-of-type(2)"));           
            shortUrls_button.Click();
           
            var CellExpectedResult = "https://nakov.com";
            var tableCells = driver.FindElements(By.CssSelector("table tr > td"));

            Assert.That(tableCells[0].Text, Is.EqualTo(CellExpectedResult));
            

        }
        [Test]
        public void Add_ShortUrl_VailidData()
        {

            var addUrls_button = driver.FindElement(By.CssSelector("header > a:nth-of-type(3)"));
            addUrls_button.Click();

            var new_url = "https://example/" + DateTime.Now.Ticks + ".com";
            var url_field = driver.FindElement(By.Id("url"));
            url_field.SendKeys(new_url);

            var new_code = "123" + DateTime.Now.Ticks;
            var code_field = driver.FindElement(By.CssSelector("#code"));
            code_field.Clear();
            code_field.SendKeys(new_code);


            var create_button = driver.FindElement(By.CssSelector("button"));
            create_button.Click();

            var table = driver.FindElements(By.CssSelector("table tr > td"));

            foreach (var item in table)
            {
                if (item.Text.Contains(new_url))
                {
                    Assert.That(item.Text, Does.Contain(new_url));
                    break;
                }
            }
        }

        [Test]
        public void Add_Url_InvailidData()
        {
            var addUrls_button = driver.FindElement(By.CssSelector("header > a:nth-of-type(3)"));
            addUrls_button.Click();

            var url_field = driver.FindElement(By.Id("url"));
            url_field.SendKeys("123" + DateTime.Now.Ticks);

            var create_button = driver.FindElement(By.CssSelector("button"));
            create_button.Click();

            var massege_field = driver.FindElement(By.CssSelector(".err")).Text;

            Assert.That(massege_field, Is.EqualTo("Invalid URL!"));

        }
        [Test]
        public void Visit_NonExisting_ShortUrl()
        {
            driver.Navigate().GoToUrl(base_url + "go/" + DateTime.Now.Ticks);



            var massege_field = driver.FindElement(By.CssSelector(".err")).Text;

            Assert.That(massege_field, Is.EqualTo("Cannot navigate to given short URL"));
        }

        [Test]
        public void Visit_Existing_ShortUrl()
        {
            var shortUrls_button = driver.FindElement(By.CssSelector("header > a:nth-of-type(2)"));
            shortUrls_button.Click();

            var first_visit_count = driver.FindElement(By.CssSelector("tr:nth-of-type(1) > td:nth-of-type(4)")).Text;
            var first_visit_countInt = int.Parse(first_visit_count);

            var first_Url = driver.FindElement(By.CssSelector("tr:nth-of-type(1) > td:nth-of-type(2) > .shorturl"));
            first_Url.Click();

            var newUrlWindow = driver.SwitchTo().Window(driver.WindowHandles[1]);
            var expektedNewWindowTitle = "Svetlin Nakov - Svetlin Nakov – Official Web Site and Blog";

            Assert.That(newUrlWindow.Title, Is.EqualTo(expektedNewWindowTitle));

            driver.SwitchTo().Window(driver.WindowHandles[0]);

            var last_visit_count = driver.FindElement(By.CssSelector("tr:nth-of-type(1) > td:nth-of-type(4)")).Text;
            var last_visit_countInt = int.Parse(last_visit_count);

            Assert.That(first_visit_countInt, Is.LessThan(last_visit_countInt));
            Assert.That(1, Is.EqualTo(last_visit_countInt - first_visit_countInt));

        }
        
    }
}
