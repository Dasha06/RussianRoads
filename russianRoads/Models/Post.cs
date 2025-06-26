using System;
using System.Collections.Generic;

namespace russianRoads.Models;

public partial class Post
{
    public int PostId { get; set; }

    public string PostName { get; set; } = null!;

    public int? PostIsprimId { get; set; }

    public virtual Ismain? PostIsprim { get; set; }

    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
