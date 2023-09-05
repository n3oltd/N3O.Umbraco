using N3O.Umbraco.Attributes;
using N3O.Umbraco.Extensions;
using System;

namespace N3O.Umbraco.Data.Models; 

public class CalendarWeekReq : Value {
    [Name("Week")]
    public int? Week { get; set; }

    [Name("Year")]
    public int? Year { get; set; }

    public static explicit operator CalendarWeek(CalendarWeekReq req) {
        return new CalendarWeek(req.Week.GetValueOrThrow(), req.Year.GetValueOrThrow());
    }

    public static implicit operator CalendarWeekReq(CalendarWeek calendarWeek) {
        var req = new CalendarWeekReq();
        req.Week = calendarWeek.Week;
        req.Year = calendarWeek.Year;

        return req;
    }
}
