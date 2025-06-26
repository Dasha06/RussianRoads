using System;
using System.Collections.Generic;

namespace russianRoads.Models;

public partial class CalendarLearning
{
    public int CalenlearnId { get; set; }

    public DateOnly CalenlearnDateStart { get; set; }

    public DateOnly CalenlearnDateEnd { get; set; }

    public int? CalenlearnEventId { get; set; }

    public virtual LearnEvent? CalenlearnEvent { get; set; }
}
