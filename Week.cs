using System;
using System.Collections.Generic;
using System.Linq;

namespace pdfApp
{
    public class Week
    {
        public List<DateTime> TimeIn { get => _timeIn; set => _timeIn = value; }
        public List<DateTime> TimeOut { get => _timeOut; set => _timeOut = value; }
        public List<double> DayHours { get { return _dayHours; } set { _dayHours = value; } }
        public double WeekHours { get { return _weekHours; } set { _weekHours = value; } }
        private List<double> _dayHours = new List<double>();
        private double _weekHours;
        private List<DateTime> _timeIn = new List<DateTime>();
        private List<DateTime> _timeOut = new List<DateTime>();


        public void CalculateHours() {
            for (int i = 0; i < _timeIn.Count; i++) 
            {
                if (_timeIn[i] != null && _timeOut[i] != null) 
                {
                    //Console.WriteLine("Time in, Time of day: " + _timeIn[i].TimeOfDay);
                    // if (_timeIn[i].TimeOfDay == )
                    var difference = _timeOut[i].Subtract(_timeIn[i]);
                    //Console.WriteLine($"Day Hours: {difference.TotalHours}");
                    _dayHours.Add(difference.TotalHours);
                }
            }
            _weekHours = _dayHours.Sum();
            //Console.WriteLine($"Week Hours: {_weekHours}");
        }
    }
}