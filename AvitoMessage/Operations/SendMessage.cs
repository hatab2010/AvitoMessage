using OpenQA.Selenium;
using AvitoMessage.Pages;
using System.Text.RegularExpressions;

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
            if (client.DialogStep > Dialog.template.interview.Count)
                return;
            
            if (client.DialogStep > 0 && client.DialogStep < Dialog.template.interview.Count + 1)
            {
                
                var answerEl = driver.WaitElement(By.XPath("(//div[contains(@class, 'message-base-message')])[last()]"));

                var isCorrect = Regex.IsMatch(answerEl.Text, @"(да)|(нет)", RegexOptions.IgnoreCase);
                if (isCorrect)
                {
                    var answerVallue = Regex.IsMatch(answerEl.Text, "да", RegexOptions.IgnoreCase);
                    var lastQuestion = Dialog.template.interview[client.DialogStep - 1];

                    if (lastQuestion.important == true
                        && answerVallue != lastQuestion.correctValue)
                    {
                        client.IsApproved = false;
                    }
                }
                else
                {
                    FormatedSend(Dialog.template.warning, input);
                    return;
                }
            }

            if (client.DialogStep < Dialog.template.interview.Count)
            {
                var currentMessage = Dialog.template.interview[client.DialogStep].message;
                FormatedSend(currentMessage, input);
            }
            
            if (client.DialogStep == Dialog.template.interview.Count)
            {
                var currentMessage = client.IsApproved == true ? Dialog.template.success : Dialog.template.fail;
                FormatedSend(currentMessage, input);
            }

            client.DialogStep += 1;
        }

        void FormatedSend(string msg, IWebElement input)
        {
            var lines = msg.Split('\n');

            foreach (var line in lines)
            {
                input.SendKeys(line);
                input.SendKeys(Keys.Shift + Keys.Enter);
            }

            input.SendKeys(Keys.Enter);
        }
    }
}
