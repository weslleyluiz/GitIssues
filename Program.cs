using OpenQA.Selenium;
using OpenQA.Selenium.Chrome; 
namespace gitIssues
{
    class Program
    {
        static void Main(string[] args)
        {
            var username = "";
            var password = "";
            var repo = "";
            // parameter "." will instruct to look for the chromedriver.exe in the current folder
            using (var driver = new ChromeDriver("."))
            {
                //Navigate to github issues repository page
                driver.Navigate().GoToUrl($"https://github.com/{username}/{repo}/issues");
                //Click the new issue button and do login
                driver.FindElement(By.XPath("//*[@id=\"repo-content-pjax-container\"]//*/details/summary")).Click();
                driver.FindElement(By.XPath("*//p[@class=\"mt-4 color-text-secondary text-center\"][2]//a")).Click();
                driver.FindElement(By.XPath("//*[@id=\"login_field\"]")).SendKeys(username);
                driver.FindElement(By.XPath("//*[@id=\"password\"]")).SendKeys(password);
                driver.FindElement(By.XPath("*//input[@name=\"commit\" ]")).Click();

                //read input file ( pattern [id ; title ; description ]
                var data = System.IO.File.ReadAllLines("./Input.txt");

                for (int i = 0; i < data.Length; i++)
                {
                    if (string.IsNullOrEmpty(data[i]) || string.IsNullOrWhiteSpace(data[i]))
                        continue;
                    var line = data[i];
                    var fields = line.Split(';');
                    //fill title
                    driver.FindElement(By.XPath("//*[@id=\"issue_title\"]")).SendKeys(fields[0] + " - " + fields[1]);
                    //fill description
                    driver.FindElement(By.XPath("//*[@id=\"issue_body\"]")).SendKeys(fields[2]);
                    //select label
                    driver.FindElement(By.XPath("//*[@id=\"labels-select-menu\"]/summary")).Click();
                    System.Threading.Thread.Sleep(170);
                    driver.FindElement(By.XPath("//*[@id=\"label-filter-field\"]")).SendKeys("todo");
                    System.Threading.Thread.Sleep(170);
                    var builder = new OpenQA.Selenium.Interactions.Actions(driver);
                    builder.SendKeys(Keys.Enter);
                    builder.SendKeys(Keys.Escape);
                    builder.Perform(); 
                    System.Threading.Thread.Sleep(70);
                    //select project
                    driver.FindElement(By.XPath("//*[@id=\"projects-select-menu\"]/summary")).Click();
                    System.Threading.Thread.Sleep(800);
                    driver.FindElement(By.XPath("//*[@id=\"project-sidebar-filter-field\"]")).SendKeys("Development");
                    builder = new OpenQA.Selenium.Interactions.Actions(driver);
                    builder.SendKeys(Keys.Enter);
                    builder.SendKeys(Keys.Escape);
                    builder.Perform();

                    driver.FindElement(By.XPath("*//button[@class=\"btn-primary btn\"]")).Click();
                    if ((i + 1) == data.Length)
                        break;
                    else
                    {
                        builder = new OpenQA.Selenium.Interactions.Actions(driver);
                        builder.SendKeys("c");
                        builder.Perform();
                    }
                }
                driver.Close();
            }
        }
    }
}
