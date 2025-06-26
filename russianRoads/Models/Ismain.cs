using System;
using System.Collections.Generic;

namespace russianRoads.Models;

public partial class Ismain
{
    public int IsmainId { get; set; }

    public string IsmainName { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
