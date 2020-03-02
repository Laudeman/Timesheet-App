using System;

namespace pdfApp
{
    class Program
    {
        static void Main(string[] args)
        {
            int begDay;
            int endDay;
            
            Console.Write("Bobby? y/n: ");
            if (Console.ReadLine() == "y")
            {
                Timesheet.Default();
            }

            else
            {
                Console.Write("Enter your student id: ");
                Timesheet.id = Console.ReadLine();

                Console.Write("Enter your hourly pay: ");
                Timesheet.payRate = TryParsePayRate();
            }

            Console.Write("Enter Password: ");
            SiteScraper.password = Console.ReadLine();

            Console.Write ("Enter the month number for the timesheet: ");
            Timesheet.inputMonth = TryParseTimeSheetMonth();
            
            Console.Write("Enter the first day of the timesheet: ");
            begDay = TryParseTimeSheetDay();
            
            Console.Write("Enter last day of the timesheet: ");
            endDay = TryParseTimeSheetDay();
            
            SiteScraper.SetBegAndEndOfTimesheet(begDay, endDay);
            
            Console.WriteLine(SiteScraper.PWD());
            Console.WriteLine(Timesheet.date);
            
            SiteScraper.Activate();
            
            Timesheet.UpdatePdf();
        }

        static double TryParsePayRate()
        {
            try
            {
                return double.Parse(Console.ReadLine());
            }
            catch (System.Exception)
            {
                Console.Write("Invalid pay, try again: ");
                return TryParsePayRate();
            }
        }

        static int TryParseTimeSheetDay()
        {
            try
            {
                return Int32.Parse(Console.ReadLine());
            }
            catch (System.Exception)
            {
                Console.Write("Invalid day, try again: ");
                return TryParseTimeSheetDay();
            }
        }
        static int TryParseTimeSheetMonth()
        {
            int month;
            try
            {
                month = Int32.Parse(Console.ReadLine());
                if (month <= 12 && month >= 1)
                {
                    return month;
                }   
                else
                {
                    throw new System.Exception();
                }
            }
            catch (System.Exception)
            {
                
                Console.WriteLine("Invalid month please try again: ");
                return TryParseTimeSheetMonth();
            }
            
            
        }
    }
}
