using System;
using System.Collections.Generic;

namespace russianRoads.Models;

public partial class Subsuborganization
{
    public int SubsuborganId { get; set; }

    public string SubsuborganName { get; set; } = null!;

    public virtual ICollection<OrganizationsHierarchy> OrganizationsHierarchies { get; set; } = new List<OrganizationsHierarchy>();
}
