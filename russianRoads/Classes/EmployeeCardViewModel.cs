using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using russianRoads.Models;

namespace russianRoads.Classes;

public class EmployeeCardViewModel : INotifyPropertyChanged
{
    private readonly Worker _worker;

    public EmployeeCardViewModel(Worker worker)
    {
        _worker = worker;
    }

    public string DepartmentAndRole => _worker.WorkerPost?.PostName ?? "";
    public string FullName => _worker.WorkerFio;
    public string Phone => _worker.WorkerWorkphone;
    public string Email => _worker.WorkerEmail;
    public string Extra => _worker.WorkerPersonalphone ?? "";
    public string Cabinet => _worker.WorkerCab?.CabNumber ?? "";
    public string Birthday => _worker.WorkerBirtday?.ToString("dd.MM.yyyy") ?? "";
    
    public Worker Worker => _worker;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
} 