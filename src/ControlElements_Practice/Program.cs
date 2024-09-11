using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Text;

namespace ControlElements_Practice;

internal class Program
{
    private static IWebDriver webDriver = new ChromeDriver();
    private static WebDriverWait webDriverWait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
    private static readonly string yourCashbackNewURL = "https://ib.psbank.ru/store/products/your-cashback-new";
    //private static readonly string categoryCheckboxesXPath = "//ul[@class='categories-list']//input";
    //private static readonly string categoryNamesXPath = "//ul[@class='categories-list']//span[@class='category-caption']";
    private static readonly string changeCategoriesButtonXPath = "//button[contains(@class,'change-categories-btn')]";

    private static readonly string modalTitleContainerCloseButtonXPath = "//rui-modal-title-container//button[contains(@class,'rui-modal-title-container__close-btn')]";

    private static readonly string cardHolderLastNameInputXPath = "//input[@name='CardHolderLastName']";

    private static Dictionary<IWebElement, string> categoriesDictionary = new();

    internal static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.Unicode;

        webDriver.Manage().Window.Maximize();
        webDriver.Navigate().GoToUrl(yourCashbackNewURL);
        webDriverWait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));

        // 1. Метод взаимодействия с CheckBox

        webDriverWait.Until(driver => driver.FindElement(By.XPath(changeCategoriesButtonXPath))).Click();
        Console.WriteLine("Введите категорию. Например - Такси и каршеринг.");
        SelectCheckbox(Console.ReadLine());
        webDriverWait.Until(driver => driver.FindElement(By.XPath(modalTitleContainerCloseButtonXPath))).Click();
        
        // 2. Метод взаимодействия с выпадающим списоком.
        
        SelectOption("Пу", "Пушкин");

        // 3. Метод взаимодействия с кнопкой скачивания.

        //

        webDriver.Quit();
        webDriver.Dispose();
    }

    /// <summary>
    /// 1. Метод взаимодействия с CheckBox
    /// </summary>
    /// <param name="categoryName"></param>
    private static void SelectCheckbox(string categoryName)
    {
        string categoryDescriptionSpanXPath = $"//ul[@class='categories-list']//span[contains(text(),'{categoryName}')]";
        string categoryCheckboxRUIXPath = $"//ul[@class='categories-list']//span[contains(text(),'{categoryName}')]//preceding-sibling::rui-checkbox";
        string categoryCheckboxInputXPath = $"//ul[@class='categories-list']//span[contains(text(),'{categoryName}')]//preceding-sibling::rui-checkbox//input";
        IWebElement categoryDescriptionSpan = webDriverWait.Until(driver => driver.FindElement(By.XPath(categoryDescriptionSpanXPath)));
        IWebElement checkboxRUI = webDriverWait.Until(driver => driver.FindElement(By.XPath(categoryCheckboxRUIXPath)));
        IWebElement checkboxInput = checkboxRUI.FindElement(By.XPath(categoryCheckboxInputXPath));
        Console.WriteLine(
            $"Категория: {categoryDescriptionSpan.Text}\n" +
            $"Значение категории до клика - {checkboxInput.Selected}"
            );
        checkboxRUI.Click();
        Console.WriteLine($"Значение категории после клика - {checkboxInput.Selected}");

        //categoriesDictionary = webDriverWait.Until(driver => driver.FindElements(By.XPath(categoryCheckboxesXPath)))
        //.Zip(webDriverWait.Until(driver => driver.FindElements(By.XPath(categoryNamesXPath))),
        //    (checkbox, name) => new { checkbox, name })
        //.ToDictionary(x => x.checkbox, x => x.name.Text);
        //int index = categoriesDictionary.Values.ToList().IndexOf(categoryName);
        //categoriesDictionary.Keys.ToList()[index].FindElement(By.XPath("ancestor::rui-checkbox")).Click();
    }

    /// <summary>
    /// 2. Метод взаимодействия с выпадающим списоком.
    /// </summary>
    /// <param name="option"></param>
    private static void SelectOption(string input, string option)
    {
        IWebElement cardHolderLastNameInput = webDriverWait.Until(driver => driver.FindElement(By.XPath(cardHolderLastNameInputXPath)));
        cardHolderLastNameInput.Click();
        cardHolderLastNameInput.SendKeys(input);
        IWebElement cardHolderLastNameOption = webDriverWait.Until(driver => driver.FindElement(By.XPath($"//mat-option//div[text()=' {option} ']")));
        cardHolderLastNameOption.Click();
        Console.WriteLine($"Введённая фамилия - {cardHolderLastNameInput.GetAttribute("value")}");
    }
}