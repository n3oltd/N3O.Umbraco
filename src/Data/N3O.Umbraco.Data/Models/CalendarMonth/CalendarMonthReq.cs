using N3O.Umbraco.Attributes;
using N3O.Umbraco.Extensions;
using System.ComponentModel;

namespace N3O.Umbraco.Data.Models; 

public class CalendarMonthReq : Value {
    [Name("Month")]
    [Description("TODO")]
    public int? Month { get; set; }

    [Name("Year")]
    [Description("TODO")]
    public int? Year { get; set; }

    public static explicit operator CalendarMonth(CalendarMonthReq req) {
        return new CalendarMonth(req.Month.GetValueOrThrow(), req.Year.GetValueOrThrow());
    }

    public static implicit operator CalendarMonthReq(CalendarMonth calendarMonth) {
        var req = new CalendarMonthReq();
        req.Month = calendarMonth.Month;
        req.Year = calendarMonth.Year;

        return req;
    }
}
