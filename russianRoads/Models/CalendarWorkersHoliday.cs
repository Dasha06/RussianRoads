using System;
using System.Collections.Generic;

namespace russianRoads.Models;

public partial class CalendarWorkersHoliday
{
    public int CalenholidayId { get; set; }

    public DateOnly CalenholidayDateStart { get; set; }

    public DateOnly CalenholidayDateEnd { get; set; }

    public int? WorkerId { get; set; }

    public virtual Worker? Worker { get; set; }
}
