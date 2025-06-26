using System;
using System.Collections.Generic;

namespace russianRoads.Models;

public partial class Organization
{
    public int OrganId { get; set; }

    public string OrganName { get; set; } = null!;

    public virtual ICollection<OrganizationsHierarchy> OrganizationsHierarchies { get; set; } = new List<OrganizationsHierarchy>();
}
