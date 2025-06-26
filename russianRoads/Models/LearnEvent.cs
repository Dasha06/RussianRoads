using System;
using System.Collections.Generic;

namespace russianRoads.Models;

public partial class LearnEvent
{
    public int LearneventId { get; set; }

    public string LearneventName { get; set; } = null!;

    public string? LearneventDescription { get; set; }

    public virtual ICollection<CalendarLearning> CalendarLearnings { get; set; } = new List<CalendarLearning>();
}
