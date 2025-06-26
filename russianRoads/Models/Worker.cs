using System;
using System.Collections.Generic;

namespace russianRoads.Models;

public partial class Worker
{
    public int WorkerId { get; set; }

    public string WorkerFio { get; set; } = null!;

    public int? WorkerPostId { get; set; }

    public string WorkerWorkphone { get; set; } = null!;

    public string? WorkerPersonalphone { get; set; }

    public int? WorkerCabId { get; set; }

    public string WorkerEmail { get; set; } = null!;

    public DateOnly? WorkerBirtday { get; set; }

    public int? WorkerOrganId { get; set; }

    public virtual ICollection<CalendarMissedWorker> CalendarMissedWorkerWorkerMisseds { get; set; } = new List<CalendarMissedWorker>();

    public virtual ICollection<CalendarMissedWorker> CalendarMissedWorkerWorkerReplaceds { get; set; } = new List<CalendarMissedWorker>();

    public virtual ICollection<CalendarWorkersHoliday> CalendarWorkersHolidays { get; set; } = new List<CalendarWorkersHoliday>();

    public virtual Cabinet? WorkerCab { get; set; }

    public virtual OrganizationsHierarchy? WorkerOrgan { get; set; }

    public virtual Post? WorkerPost { get; set; }
}
