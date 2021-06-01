using System;
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
            var calendar = new Calendar();
            calendar.Events.Add(new CalendarEvent
            {
                Start = new CalDateTime(DateTime.Now),
                End = new CalDateTime(DateTime.Now.AddDays(1))
            });

            var serializer = new CalendarSerializer();
            Console.WriteLine(serializer.SerializeToString(calendar));
        }
    }
}
