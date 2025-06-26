using System;
using System.Collections.Generic;

namespace russianRoads.Models;

/// <summary>
/// Список дней исключений в производственном календаре
/// </summary>
public partial class Workingcalendar
{
    public int Id { get; set; }

    /// <summary>
    /// День-исключение
    /// </summary>
    public DateOnly Exceptiondate { get; set; }

    /// <summary>
    /// 0 - будний день, но законодательно принят выходным; 1 - сб или вс, но является рабочим
    /// </summary>
    public bool Isworkingday { get; set; }
}
