using System;
using System.IO;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;

namespace PtoCalculator
{
    class Program
    {
        static void Main()
        {
            var CURRENT_YEAR = 2021;
            var PTO_DAYS_EARNED_PER_YEAR = 20.0;
            var PTO_DAYS_NEEDED_FOR_WINTER_HOLIDAYS = 7;

            //var testText = File.ReadAllText(new DirectoryInfo("test.ics").FullName);
            //var test = Calendar.Load(testText);

            var calendar = new Calendar();

            //timezones: https://nodatime.org/TimeZones
            calendar.AddTimeZone(new VTimeZone("America/Chicago"));

            var days_till_next_full_pto_day_accrual = 365 / (PTO_DAYS_EARNED_PER_YEAR  - PTO_DAYS_NEEDED_FOR_WINTER_HOLIDAYS);

            var currentDate = new DateTime(CURRENT_YEAR, 1, 1).AddDays(days_till_next_full_pto_day_accrual);
            var endDate = currentDate.AddYears(1);

            var dayCount = 1;

            while (currentDate < endDate)
            {
                var newEvent = new CalendarEvent();

                if (currentDate.Hour > 0 || currentDate.Minute > 0 || currentDate.Second > 0)
                {
                    //round to next day
                    newEvent.Start = new CalDateTime(new DateTime(currentDate.Year, currentDate.Month, currentDate.Day).AddDays(1));
                }
                else
                {
                    newEvent.Start = new CalDateTime(currentDate);
                }

                newEvent.End = new CalDateTime(currentDate.AddDays(1));
                newEvent.Summary = $"{dayCount}/5 PTO Days Accrued";

                calendar.Events.Add(newEvent);

                currentDate = currentDate.AddDays(days_till_next_full_pto_day_accrual);
                dayCount++;
            }

            var serializer = new CalendarSerializer();
            File.WriteAllText("pto.ics", serializer.SerializeToString(calendar));
        }
    }
}
