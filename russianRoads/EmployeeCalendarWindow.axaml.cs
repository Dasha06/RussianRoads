using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using russianRoads.Classes;
using russianRoads.Models;

namespace russianRoads;

public partial class EmployeeCalendarWindow : Window
{
    private Worker? _worker;

    public EmployeeCalendarWindow()
    {
        InitializeComponent();
    }

    public EmployeeCalendarWindow(Worker worker) : this()
    {
        _worker = worker;
        
        LoadEmployeeInfo();
        LoadCalendarData();
    }

    private void LoadEmployeeInfo()
    {
        if (_worker == null) return;
        
        EmployeeNameText.Text = $"Календарь сотрудника: {_worker.WorkerFio}";
        EmployeeInfoText.Text = $"{_worker.WorkerPost?.PostName} | {_worker.WorkerEmail}";
    }

    private void LoadCalendarData()
    {
        if (_worker == null) return;
        
        var holidays = CalendarService.GetWorkerHolidays(_worker.WorkerId);
        HolidaysDataGrid.ItemsSource = holidays;

        var missedDays = CalendarService.GetWorkerMissedDays(_worker.WorkerId);
        MissedDataGrid.ItemsSource = missedDays;

        var learning = CalendarService.GetWorkerLearning(_worker.WorkerId);
        LearningDataGrid.ItemsSource = learning;
    }
} 