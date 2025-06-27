using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using russianRoads.Models;

namespace russianRoads.Classes;

public static class CalendarService
{
    public static List<CalendarWorkersHoliday> GetWorkerHolidays(int workerId)
    {
        return HelperDB.context.CalendarWorkersHolidays
            .Include(h => h.Worker)
            .Where(h => h.WorkerId == workerId)
            .OrderBy(h => h.CalenholidayDateStart)
            .ToList();
    }

    public static List<CalendarWorkersHoliday> GetAllHolidays()
    {
        return HelperDB.context.CalendarWorkersHolidays
            .Include(h => h.Worker)
            .ThenInclude(w => w.WorkerPost)
            .OrderBy(h => h.CalenholidayDateStart)
            .ToList();
    }

    public static List<CalendarMissedWorker> GetWorkerMissedDays(int workerId)
    {
        return HelperDB.context.CalendarMissedWorkers
            .Include(m => m.WorkerMissed)
            .Include(m => m.WorkerReplaced)
            .Where(m => m.WorkerMissedId == workerId || m.WorkerReplacedId == workerId)
            .OrderBy(m => m.CalenmissDate)
            .ToList();
    }

    public static List<CalendarMissedWorker> GetAllMissedDays()
    {
        return HelperDB.context.CalendarMissedWorkers
            .Include(m => m.WorkerMissed)
            .ThenInclude(w => w.WorkerPost)
            .Include(m => m.WorkerReplaced)
            .ThenInclude(w => w.WorkerPost)
            .OrderBy(m => m.CalenmissDate)
            .ToList();
    }

    public static List<CalendarLearning> GetWorkerLearning(int workerId)
    {
        return HelperDB.context.CalendarLearnings
            .Include(l => l.CalenlearnEvent)
            .Where(l => HelperDB.context.LearnAndWorkers
                .Any(lw => lw.LearnId == l.CalenlearnEventId && lw.WorkerId == workerId))
            .OrderBy(l => l.CalenlearnDateStart)
            .ToList();
    }

    public static List<CalendarLearning> GetAllLearning()
    {
        return HelperDB.context.CalendarLearnings
            .Include(l => l.CalenlearnEvent)
            .OrderBy(l => l.CalenlearnDateStart)
            .ToList();
    }
} 