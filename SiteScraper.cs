using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using HtmlAgilityPack;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdfApp
{
    public static class SiteScraper
    {
        
        private static string url = "https://61783.tcplusondemand.com/app/webclock/#/EmployeeLogOn/61783";
        public static string password;
        private static IWebDriver driver;
        //This will be the first day on your timesheet....find a way for user to enter what day.
        private static DateTime _begOfTimesheet;
        //This will be the last day on your timesheet....find a way for user to enter what day.
        private static DateTime _endOfTimesheet;
        private static HtmlDocument html = new HtmlDocument();
        private static List<Week> weeks = new List<Week>();
        public static List<Week> Weeks {
            get {return weeks;}
        }
        private static DateTime begDate, endDate;
        private static double totalHours;
        public static double TotalHours {
            get {return totalHours;}
        }


        public static void SetBegAndEndOfTimesheet(int begDay, int endDay) {
            _begOfTimesheet = new DateTime(Timesheet.date.Year, Timesheet.inputMonth, begDay);
            _endOfTimesheet = new DateTime(Timesheet.date.Year, Timesheet.inputMonth, endDay, 23, 59, 59); 
            // Added the time specification so that it counts the hours for that day as well.
        }

        public static string PWD() {
            return Directory.GetCurrentDirectory();
        }

        public static void SetPassword(string pass) {
            password = pass;
        }

        // Main method for this class...could probably use a better name.
        public static void Activate() {
            Login();
            FindFirstWorkWeek();
        }


        public static void Login() {
            // id is the password that the user sets.
            //Console.WriteLine(Timesheet.id.Substring(Timesheet.id.Length - 5, 5));
            //SetPassword(Timesheet.id.Substring(Timesheet.id.Length - 5, 5));
            Console.WriteLine(Directory.GetCurrentDirectory());
            string path = Directory.GetCurrentDirectory();
            driver = new ChromeDriver(path);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Navigate().GoToUrl(url);
            System.Threading.Thread.Sleep(5000);

            IWebElement passwordForm = driver.FindElement(By.Id("LogOnEmployeeId"));
            IWebElement submitButton = driver.FindElement(By.ClassName("DefaultSubmitBehavior"));
            passwordForm.SendKeys(password);
            submitButton.Click();
            try
            {
                IWebElement viewButton = driver.FindElement(By.Id("View"));
                viewButton.Click();
            }
            catch (System.Exception)
            {
                Console.WriteLine("Retrying Login...");
                passwordForm.SendKeys(password);
                submitButton.Click();
            }
        IWebElement viewHoursButton = driver.FindElement(By.Id("ViewHours"));
            viewHoursButton.Click();
            
            
            // Gets the name of the current user from the website.
            Timesheet.name = driver.FindElement(By.ClassName("EmployeeFullName")).Text;
        }

        private static void GetCurrentWeek() {
            IWebElement currentWeek = driver.FindElement(By.ClassName("PeriodTotal"));
            string currentWeekText = currentWeek.Text;
            string begOfWeek, endOfWeek;

            //Console.WriteLine(currentWeekText);

            string[] days = currentWeekText.Split(' ');
            //Console.WriteLin5e("Days: " + days[0] + days[1] + days[2]);


            begOfWeek = days[0] + $"/{Timesheet.date.Year}";
            endOfWeek = days[2] + $"/{Timesheet.date.Year}";
                    

            begDate = Convert.ToDateTime(begOfWeek);
            endDate = Convert.ToDateTime(endOfWeek);


            //Console.WriteLine($"{begDate} -> {endDate}");

        }

        private static void GoToNextPage() {
            IWebElement nextButton = driver.FindElement(By.Id("Next"));
            TryToClickElement(nextButton);
        }

        private static void GoToPreviousPage() {
            IWebElement previousButton = driver.FindElement(By.Id("Previous"));
            TryToClickElement(previousButton);
        }



        public static void FindFirstWorkWeek() {
            GetCurrentWeek();
            //Console.WriteLine($"beg of timesheet -> {_begOfTimesheet} // begDate -> {begDate} // endDate -> {endDate}");
            // Check to see if the current week displayed in the chrome driver is within the timesheet time period.
            if ((begDate <= _begOfTimesheet && _begOfTimesheet <= endDate) || (begDate.Month > endDate.Month))  {
                ScrapeHours();
                return;
            }
            else if (begDate <= _begOfTimesheet && _begOfTimesheet >= endDate)
            {
                GoToNextPage();
                FindFirstWorkWeek();
            }

            else {
                GoToPreviousPage();
                FindFirstWorkWeek();
            }

        }




        private static void TryToClickElement(IWebElement button) {
            try {
                var old = begDate;
                button.Click();
                GetCurrentWeek();
                //Wait for page to load before continuing.
                while (old == begDate) {
                    System.Threading.Thread.Sleep(100);
                    GetCurrentWeek();
                }
                
            }
            catch (Exception) {
                System.Threading.Thread.Sleep(200);
            }
        }


        private static void ScrapeHours() {
            var oldDate = begDate;
            GetCurrentWeek();

            //Console.WriteLine("Scraping...");
            html.LoadHtml(driver.PageSource);
            var table = html.DocumentNode.SelectSingleNode("//*[@id='featureForm']/div[2]/div/table/tbody");
            //for (int i = 0; i < table.SelectNodes)
            var tableRows = table.SelectNodes("tr");
            //.First().Elements("td");
            //.First().Elements("td");
            Week newWeek = new Week();

            foreach (var row in tableRows) {
                foreach (var line in row.Elements("td")) {
                    if (line.Attributes["ng-bind"] != null) {

                        if (line.Attributes["ng-bind"].Value == "workSegment.strFormattedDateTimeIn") {
                            //Console.WriteLine(Convert.ToDateTime(line.InnerHtml));
                            var dateTime = Convert.ToDateTime(line.InnerHtml);
                            //Console.WriteLine(_endOfTimesheet);
                            // Added this to make sure that the dates within this week are within the timesheet period before adding.
                            if (dateTime <= _endOfTimesheet && dateTime >= _begOfTimesheet)
                                newWeek.TimeIn.Add(Convert.ToDateTime(line.InnerHtml));
                        }

                        else if (line.Attributes["ng-bind"].Value == "workSegment.strFormattedDateTimeOut") {
                            try
                            {
                                var dateTime = Convert.ToDateTime(line.InnerHtml);
                                if (dateTime <= _endOfTimesheet && dateTime >= _begOfTimesheet)
                                    newWeek.TimeOut.Add(Convert.ToDateTime(line.InnerHtml));   
                            }
                            catch (System.Exception)
                            {
                                newWeek.TimeOut.Add(newWeek.TimeIn.Last());
                                break;
                            }
                        }

                    }
                }
            }
            newWeek.CalculateHours();
            // Don't want to add the week if it doesn't have any hours.
            if (newWeek.WeekHours > 0)
                weeks.Add(newWeek);
            // Check if this is the last week, if it is we don't want to go to the next page.
            if (endDate >= _endOfTimesheet) {
                //Console.WriteLine("This is the last week");
                foreach (var week in weeks)
                {
                    totalHours += week.WeekHours;
                }
                //Console.WriteLine($"Total Hours: {totalHours}");
                //Console.WriteLine($"Total Pay: {totalHours * 14}");
                return;
            }
            else {
                GoToNextPage();
                ScrapeHours();
            }
                

        }
    }
}