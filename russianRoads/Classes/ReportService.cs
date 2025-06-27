using System;
using System.Collections.Generic;
using System.Linq;
using russianRoads.Models;

namespace russianRoads.Classes;

public static class ReportService
{
    public static List<WorkerReport> GetWorkersReport(int? organizationId = null)
    {
        var workers = organizationId.HasValue 
            ? EmployeeService.GetWorkersByOrganization(organizationId.Value)
            : EmployeeService.GetAllWorkers();

        var report = new List<WorkerReport>();
        
        foreach (var worker in workers)
        {
            var holidays = CalendarService.GetWorkerHolidays(worker.WorkerId);
            var missedDays = CalendarService.GetWorkerMissedDays(worker.WorkerId);
            var learning = CalendarService.GetWorkerLearning(worker.WorkerId);

            var totalHolidayDays = holidays.Sum(h => 
                h.CalenholidayDateEnd.DayNumber - h.CalenholidayDateStart.DayNumber + 1);
            
            var totalMissedDays = missedDays.Count(m => m.WorkerMissedId == worker.WorkerId);
            var totalReplacementDays = missedDays.Count(m => m.WorkerReplacedId == worker.WorkerId);

            report.Add(new WorkerReport
            {
                WorkerId = worker.WorkerId,
                WorkerName = worker.WorkerFio,
                Position = worker.WorkerPost?.PostName ?? "",
                Organization = GetOrganizationName(worker.WorkerOrgan),
                Email = worker.WorkerEmail,
                Phone = worker.WorkerWorkphone,
                TotalHolidayDays = totalHolidayDays,
                TotalMissedDays = totalMissedDays,
                TotalReplacementDays = totalReplacementDays,
                LearningEventsCount = learning.Count
            });
        }

        return report;
    }
    public static List<HolidayReport> GetHolidaysReport(DateOnly? startDate = null, DateOnly? endDate = null)
    {
        var holidays = CalendarService.GetAllHolidays();

        if (startDate.HasValue)
            holidays = holidays.Where(h => h.CalenholidayDateStart >= startDate.Value).ToList();
        
        if (endDate.HasValue)
            holidays = holidays.Where(h => h.CalenholidayDateEnd <= endDate.Value).ToList();

        var report = new List<HolidayReport>();
        
        foreach (var holiday in holidays)
        {
            var daysCount = holiday.CalenholidayDateEnd.DayNumber - holiday.CalenholidayDateStart.DayNumber + 1;
            
            report.Add(new HolidayReport
            {
                WorkerName = holiday.Worker?.WorkerFio ?? "",
                Position = holiday.Worker?.WorkerPost?.PostName ?? "",
                StartDate = holiday.CalenholidayDateStart,
                EndDate = holiday.CalenholidayDateEnd,
                DaysCount = daysCount
            });
        }

        return report;
    }

    public static List<MissedDayReport> GetMissedDaysReport(DateOnly? startDate = null, DateOnly? endDate = null)
    {
        var missedDays = CalendarService.GetAllMissedDays();

        if (startDate.HasValue)
            missedDays = missedDays.Where(m => m.CalenmissDate >= startDate.Value).ToList();
        
        if (endDate.HasValue)
            missedDays = missedDays.Where(m => m.CalenmissDate <= endDate.Value).ToList();

        var report = new List<MissedDayReport>();
        
        foreach (var missedDay in missedDays)
        {
            report.Add(new MissedDayReport
            {
                Date = missedDay.CalenmissDate,
                MissedWorkerName = missedDay.WorkerMissed?.WorkerFio ?? "",
                MissedWorkerPosition = missedDay.WorkerMissed?.WorkerPost?.PostName ?? "",
                ReplacedWorkerName = missedDay.WorkerReplaced?.WorkerFio ?? "",
                ReplacedWorkerPosition = missedDay.WorkerReplaced?.WorkerPost?.PostName ?? ""
            });
        }

        return report;
    }

    public static List<LearningReport> GetLearningReport(DateOnly? startDate = null, DateOnly? endDate = null)
    {
        var learning = CalendarService.GetAllLearning();

        if (startDate.HasValue)
            learning = learning.Where(l => l.CalenlearnDateStart >= startDate.Value).ToList();
        
        if (endDate.HasValue)
            learning = learning.Where(l => l.CalenlearnDateEnd <= endDate.Value).ToList();

        var report = new List<LearningReport>();
        
        foreach (var learn in learning)
        {
            var daysCount = learn.CalenlearnDateEnd.DayNumber - learn.CalenlearnDateStart.DayNumber + 1;
            
            report.Add(new LearningReport
            {
                EventName = learn.CalenlearnEvent?.LearneventName ?? "",
                Description = learn.CalenlearnEvent?.LearneventDescription ?? "",
                StartDate = learn.CalenlearnDateStart,
                EndDate = learn.CalenlearnDateEnd,
                DaysCount = daysCount
            });
        }

        return report;
    }

    private static string GetOrganizationName(OrganizationsHierarchy? hierarchy)
    {
        if (hierarchy == null) return "";
        
        var parts = new List<string>();
        
        if (hierarchy.HierOrgan != null)
            parts.Add(hierarchy.HierOrgan.OrganName);
        
        if (hierarchy.HierSuborgan != null)
            parts.Add(hierarchy.HierSuborgan.SuborganName);
        
        if (hierarchy.HierSubsuborgan != null)
            parts.Add(hierarchy.HierSubsuborgan.SubsuborganName);
        
        return string.Join(" / ", parts);
    }
}

public class WorkerReport
{
    public int WorkerId { get; set; }
    public string WorkerName { get; set; } = "";
    public string Position { get; set; } = "";
    public string Organization { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public int TotalHolidayDays { get; set; }
    public int TotalMissedDays { get; set; }
    public int TotalReplacementDays { get; set; }
    public int LearningEventsCount { get; set; }
}

public class HolidayReport
{
    public string WorkerName { get; set; } = "";
    public string Position { get; set; } = "";
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int DaysCount { get; set; }
}

public class MissedDayReport
{
    public DateOnly Date { get; set; }
    public string MissedWorkerName { get; set; } = "";
    public string MissedWorkerPosition { get; set; } = "";
    public string ReplacedWorkerName { get; set; } = "";
    public string ReplacedWorkerPosition { get; set; } = "";
}

public class LearningReport
{
    public string EventName { get; set; } = "";
    public string Description { get; set; } = "";
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int DaysCount { get; set; }
}