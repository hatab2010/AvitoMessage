using AvitoMessage.Operations;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AvitoMessage
{
    class Program
    {       
        static void Main(string[] args)
        {
            Dialog.LoadTemplate();
            var worker = new Worker();
            worker.Start();

            Task.Delay(-1).Wait();
        }
    }

    public enum AccountState
    {
        NotAuthorized, Authorized
    }

    public class Worker
    {
        private List<Client> clients = new List<Client>();
        private List<IOperation> operations = new List<IOperation>();
        private AccountState state;
        private IWebDriver driver;

        private WaitMessageOperation WaitOperation;
        private AuthorizationOperation AuthorizationOperation;

        public Worker()
        {
            var dataPath = $"{Directory.GetCurrentDirectory()}/Data";
            var options = new ChromeOptions();
            var services = ChromeDriverService.CreateDefaultService();

            options.AddArgument($"--user-data-dir={dataPath}");
            driver = new ChromeDriver(services, options);

            WaitOperation = new WaitMessageOperation(driver);
            AuthorizationOperation = new AuthorizationOperation(driver);

            WaitOperation.NewMessage += OnNewMessage;
        }

        public void Start()
        {
            //Авторизация
            AuthorizationOperation.Execute();

            //Запускаем очередь операций
            Task.Run(() =>
            {
                while (true)
                {
                    Process();
                    Thread.Sleep(500);
                }
            });
        }

        private void Process()
        {            
            if (operations.Count == 0)
            {
                operations.Add(WaitOperation);
            }

            var currentOpertion = operations.First();
            var success = currentOpertion.Execute();

            if (success)
                operations.Remove(currentOpertion);

            Thread.Sleep(500);
        }

        private void OnNewMessage(string chatId)
        {
            var currentClient = clients.FirstOrDefault(_ => _.Id == chatId);

            if (currentClient == null)
            {
                currentClient = new Client(chatId);
                clients.Add(currentClient);
            }

            if (currentClient.DialogStep < Dialog.Messages.Count())
            {
                operations.Add(new SendMessage(driver, currentClient));
            }
            else
            {
                clients.Remove(currentClient);
            }
        }
    }

    public class Client
    {
        public int DialogStep = 0;
        public string Id { private set; get; }        
        public Client(string id)
        {
            Id = id;
        }        
    }

    public static class Global
    {
        
    }

    public class Dialog
    {
        public Dialog()
        {
        }

        public static List<string> Messages { private set; get; }
        public int currentStep;

        public static void LoadTemplate()
        {
            Messages = new List<string>();
            var path = Directory.GetCurrentDirectory() + "/dialog.txt";
            var text = File.ReadAllText(path);

            var messageCol = text.Split(';');

            foreach(var item in messageCol)
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }

                Messages.Add(item);
            }
        }
    }
}
