using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AvitoMessage.Pages;

namespace AvitoMessage.Operations
{
    public class WaitMessageOperation : OperationBase
    {
        public event Action<string> NewMessage;
        private const string BOLD_CSS_CLASS = "channel-preview-root_boldText-3N-fs";
        public WaitMessageOperation(IWebDriver driver) : base(driver)
        {
            OperationPage = new Page("https://www.avito.ru/profile/messenger");
        }

        protected override void Action()
        {  
           var boldMessage = driver.ElementIsExist(By.ClassName(BOLD_CSS_CLASS));

            if (boldMessage)
            {
                var linkEl = driver
                    .FindElement(By.XPath($"//div[contains(@class, '{BOLD_CSS_CLASS}')]//parent::a"));
                var linkPath = linkEl.GetAttribute("href");
                var chatId = Regex.Match(linkPath, @"u2i-\d*-\d*", RegexOptions.IgnoreCase).Value;
                NewMessage?.Invoke(chatId);
            }
        }        
    }
}
