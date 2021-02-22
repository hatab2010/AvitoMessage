using AvitoMessage.Pages;
using OpenQA.Selenium;
using System;
using System.Threading;
using System.Windows;

namespace AvitoMessage.Operations
{
    class AuthorizationOperation : OperationBase
    {
        public AuthorizationOperation(IWebDriver driver) : base(driver)
        {
            OperationPage = new Page("https://www.avito.ru");
        }

        protected override void Action()
        {
            if (!IsAuthorized())
            {
                MessageBox.Show("Пройдите авторизацию на сайте avito.ru");

                while (true)
                {
                    if (IsAuthorized()) break;
                    Thread.Sleep(1000);
                }
            }

        }

        public bool IsAuthorized()
        {
            if (driver.ElementIsExist(By.XPath("//div[contains(@class, 'header-services-menu-avatar')]")))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
