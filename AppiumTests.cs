using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using System;
using System.Threading;

namespace ContactBook.AndroidTests
{
    public class AndroidTests_ShortUrls
    {
        private const string AppiumUrl = "http://[::1]:4723/wd/hub";
        private const string Url = "https://shorturl.nakov.repl.co/api";
        //private const string Url = "https://shorturl.ivaylogodu.repl.co/api";
        private const string appLocation = @"C:\com.android.example.github.apk";

        private AndroidDriver<AndroidElement> driver;
        private AppiumOptions options;


        [SetUp]
        public void StartApp()
        {
            options = new AppiumOptions() { PlatformName = "Android" };
            options.AddAdditionalCapability("app", appLocation);

            driver = new AndroidDriver<AndroidElement>(new Uri(AppiumUrl), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

        }


        [TearDown]
        public void CloseApp()
        {
            driver.Quit();
        }

        [Test]
        public void Test_Assert_Developer_Profile()
        {
            var surch_field = driver.FindElement(By.Id("com.android.example.github:id/input"));
            surch_field.Click();
            surch_field.SendKeys("Selenium");

            driver.PressKeyCode(AndroidKeyCode.Keycode_ENTER);

            Thread.Sleep(3000);

            var selenium_element_title = driver.FindElement(By.XPath("/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.view.ViewGroup/android.widget.FrameLayout[2]/android.widget.FrameLayout/android.widget.FrameLayout/android.view.ViewGroup/androidx.recyclerview.widget.RecyclerView/android.widget.FrameLayout[1]/android.view.ViewGroup/android.widget.TextView[2]"));
            Assert.IsNotNull(selenium_element_title);
            Assert.That(selenium_element_title.Text, Is.EqualTo("SeleniumHQ/selenium"));

            var selenium_element_button = driver.FindElement(By.XPath("/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.view.ViewGroup/android.widget.FrameLayout[2]/android.widget.FrameLayout/android.widget.FrameLayout/android.view.ViewGroup/androidx.recyclerview.widget.RecyclerView/android.widget.FrameLayout[1]/android.view.ViewGroup"));
            selenium_element_button.Click();

            var enter_developer_profile = driver.FindElement(By.XPath("/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.view.ViewGroup/android.widget.FrameLayout[2]/android.widget.FrameLayout/android.widget.FrameLayout/android.view.ViewGroup/androidx.recyclerview.widget.RecyclerView/android.widget.FrameLayout[2]/android.view.ViewGroup/android.widget.TextView"));
            Assert.That(enter_developer_profile.Text, Is.EqualTo("barancev"));
            enter_developer_profile.Click();

            var developer_profile = driver.FindElement(By.Id("com.android.example.github:id/name"));
            Assert.That(developer_profile.Text, Is.EqualTo("Alexei Barantsev"));

        }
    }
}