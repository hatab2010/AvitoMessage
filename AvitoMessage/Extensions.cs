using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AvitoMessage
{
    public static class Extensions
    {
        public static bool ElementIsExist(this IWebDriver driver, By findOption)
        {
            try
            {
                driver.FindElement(findOption);
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public static IWebElement WaitElement(this IWebDriver driver, By findOption)
        {
            int totalSecond = 0;
            while (true)
            {
                if (driver.ElementIsExist(findOption))
                {
                    return driver.FindElement(findOption);
                }

                Thread.Sleep(1000);
                totalSecond += 1;

                if (totalSecond > 60)
                {
                    throw new Exception($"элемент {findOption} не найден");
                }
            }
        }
    }
}
