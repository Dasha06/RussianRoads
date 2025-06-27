using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using russianRoads.Classes;
using russianRoads.Models;

namespace russianRoads;

public partial class EmployeeEditWindow : Window
{
    private Worker? _worker;
    private bool _isEditMode;
    private List<Post> _posts = new();
    private List<Cabinet> _cabinets = new();
    private List<OrganizationsHierarchy> _organizations = new();

    public EmployeeEditWindow()
    {
        InitializeComponent();
        
        LoadData();
        SetupComboBoxes();
    }

    public EmployeeEditWindow(Worker? worker) : this()
    {
        _worker = worker;
        _isEditMode = worker != null;
        
        Title = _isEditMode ? "Редактирование сотрудника" : "Добавление сотрудника";
        
        if (_isEditMode && _worker != null)
        {
            LoadWorkerData();
        }
    }

    private void LoadData()
    {
        _posts = HelperDB.context.Posts.ToList();
        _cabinets = HelperDB.context.Cabinets.ToList();
        _organizations = HelperDB.context.OrganizationsHierarchies
            .Include(h => h.HierOrgan)
            .Include(h => h.HierSuborgan)
            .Include(h => h.HierSubsuborgan)
            .ToList();
    }

    private void SetupComboBoxes()
    {
        PostComboBox.ItemsSource = _posts;
        PostComboBox.DisplayMemberBinding = new Avalonia.Data.Binding("PostName");
        
        CabinetComboBox.ItemsSource = _cabinets;
        CabinetComboBox.DisplayMemberBinding = new Avalonia.Data.Binding("CabNumber");
        
        OrganizationComboBox.ItemsSource = _organizations;
        OrganizationComboBox.DisplayMemberBinding = new Avalonia.Data.Binding("DisplayName");
    }

    private void LoadWorkerData()
    {
        if (_worker == null) return;
        
        FioTextBox.Text = _worker.WorkerFio;
        WorkPhoneTextBox.Text = _worker.WorkerWorkphone;
        PersonalPhoneTextBox.Text = _worker.WorkerPersonalphone ?? "";
        EmailTextBox.Text = _worker.WorkerEmail;
        BirthdayDatePicker.SelectedDate = _worker.WorkerBirtday?.ToDateTime(TimeOnly.MinValue);
        
        PostComboBox.SelectedItem = _posts.FirstOrDefault(p => p.PostId == _worker.WorkerPostId);
        CabinetComboBox.SelectedItem = _cabinets.FirstOrDefault(c => c.CabId == _worker.WorkerCabId);
        OrganizationComboBox.SelectedItem = _organizations.FirstOrDefault(o => o.HierId == _worker.WorkerOrganId);
    }

    private void SaveButton_Click(object? sender, RoutedEventArgs e)
    {
        var fio = FioTextBox.Text?.Trim();
        var workPhone = WorkPhoneTextBox.Text?.Trim();
        var email = EmailTextBox.Text?.Trim();

        if (string.IsNullOrWhiteSpace(fio))
        {
            ShowError("ФИО обязательно для заполнения");
            return;
        }

        if (string.IsNullOrWhiteSpace(workPhone))
        {
            ShowError("Рабочий телефон обязателен для заполнения");
            return;
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            ShowError("Email обязателен для заполнения");
            return;
        }

        try
        {
            var selectedPost = PostComboBox.SelectedItem as Post;
            var selectedCabinet = CabinetComboBox.SelectedItem as Cabinet;
            var selectedOrganization = OrganizationComboBox.SelectedItem as OrganizationsHierarchy;

            if (_isEditMode && _worker != null)
            {
                _worker.WorkerFio = fio;
                _worker.WorkerPostId = selectedPost?.PostId;
                _worker.WorkerWorkphone = workPhone;
                _worker.WorkerPersonalphone = PersonalPhoneTextBox.Text?.Trim();
                _worker.WorkerEmail = email;
                _worker.WorkerBirtday = BirthdayDatePicker.SelectedDate?.Date != null ? 
                    DateOnly.FromDateTime(BirthdayDatePicker.SelectedDate.Value.Date) : null;
                _worker.WorkerCabId = selectedCabinet?.CabId;
                _worker.WorkerOrganId = selectedOrganization?.HierId;
            }
            else
            {
                var newWorker = new Worker
                {
                    WorkerId = EmployeeService.GetNextWorkerId(),
                    WorkerFio = fio,
                    WorkerPostId = selectedPost?.PostId,
                    WorkerWorkphone = workPhone,
                    WorkerPersonalphone = PersonalPhoneTextBox.Text?.Trim(),
                    WorkerEmail = email,
                    WorkerBirtday = BirthdayDatePicker.SelectedDate?.Date != null ? 
                        DateOnly.FromDateTime(BirthdayDatePicker.SelectedDate.Value.Date) : null,
                    WorkerCabId = selectedCabinet?.CabId,
                    WorkerOrganId = selectedOrganization?.HierId
                };

                HelperDB.context.Workers.Add(newWorker);
            }

            HelperDB.context.SaveChanges();
            Close();
        }
        catch (Exception ex)
        {
            ShowError($"Ошибка при сохранении: {ex.Message}");
        }
    }

    private void CancelButton_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ShowError(string message)
    {
        var messageBox = new Window
        {
            Title = "Ошибка",
            Width = 400,
            Height = 150,
            Content = new StackPanel
            {
                Margin = new Avalonia.Thickness(20),
                Children =
                {
                    new TextBlock { Text = message, TextWrapping = Avalonia.Media.TextWrapping.Wrap },
                    new Button { Content = "OK", HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right, 
                                Margin = new Avalonia.Thickness(0, 20, 0, 0), Width = 80 }
                }
            }
        };
        
        messageBox.ShowDialog(this);
    }
} 