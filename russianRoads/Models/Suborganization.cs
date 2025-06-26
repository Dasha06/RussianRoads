using System;
using System.Collections.Generic;

namespace russianRoads.Models;

public partial class Suborganization
{
    public int SuborganId { get; set; }

    public string? SuborganName { get; set; }

    public string? SuborganDescription { get; set; }

    public virtual ICollection<OrganizationsHierarchy> OrganizationsHierarchies { get; set; } = new List<OrganizationsHierarchy>();
}
