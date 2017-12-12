using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace Tiver.Cuckoo
{
    public class LazyWebDriver : IWebDriver, IJavaScriptExecutor, ITakesScreenshot, IWrapsDriver
    {
        public LazyWebDriver(IWebDriver driver)
        {
            _driver = driver;
        }

        // Lazy find

        public IWebElement FindElement(By by)
        {
            return new LazyElement(WrappedDriver, by);
        }

        // Disable find elements

        public ReadOnlyCollection<IWebElement> FindElements(By @by)
        {
            throw new System.NotImplementedException();
        }


        // Not modified

        public void Dispose()
        {
            _driver.Dispose();
        }

        public void Close()
        {
            _driver.Close();
        }

        public void Quit()
        {
            _driver.Quit();
        }

        public IOptions Manage()
        {
            return _driver.Manage();
        }

        public INavigation Navigate()
        {
            return _driver.Navigate();
        }

        public ITargetLocator SwitchTo()
        {
            return _driver.SwitchTo();
        }

        public object ExecuteScript(string script, params object[] args)
        {
            return (_driver as IJavaScriptExecutor)?.ExecuteScript(script, args);
        }

        public object ExecuteAsyncScript(string script, params object[] args)
        {
            return (_driver as IJavaScriptExecutor)?.ExecuteAsyncScript(script, args);
        }

        public Screenshot GetScreenshot()
        {
            return (_driver as ITakesScreenshot)?.GetScreenshot();
        }

        public string Url
        {
            get { return _driver.Url; }
            set { _driver.Url = value; }
        }

        public IWebDriver WrappedDriver => _driver;
        public string Title => _driver.Title;
        public string PageSource => _driver.PageSource;
        public string CurrentWindowHandle => _driver.CurrentWindowHandle;
        public ReadOnlyCollection<string> WindowHandles => _driver.WindowHandles;

        private readonly IWebDriver _driver;
    }
}