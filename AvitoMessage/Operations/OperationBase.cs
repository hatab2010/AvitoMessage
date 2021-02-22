using AvitoMessage.Pages;
using OpenQA.Selenium;
using System;
using System.Linq;

namespace AvitoMessage.Operations
{
    public abstract class OperationBase : IOperation
    {
        protected IWebDriver driver;
        public Page OperationPage
        {
            protected set; get;
        }

        public OperationBase(IWebDriver driver)
        {
            this.driver = driver;
        }

        public bool Execute()
        {
            try
            {
                GoToOperationPage();
                Action();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            return true;
        }

        protected virtual void Action()
        {

        }

        public void GoToOperationPage()
        {
            var currentPage = driver.Url;

            if (!currentPage.Equals(OperationPage.link, StringComparison.OrdinalIgnoreCase))
            {
                driver.Navigate().GoToUrl(OperationPage.link);
            }
        }
    }
}
