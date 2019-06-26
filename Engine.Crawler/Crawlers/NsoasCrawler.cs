using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace Engine.Crawler.Crawlers
{
    /// <summary>
    /// 硬核网页爬虫（Nsoas）
    /// </summary>
    public class NsoasCrawler : ISimulationUICrawler
    {
        string url = "http://dds.nsoas.org.cn/system/index/loginback.do";
        string uid = "Minerva";
        string pwd = "whdx123";

        IWebDriver driver; 

        public NsoasCrawler()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            NsoasLogin();
            OrderOneTime();
        }

        public void NsoasLogin()
        {
            driver.Url = url;
            //uid pwd
            driver.FindElement(By.Id("login_uid")).SendKeys(uid);
            driver.FindElement(By.Id("login_pwd")).SendKeys(pwd);
            //button click
            driver.FindElement(By.Id("loginbtn")).Click();
        }

        public void OrderOneTime()
        {
            //1. topmenu 
            IWebElement button = driver.FindElement(By.CssSelector("li[id=topmenu_order]>a"));
            Actions builder = new Actions(driver);
            builder.MoveToElement(button).Click().Build().Perform();
            //var element = driver.FindElement(By.CssSelector("li[id=topmenu_order]>ul>li>a"));
            //2.order
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
            var element = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("li[id=topmenu_order]>ul>li>a")));
            element.Click();
        }

        public void StartCrawler()
        {
            //
            //http://dds.nsoas.org.cn/business/order_manage/order_onetime/orderonetime.jsp
            //
        }
    }
}
