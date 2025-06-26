using System.Collections.Generic;
using russianRoads.Models;

namespace russianRoads.Classes;

public class OrganizationsNode
{
    public string Name { get; set; }
    public List<OrganizationsNode> Children { get; set; } = new List<OrganizationsNode>();
    public OrganizationsHierarchy OriginalData { get; set; } // Для доступа к исходным данным
    
    
}