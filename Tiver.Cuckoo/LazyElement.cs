using System;
using System.Collections.ObjectModel;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using Tiver.Fowl.Waiting;

namespace Tiver.Cuckoo
{
    public class LazyElement : IWebElement, IWrapsElement
    {
        public LazyElement(IWebDriver driver, By locator)
        {
            _driver = driver;
            _locator = locator;
        }

        // Process

        public TResult Process<TResult>(Func<IWebElement, TResult> function)
        {
            var result = default(TResult);
            Wait.Until(() =>
            {
                result = function.Invoke(WebElement);
                return true;
            }, typeof(NoSuchElementException), typeof(StaleElementReferenceException));

            return result;
        }

        public void Process(Action<IWebElement> action)
        {
            Process(e =>
            {
                action.Invoke(e);
                return true;
            });
        }

        // Disable find

        public IWebElement FindElement(By by)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            throw new NotImplementedException();
        }

        // Lazy actions

        public void Clear()
        {
            Process(e => e.Clear());
        }

        public void SendKeys(string text)
        {
            Process(e => e.SendKeys(text));
        }

        public void Submit()
        {
            Process(e => e.Submit());
        }

        public void Click()
        {
            Process(e => e.Click());
        }

        public string GetAttribute(string attributeName)
        {
            return Process(e => e.GetAttribute(attributeName));
        }

        public string GetProperty(string propertyName)
        {
            return Process(e => e.GetProperty(propertyName));
        }

        public string GetCssValue(string propertyName)
        {
            return Process(e => e.GetCssValue(propertyName));
        }

        public string TagName => Process(e => e.TagName);
        public string Text => Process(e => e.Text);
        public bool Enabled => Process(e => e.Enabled);
        public bool Selected => Process(e => e.Selected);
        public Point Location => Process(e => e.Location);
        public Size Size => Process(e => e.Size);
        public bool Displayed => Process(e => e.Displayed);
        public IWebElement WrappedElement => WebElement;

        private IWebElement WebElement => _driver.FindElement(_locator);
        private readonly IWebDriver _driver;
        private readonly By _locator;
    }
}