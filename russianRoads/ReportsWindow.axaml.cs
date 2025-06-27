using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Controls.Primitives;
using russianRoads.Classes;
using russianRoads.Models;

namespace russianRoads;

public partial class ReportsWindow : Window
{
    private List<OrganizationsHierarchy> _organizations = new();
    private string _currentReportType = string.Empty;

    public ReportsWindow()
    {
        InitializeComponent();
        
        LoadData();
        SetupControls();
    }

    private void LoadData()
    {
        _organizations = EmployeeService.GetAllOrganizations();
    }

    private void SetupControls()
    {
        var now = DateOnly.FromDateTime(DateTime.Now);
        StartDatePicker.SelectedDate = new DateTime(now.Year, 1, 1);
        EndDatePicker.SelectedDate = new DateTime(now.Year, 12, 31);
        
        OrganizationComboBox.ItemsSource = _organizations;
        OrganizationComboBox.DisplayMemberBinding = new Avalonia.Data.Binding("DisplayName");
    }

    private void WorkersReportButton_Click(object? sender, RoutedEventArgs e)
    {
        _currentReportType = "Workers";
        ApplyFiltersButton_Click(sender, e);
    }

    private void HolidaysReportButton_Click(object? sender, RoutedEventArgs e)
    {
        _currentReportType = "Holidays";
        ApplyFiltersButton_Click(sender, e);
    }

    private void MissedReportButton_Click(object? sender, RoutedEventArgs e)
    {
        _currentReportType = "Missed";
        ApplyFiltersButton_Click(sender, e);
    }

    private void LearningReportButton_Click(object? sender, RoutedEventArgs e)
    {
        _currentReportType = "Learning";
        ApplyFiltersButton_Click(sender, e);
    }

    private void ApplyFiltersButton_Click(object? sender, RoutedEventArgs e)
    {
        DateOnly? startDate = null;
        DateOnly? endDate = null;
        
        if (StartDatePicker.SelectedDate?.Date != null)
            startDate = DateOnly.FromDateTime(StartDatePicker.SelectedDate.Value.Date);
        if (EndDatePicker.SelectedDate?.Date != null)
            endDate = DateOnly.FromDateTime(EndDatePicker.SelectedDate.Value.Date);
        
        var selectedOrganization = OrganizationComboBox.SelectedItem as OrganizationsHierarchy;
        var organizationId = selectedOrganization?.HierId;
        
        if (_currentReportType == "Workers")
        {
            var report = ReportService.GetWorkersReport(organizationId);
            SetupWorkersReportColumns();
            ReportDataGrid.ItemsSource = report;
        }
        else if (_currentReportType == "Holidays")
        {
            var report = ReportService.GetHolidaysReport(startDate, endDate);
            SetupHolidaysReportColumns();
            ReportDataGrid.ItemsSource = report;
        }
        else if (_currentReportType == "Missed")
        {
            var report = ReportService.GetMissedDaysReport(startDate, endDate);
            SetupMissedReportColumns();
            ReportDataGrid.ItemsSource = report;
        }
        else if (_currentReportType == "Learning")
        {
            var report = ReportService.GetLearningReport(startDate, endDate);
            SetupLearningReportColumns();
            ReportDataGrid.ItemsSource = report;
        }
    }

    private void SetupWorkersReportColumns()
    {
        ReportDataGrid.Columns.Clear();
        
        ReportDataGrid.Columns.Add(new DataGridTextColumn 
        { 
            Header = "ФИО", 
            Binding = new Avalonia.Data.Binding("WorkerName") 
        });
        ReportDataGrid.Columns.Add(new DataGridTextColumn 
        { 
            Header = "Должность", 
            Binding = new Avalonia.Data.Binding("Position") 
        });
        ReportDataGrid.Columns.Add(new DataGridTextColumn 
        { 
            Header = "Организация", 
            Binding = new Avalonia.Data.Binding("Organization") 
        });
        ReportDataGrid.Columns.Add(new DataGridTextColumn 
        { 
            Header = "Email", 
            Binding = new Avalonia.Data.Binding("Email") 
        });
        ReportDataGrid.Columns.Add(new DataGridTextColumn 
        { 
            Header = "Телефон", 
            Binding = new Avalonia.Data.Binding("Phone") 
        });
        ReportDataGrid.Columns.Add(new DataGridTextColumn 
        { 
            Header = "Дней отпуска", 
            Binding = new Avalonia.Data.Binding("TotalHolidayDays") 
        });
        ReportDataGrid.Columns.Add(new DataGridTextColumn 
        { 
            Header = "Пропусков", 
            Binding = new Avalonia.Data.Binding("TotalMissedDays") 
        });
        ReportDataGrid.Columns.Add(new DataGridTextColumn 
        { 
            Header = "Замен", 
            Binding = new Avalonia.Data.Binding("TotalReplacementDays") 
        });
        ReportDataGrid.Columns.Add(new DataGridTextColumn 
        { 
            Header = "Обучений", 
            Binding = new Avalonia.Data.Binding("LearningEventsCount") 
        });
    }

    private void SetupHolidaysReportColumns()
    {
        ReportDataGrid.Columns.Clear();
        
        ReportDataGrid.Columns.Add(new DataGridTextColumn 
        { 
            Header = "Сотрудник", 
            Binding = new Avalonia.Data.Binding("WorkerName") 
        });
        ReportDataGrid.Columns.Add(new DataGridTextColumn 
        { 
            Header = "Должность", 
            Binding = new Avalonia.Data.Binding("Position") 
        });
        ReportDataGrid.Columns.Add(new DataGridTextColumn 
        { 
            Header = "Начало", 
            Binding = new Avalonia.Data.Binding("StartDate") { StringFormat = "dd.MM.yyyy" } 
        });
        ReportDataGrid.Columns.Add(new DataGridTextColumn 
        { 
            Header = "Конец", 
            Binding = new Avalonia.Data.Binding("EndDate") { StringFormat = "dd.MM.yyyy" } 
        });
        ReportDataGrid.Columns.Add(new DataGridTextColumn 
        { 
            Header = "Дней", 
            Binding = new Avalonia.Data.Binding("DaysCount") 
        });
    }

    private void SetupMissedReportColumns()
    {
        ReportDataGrid.Columns.Clear();
        
        ReportDataGrid.Columns.Add(new DataGridTextColumn 
        { 
            Header = "Дата", 
            Binding = new Avalonia.Data.Binding("Date") { StringFormat = "dd.MM.yyyy" } 
        });
        ReportDataGrid.Columns.Add(new DataGridTextColumn 
        { 
            Header = "Отсутствовал", 
            Binding = new Avalonia.Data.Binding("MissedWorkerName") 
        });
        ReportDataGrid.Columns.Add(new DataGridTextColumn 
        { 
            Header = "Должность отсутствующего", 
            Binding = new Avalonia.Data.Binding("MissedWorkerPosition") 
        });
        ReportDataGrid.Columns.Add(new DataGridTextColumn 
        { 
            Header = "Заменял", 
            Binding = new Avalonia.Data.Binding("ReplacedWorkerName") 
        });
        ReportDataGrid.Columns.Add(new DataGridTextColumn 
        { 
            Header = "Должность заменяющего", 
            Binding = new Avalonia.Data.Binding("ReplacedWorkerPosition") 
        });
    }

    private void SetupLearningReportColumns()
    {
        ReportDataGrid.Columns.Clear();
        
        ReportDataGrid.Columns.Add(new DataGridTextColumn 
        { 
            Header = "Событие", 
            Binding = new Avalonia.Data.Binding("EventName") 
        });
        ReportDataGrid.Columns.Add(new DataGridTextColumn 
        { 
            Header = "Описание", 
            Binding = new Avalonia.Data.Binding("Description") 
        });
        ReportDataGrid.Columns.Add(new DataGridTextColumn 
        { 
            Header = "Начало", 
            Binding = new Avalonia.Data.Binding("StartDate") { StringFormat = "dd.MM.yyyy" } 
        });
        ReportDataGrid.Columns.Add(new DataGridTextColumn 
        { 
            Header = "Конец", 
            Binding = new Avalonia.Data.Binding("EndDate") { StringFormat = "dd.MM.yyyy" } 
        });
        ReportDataGrid.Columns.Add(new DataGridTextColumn 
        { 
            Header = "Дней", 
            Binding = new Avalonia.Data.Binding("DaysCount") 
        });
    }
} 