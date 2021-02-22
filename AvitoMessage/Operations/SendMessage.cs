using OpenQA.Selenium;
using AvitoMessage.Pages;

namespace AvitoMessage.Operations
{
    public class SendMessage : OperationBase
    {
        private Client client;
        public SendMessage(IWebDriver driver, Client client) : base(driver) 
        {
            this.client = client;
            OperationPage = new Page("https://www.avito.ru/profile/messenger/channel/" + client.Id);
        }

        protected override void Action()
        {
            var input = driver.WaitElement(By.ClassName("channel-bottom-base-input-1P_NB"));
            input.SendKeys(Dialog.Messages[client.DialogStep]);
            input.SendKeys(Keys.Enter);
            client.DialogStep += 1;
        }
    }
}
