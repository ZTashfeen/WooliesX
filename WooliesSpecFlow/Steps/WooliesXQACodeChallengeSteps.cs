using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using TechTalk.SpecFlow;

namespace WooliesSpecFlow.Steps
{
    [Binding]
    public class IPlaceOrderPages
    {
        public static IWebDriver Driver { get; set; }
        public static ChromeOptions GetChromeOptions(string strDownloadFolderPath)
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--disable-extensions");
            chromeOptions.AddArguments("--ignore-certificate-errors");
            chromeOptions.AddArguments("no-sandbox");

            chromeOptions.AddUserProfilePreference("profile.default_content_settings.popups", 0);
            chromeOptions.AddUserProfilePreference("profile.content_settings.exceptions.automatic_downloads.*.setting", 1);
            chromeOptions.AddUserProfilePreference("download.prompt_for_download", false);
            chromeOptions.AddUserProfilePreference("download.default_directory", strDownloadFolderPath);
            return chromeOptions;
        }



        [Given(@"I navigate to url")]
        public void GivenINavigateToUrl()
        {
            NavigateToURL();
        }

        [Given(@"I add items to cart and sign in successfully")]
        public void GivenIAddItemsToCartAndSignInSuccessfully()
        {
            Additems();
            ShoppingCartSummary();
            RegisterationProcess();
            ProcessAddress();
        }

        [When(@"I place order")]
        public void WhenIPlaceOrder()
        {
            Shipping();
            Payment();
        }

        [Then(@"Order is successfully placed")]
        public void ThenOrderIsSuccessfullyPlaced()
        {
            Order();
            Driver.Quit();
        }

        


        //Implementation
        public void NavigateToURL()
        {
            var chromeOptions = GetChromeOptions(Environment.CurrentDirectory);
            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
            Driver = new ChromeDriver(chromeDriverService, chromeOptions);
            Driver.Url = "http://automationpractice.com/index.php";
            Driver.Navigate();
        }

        public void Additems()
        {
            if (ElementContainsText("a[class=product-name]","Short Sleeve"))
            {
                ClickByCSS("a[class=product-name]");
            }
            WaitforElementVisible("button[name=Submit]");
            Driver.FindElement(By.CssSelector("input[id=quantity_wanted]")).Clear();
            SendKeysByCSS("input[id=quantity_wanted]", "2");
            ClickByCSS("button[name=Submit]");
            WaitforElementVisible(".btn.btn-default.button.button-medium");
            ClickByCSS(".btn.btn-default.button.button-medium");
        }

        public void CheckOutAsGuest()
        {
            WaitforElementVisible("div[id=uniform-id_gender2]");
            ClickByCSS("div[id=uniform-id_gender2]");
            SendKeysByCSS("input[id=customer_firstname]", "First");
            SendKeysByCSS("input[id=customer_lastname]", "Last");
            ClickByCSS("input[id=email]");
            SendKeysByCSS("input[type=password]", "password");
            ClickByCSS("select[id=days]");
            SendKeysByCSS("select[id=days]", Keys.Down);
            ClickByCSS("select[id=months]");
            SendKeysByCSS("select[id=months]", Keys.Down);
            ClickByCSS("select[id=years]");
            SendKeysByCSS("select[id=years]", "1980");
            ClickByCSS("input[id=firstname]");
            ClickByCSS("input[id=lastname]");
            SendKeysByCSS("input[id=address1]", "123 wrong address");
            SendKeysByCSS("input[id=city]", "Cedar Rapids");
            SendKeysByCSS("select[id=id_state]", "Iowa");
            SendKeysByCSS("input[id=postcode]", "52228");
            SendKeysByCSS("input[id=phone_mobile]", "010000000000");
            SendKeysByCSS("input[id=alias]", " shopping");
            ClickByCSS("button[id=submitAccount]");
        }


        public void RegisterationProcess()
        {
            SendKeysByCSS("input[id=email_create]", "wrongemail@abc.net");
            ClickByCSS("button[id=SubmitCreate]");
            WaitforElementVisible("div[id=create_account_error]");
            if (ElementExists("div[id=create_account_error]"))
            {
                RegisteredCustomer();
            }
            else
            {
                CheckOutAsGuest();
            }

        }

        public void RegisteredCustomer()
        {
            SendKeysByCSS("input[id=email]", "wrongemail@abc.net"); // login email 
            SendKeysByCSS("input[type=password]", "password"); // login password 
            ClickByCSS("button[id=SubmitLogin]"); //Submit
        }

        public void ProcessAddress()
        {
            ClickByCSS("button[name=processAddress]");
        }

        public void Shipping()
        {
            ClickByCSS("input[id=cgv]");
            ClickByCSS("button[name=processCarrier]");
        }

        public void ShoppingCartSummary()
        {
            ClickByCSS("a.button.btn.btn-default.standard-checkout.button-medium");
        }

        public void Payment()
        {
            ClickByCSS("a[class=bankwire]");
        }

        public void Summary()
        {
            ClickByCSS("button.btn.btn-default.button-medium");
        }

        public void Order()
        {
            ElementContainsText("p[class=cheque-indent]", "complete");
            Console.WriteLine("Order Complete");
        }




        public void WaitforElementVisible(string ByCSS)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(60));
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector(ByCSS)));
        }

        public void ClickByCSS(string ByCSS)
        {
            Driver.FindElement(By.CssSelector(ByCSS)).Click();
        }

        public void SendKeysByCSS(string ByCSS, string Text)
        {
            Driver.FindElement(By.CssSelector(ByCSS)).SendKeys(Text);
        }

        public bool ElementExists(string ByCSS)
        {
            bool result = Driver.FindElement(By.CssSelector(ByCSS)).Displayed;
            return result;
        }

        public bool ElementContainsText(string ByCSS, string text)
        {
            bool result = Driver.FindElement(By.CssSelector(ByCSS)).Text.Contains(text);
            return result;
        }
    }
}
