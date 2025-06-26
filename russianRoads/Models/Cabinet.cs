using System;
using System.Collections.Generic;

namespace russianRoads.Models;

public partial class Cabinet
{
    public int CabId { get; set; }

    public string CabNumber { get; set; } = null!;

    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
