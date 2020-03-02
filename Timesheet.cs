using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using iTextSharp.text.pdf;

namespace pdfApp
{
    public static class Timesheet
    {
        public static DateTime date = DateTime.Now;
        public static int inputMonth; 
        private static List<Week> weeks;
        private static AcroFields form;
        private static ICollection<string> fieldKeys;
        public static double payRate;
        private const int JUMP_INTERVAL = 12;
        
        public static string name, id, department, jobTitle, currentMonth;
        private static Dictionary<int, string> months = new Dictionary<int, string>() {
            {1, "January"},
            {2, "February"},
            {3, "March"},
            {4, "April"},
            {5, "May"},
            {6, "June"},
            {7, "July"},
            {8, "August"},
            {9, "September"},
            {10, "October"},
            {11, "November"},
            {12, "December"}
        };
        private static Dictionary<int, DateTime> daysInMonth = new Dictionary<int, DateTime>();

        public static void UpdatePdf() {

            currentMonth = months[inputMonth];
            string fileNameExisting = @"timesheet.pdf";
            string fileNameNew = $@"{name} ({currentMonth} timesheet).pdf";
            Console.WriteLine(fileNameNew);

            using (FileStream existingFileStream = new FileStream(fileNameExisting, FileMode.Open))
            using (FileStream newFileStream = new FileStream(fileNameNew, FileMode.Create)) {

                PdfReader pdfReader = new PdfReader(existingFileStream);

                PdfStamper stamper = new PdfStamper(pdfReader, newFileStream);
                form = stamper.AcroFields;
                fieldKeys = form.Fields.Keys;

                // foreach (string fieldKey in fieldKeys) {
                //     form.SetField(fieldKey, fieldKey);
                // }
                
                UpdatePersonalInfo(currentMonth, jobTitle, department,
                    name, id, payRate.ToString());

                UpdateHours();

                //stamper.FormFlattening = true;
                stamper.Close();
                pdfReader.Close();


            }

        }

        public static void UpdateHours() {
            weeks = SiteScraper.Weeks;
            //Console.WriteLine(weeks.Count);
            for (int i = 1; i <= weeks.Count; i++)
            {
                int oldDate = 0;
                Week currentWeek = weeks[i-1];
                
                for (int j = 0; j < currentWeek.TimeIn.Count; j++)
                {
                    switch (currentWeek.TimeIn[j].DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                        {
                            int numberOfWeekDay = 1;
                            int date = CalculateDay(numberOfWeekDay, i - 1);
                            int timeField = CalculateTimeField(numberOfWeekDay, i - 1);
                            if (date != oldDate)
                            {
                                form.SetField("Date" + date, currentWeek.TimeIn[j].ToString("MM/dd"));
                                form.SetField("TI" + (timeField - 1), currentWeek.TimeIn[j].ToString("HH:mm"));
                                form.SetField("TO" + (timeField - 1), currentWeek.TimeOut[j].ToString("HH:mm"));
                                form.SetField("HW" + (timeField - 1), currentWeek.DayHours[j].ToString());
                                form.SetField("TT" + date, currentWeek.DayHours[j].ToString());
                            }
                            else
                            {
                                form.SetField("TI" + (timeField), currentWeek.TimeIn[j].ToString("HH:mm"));
                                form.SetField("TO" + (timeField), currentWeek.TimeOut[j].ToString("HH:mm"));
                                form.SetField("HW" + (timeField), currentWeek.DayHours[j].ToString());
                                form.SetField("TT" + date, (currentWeek.DayHours[j] + currentWeek.DayHours[j-1]).ToString());
                            }
                            oldDate = date;
                            //Console.WriteLine("Date" + date);
                            break;
                        }
                        case DayOfWeek.Tuesday:
                        {
                            int numberOfWeekDay = 2;
                            int date = CalculateDay(numberOfWeekDay, i - 1);
                            int timeField = CalculateTimeField(numberOfWeekDay, i - 1);
                            if (date != oldDate)
                            {
                                form.SetField("Date" + date, currentWeek.TimeIn[j].ToString("MM/dd"));
                                form.SetField("TI" + (timeField - 1), currentWeek.TimeIn[j].ToString("HH:mm"));
                                form.SetField("TO" + (timeField - 1), currentWeek.TimeOut[j].ToString("HH:mm"));
                                form.SetField("HW" + (timeField - 1), currentWeek.DayHours[j].ToString());
                                form.SetField("TT" + date, currentWeek.DayHours[j].ToString());
                            }
                            else
                            {
                                form.SetField("TI" + (timeField), currentWeek.TimeIn[j].ToString("HH:mm"));
                                form.SetField("TO" + (timeField), currentWeek.TimeOut[j].ToString("HH:mm"));
                                form.SetField("HW" + (timeField), currentWeek.DayHours[j].ToString());
                                form.SetField("TT" + date, (currentWeek.DayHours[j] + currentWeek.DayHours[j-1]).ToString());
                            }
                            oldDate = date;
                            //Console.WriteLine("Date" + date);
                            break;
                        }
                        case DayOfWeek.Wednesday:
                        {
                            int numberOfWeekDay = 3;
                            int date = CalculateDay(numberOfWeekDay, i - 1);
                            int timeField = CalculateTimeField(numberOfWeekDay, i - 1);
                            if (date != oldDate)
                            {
                                form.SetField("Date" + date, currentWeek.TimeIn[j].ToString("MM/dd"));
                                form.SetField("TI" + (timeField - 1), currentWeek.TimeIn[j].ToString("HH:mm"));
                                form.SetField("TO" + (timeField - 1), currentWeek.TimeOut[j].ToString("HH:mm"));
                                form.SetField("HW" + (timeField - 1), currentWeek.DayHours[j].ToString());
                                form.SetField("TT" + date, currentWeek.DayHours[j].ToString());
                            }
                            else
                            {
                                form.SetField("TI" + (timeField), currentWeek.TimeIn[j].ToString("HH:mm"));
                                form.SetField("TO" + (timeField), currentWeek.TimeOut[j].ToString("HH:mm"));
                                form.SetField("HW" + (timeField), currentWeek.DayHours[j].ToString());
                                form.SetField("TT" + date, (currentWeek.DayHours[j] + currentWeek.DayHours[j-1]).ToString());
                            }
                            oldDate = date;
                            //Console.WriteLine("Date" + date);
                            break;
                        }
                        case DayOfWeek.Thursday:
                        {
                            int numberOfWeekDay = 4;
                            int date = CalculateDay(numberOfWeekDay, i - 1);
                            int timeField = CalculateTimeField(numberOfWeekDay, i - 1);
                            if (date != oldDate)
                            {
                                form.SetField("Date" + date, currentWeek.TimeIn[j].ToString("MM/dd"));
                                form.SetField("TI" + (timeField - 1), currentWeek.TimeIn[j].ToString("HH:mm"));
                                form.SetField("TO" + (timeField - 1), currentWeek.TimeOut[j].ToString("HH:mm"));
                                form.SetField("HW" + (timeField - 1), currentWeek.DayHours[j].ToString());
                                form.SetField("TT" + date, currentWeek.DayHours[j].ToString());
                            }
                            else
                            {
                                form.SetField("TI" + (timeField), currentWeek.TimeIn[j].ToString("HH:mm"));
                                form.SetField("TO" + (timeField), currentWeek.TimeOut[j].ToString("HH:mm"));
                                form.SetField("HW" + (timeField), currentWeek.DayHours[j].ToString());
                                form.SetField("TT" + date, (currentWeek.DayHours[j] + currentWeek.DayHours[j-1]).ToString());
                            }
                            oldDate = date;
                            //Console.WriteLine("Date" + date);
                            break;
                        }
                        case DayOfWeek.Friday:
                        {
                            int numberOfWeekDay = 5;
                            int date = CalculateDay(numberOfWeekDay, i - 1);
                            int timeField = CalculateTimeField(numberOfWeekDay, i - 1);
                            if (date != oldDate)
                            {
                                form.SetField("Date" + date, currentWeek.TimeIn[j].ToString("MM/dd"));
                                form.SetField("TI" + (timeField - 1), currentWeek.TimeIn[j].ToString("HH:mm"));
                                form.SetField("TO" + (timeField - 1), currentWeek.TimeOut[j].ToString("HH:mm"));
                                form.SetField("HW" + (timeField - 1), currentWeek.DayHours[j].ToString());
                                form.SetField("TT" + date, currentWeek.DayHours[j].ToString());
                            }
                            else
                            {
                                form.SetField("TI" + (timeField), currentWeek.TimeIn[j].ToString("HH:mm"));
                                form.SetField("TO" + (timeField), currentWeek.TimeOut[j].ToString("HH:mm"));
                                form.SetField("HW" + (timeField), currentWeek.DayHours[j].ToString());
                                form.SetField("TT" + date, (currentWeek.DayHours[j] + currentWeek.DayHours[j-1]).ToString());
                            }
                            oldDate = date;
                            //Console.WriteLine("Date" + date);
                            break;
                        }
                        case DayOfWeek.Saturday:
                        {
                            int numberOfWeekDay = 6;
                            int date = CalculateDay(numberOfWeekDay, i - 1);
                            int timeField = CalculateTimeField(numberOfWeekDay, i - 1);
                            if (date != oldDate)
                            {
                                form.SetField("Date" + date, currentWeek.TimeIn[j].ToString("MM/dd"));
                                form.SetField("TI" + (timeField - 1), currentWeek.TimeIn[j].ToString("HH:mm"));
                                form.SetField("TO" + (timeField - 1), currentWeek.TimeOut[j].ToString("HH:mm"));
                                form.SetField("HW" + (timeField - 1), currentWeek.DayHours[j].ToString());
                                form.SetField("TT" + date, currentWeek.DayHours[j].ToString());
                                
                            }
                            else
                            {
                                form.SetField("TI" + (timeField), currentWeek.TimeIn[j].ToString("HH:mm"));
                                form.SetField("TO" + (timeField), currentWeek.TimeOut[j].ToString("HH:mm"));
                                form.SetField("HW" + (timeField), currentWeek.DayHours[j].ToString());
                                form.SetField("TT" + date, (currentWeek.DayHours[j] + currentWeek.DayHours[j-1]).ToString());
                            }
                            oldDate = date;
                            //Console.WriteLine("Date" + date);
                            break;
                        }
                        default:
                            break;
                    }
                }
                form.SetField("Weekly Total" + i, SiteScraper.Weeks[i -1].WeekHours.ToString());
            }
            form.SetField("Monthly Total", SiteScraper.TotalHours.ToString());
            form.SetField("Total Pay", (SiteScraper.TotalHours * payRate).ToString());
            //int days = DateTime.DaysInMonth(date.Year, date.Month);
            // for (int i = 1; i <= days; i++) {
            //     daysInMonth.Add(i, new DateTime(date.Year, date.Month, i));
            //     while (true) {
            //     break;

            //     }
                
                
            // }
           
        }

        public static int CalculateDay(int _numberOfWeekDay, int _weekNumber)
        {
            return _numberOfWeekDay + (_weekNumber * (JUMP_INTERVAL / 2));
        }
        
        public static int CalculateTimeField(int _numberOfWeekDay, int _weekNumber)
        {
            return _numberOfWeekDay + (_weekNumber * JUMP_INTERVAL) + (_numberOfWeekDay);
        }

        public static void Default () {
            name = "Bobby Laudeman";
            id = "005705423";
            department = "ATI";
            jobTitle = "Student Assistant";
            payRate = 14.00;
        }

        public static void UpdatePersonalInfo(string _month, string _jobTitle, string _department, string _name,
            string _id, string _payRate) {
            form.SetField("Month", _month);
            form.SetField("Student Job Title", _jobTitle);
            form.SetField("Department", _department);
            form.SetField("Employee Name", _name);
            form.SetField("Coyote ID", _id);
            form.SetField("Pay Rate", _payRate);
        }
        
    }
}