using System;
using System.Collections.Generic;

namespace russianRoads.Models;

public partial class CalendarMissedWorker
{
    public int CalenmissId { get; set; }

    public DateOnly CalenmissDate { get; set; }

    public int? WorkerMissedId { get; set; }

    public int? WorkerReplacedId { get; set; }

    public virtual Worker? WorkerMissed { get; set; }

    public virtual Worker? WorkerReplaced { get; set; }
}
