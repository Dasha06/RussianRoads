using System;
using System.Collections.Generic;

namespace russianRoads.Models;

public partial class LearnAndWorker
{
    public int? LearnId { get; set; }

    public int? WorkerId { get; set; }

    public virtual LearnEvent? Learn { get; set; }

    public virtual Worker? Worker { get; set; }
}
