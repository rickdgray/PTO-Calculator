using System;
using System.CommandLine;
using System.IO;
using System.Threading.Tasks;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;

namespace PTO_Calculator
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            var daysEarnedPerYearOption = new Option<int>(
                name: "--days-earned-per-year",
                description: "The number of PTO days earned per year."
            )
            {
                IsRequired = true
            };

            var daysReservedOption = new Option<int>(
                name: "--days-reserved",
                description: "The number of PTO days reserved for planned vacation."
            )
            {
                IsRequired = true
            };

            var rootCommand = new RootCommand("PTO Calculator");
            rootCommand.AddOption(daysEarnedPerYearOption);
            rootCommand.AddOption(daysReservedOption);

            rootCommand.SetHandler(
                CalculatePto,
                daysEarnedPerYearOption,
                daysReservedOption
            );

            return await rootCommand.InvokeAsync(args);
        }

        static void CalculatePto(int daysEarnedPerYear, int daysReserved)
        {
            var calendar = new Calendar();

            //timezones: https://nodatime.org/TimeZones
            calendar.AddTimeZone(new VTimeZone("America/Chicago"));
            calendar.AddProperty("X-WR-CALNAME", "PTO Accrual");

            var days_till_next_full_pto_day_accrual = 365 / (daysEarnedPerYear - daysReserved);

            var currentYear = DateTime.Now.Year;
            var currentDate = new DateTime(currentYear, 1, 1).AddDays(days_till_next_full_pto_day_accrual);
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

                newEvent.End = new CalDateTime(newEvent.Start.AddDays(1));
                newEvent.Summary = $"{dayCount}/5 weeks PTO Accrued";

                calendar.Events.Add(newEvent);

                currentDate = currentDate.AddDays(days_till_next_full_pto_day_accrual);
                dayCount++;
            }

            var serializer = new CalendarSerializer();
            File.WriteAllText("pto.ics", serializer.SerializeToString(calendar));
        }
    }
}