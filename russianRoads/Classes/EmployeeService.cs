using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using russianRoads.Models;

namespace russianRoads.Classes;

public static class EmployeeService
{
    public static List<Worker> GetAllWorkers()
    {
        return HelperDB.context.Workers
            .Include(w => w.WorkerPost)
            .Include(w => w.WorkerCab)
            .Include(w => w.WorkerOrgan)
            .ThenInclude(o => o.HierOrgan)
            .Include(w => w.WorkerOrgan)
            .ThenInclude(o => o.HierSuborgan)
            .Include(w => w.WorkerOrgan)
            .ThenInclude(o => o.HierSubsuborgan)
            .ToList();
    }

    public static List<Worker> GetWorkersByOrganization(int organizationId)
    {
        return HelperDB.context.Workers
            .Include(w => w.WorkerPost)
            .Include(w => w.WorkerCab)
            .Include(w => w.WorkerOrgan)
            .ThenInclude(o => o.HierOrgan)
            .Include(w => w.WorkerOrgan)
            .ThenInclude(o => o.HierSuborgan)
            .Include(w => w.WorkerOrgan)
            .ThenInclude(o => o.HierSubsuborgan)
            .Where(w => w.WorkerOrganId == organizationId)
            .ToList();
    }

    public static bool DeleteWorker(Worker worker)
    {
        try
        {
            if (worker != null)
            {
                HelperDB.context.Workers.Remove(worker);
                HelperDB.context.SaveChanges();
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
    
    public static List<OrganizationsHierarchy> GetAllOrganizations()
    {
        return HelperDB.context.OrganizationsHierarchies
            .Include(h => h.HierOrgan)
            .Include(h => h.HierSuborgan)
            .Include(h => h.HierSubsuborgan)
            .ToList();
    }

    public static int GetNextWorkerId()
    {
        var maxId = HelperDB.context.Workers.Max(w => w.WorkerId);
        return maxId + 1;
    }
} 