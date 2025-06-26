using System;
using System.Collections.Generic;

namespace russianRoads.Models;

public partial class OrganizationsHierarchy
{
    public int HierId { get; set; }

    public int? HierOrganId { get; set; }

    public int? HierSuborganId { get; set; }

    public int? HierSubsuborganId { get; set; }

    public virtual Organization? HierOrgan { get; set; }

    public virtual Suborganization? HierSuborgan { get; set; }

    public virtual Subsuborganization? HierSubsuborgan { get; set; }

    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
