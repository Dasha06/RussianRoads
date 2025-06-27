using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;
using russianRoads.Classes;
using russianRoads.Models;
using Avalonia.Interactivity;
using System.Threading.Tasks;

namespace russianRoads;

public partial class MainWindow : Window
{
    private List<OrganizationsHierarchy> organisations =
        HelperDB.context.OrganizationsHierarchies
            .Include(h => h.HierOrgan)
            .Include(h => h.HierSuborgan)
            .Include(h => h.HierSubsuborgan)
            .Include(h => h.Workers)
            .ThenInclude(h => h.WorkerPost)
            .ToList();
    
    public MainWindow()
    {
        InitializeComponent();
        var hierarchy = CreateHierarchy(organisations);
        DepartmentsTree.ItemsSource = hierarchy;
    }

    private List<OrganizationsNode> CreateHierarchy(List<OrganizationsHierarchy> organisations)
    {
        var root = new OrganizationsNode
        {
            Name = "Дороги России",
            Children = new List<OrganizationsNode>()
        };

        var organs = organisations
            .Where(h => h.HierOrgan != null)
            .GroupBy(h => h.HierOrgan.OrganId)
            .ToList();

        foreach (var organGroup in organs)
        {
            var organHierarchy = organGroup.FirstOrDefault();
            var organ = organHierarchy?.HierOrgan;
            if (organ == null) continue;

            var organNode = new OrganizationsNode
            {
                Name = organ.OrganName,
                OriginalData = organHierarchy,
                Children = new List<OrganizationsNode>()
            };
            var suborgans = organGroup
                .Where(h => h.HierSuborgan != null
                            && h.HierSuborgan.SuborganName != "нет подорганизации")
                .GroupBy(h => h.HierSuborgan.SuborganId)
                .ToList();

            foreach (var suborganGroup in suborgans)
            {
                var suborganHierarchy = suborganGroup.FirstOrDefault();
                var suborgan = suborganHierarchy?.HierSuborgan;
                if (suborgan == null) continue;

                if (suborgan.SuborganName == "нет подорганизации")
                    continue;

                var suborganNode = new OrganizationsNode
                {
                    Name = suborgan.SuborganName,
                    OriginalData = suborganHierarchy, 
                    Children = new List<OrganizationsNode>()
                };

                var subsuborgans = suborganGroup
                    .Where(h => h.HierSubsuborgan != null
                                && h.HierSubsuborgan.SubsuborganName != "нет подподорганизации")
                    .GroupBy(h => h.HierSubsuborgan.SubsuborganId)
                    .ToList();

                foreach (var subsuborganGroup in subsuborgans)
                {
                    var subsuborganHierarchy = subsuborganGroup.FirstOrDefault();
                    var subsuborgan = subsuborganHierarchy?.HierSubsuborgan;
                    if (subsuborgan == null) continue;

                    if (subsuborgan.SubsuborganName == "нет подподорганизации")
                        continue;

                    var subsuborganNode = new OrganizationsNode
                    {
                        Name = subsuborgan.SubsuborganName,
                        OriginalData = subsuborganHierarchy, 
                        Children = null
                    };
                    suborganNode.Children.Add(subsuborganNode);
                }

                if (suborganNode.Children.Count == 0)
                    suborganNode.Children = null;

                organNode.Children.Add(suborganNode);
            }

            if (organNode.Children.Count == 0)
                organNode.Children = null;

            root.Children.Add(organNode);
        }

        return new List<OrganizationsNode> { root };
    }
    

    private void DepartmentsTree_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        RefreshEmployeeList();
    }

    private void AddButton_Click(object? sender, RoutedEventArgs e)
    {
        var selectedNode = DepartmentsTree.SelectedItem as OrganizationsNode;
        if (selectedNode?.OriginalData == null)
        {
            ShowMessage("Выберите организацию для добавления сотрудника");
            return;
        }

        var editWindow = new EmployeeEditWindow();
        editWindow.Closed += (s, args) => RefreshEmployeeList();
        editWindow.ShowDialog(this);
    }

    private void EditButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is Worker worker)
        {
            var editWindow = new EmployeeEditWindow(worker);
            editWindow.Closed += (s, args) => RefreshEmployeeList();
            editWindow.ShowDialog(this);
        }
    }

    private void CalendarButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is Worker worker)
        {
            var calendarWindow = new EmployeeCalendarWindow(worker);
            calendarWindow.ShowDialog(this);
        }
    }

    private void ReportsButton_Click(object? sender, RoutedEventArgs e)
    {
        var reportsWindow = new ReportsWindow();
        reportsWindow.ShowDialog(this);
    }

    private async void DeleteButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is Worker worker)
        {
            var result = await ShowConfirmDialogAsync($"Вы уверены, что хотите удалить сотрудника {worker.WorkerFio}?");
            if (result)
            {
                if (EmployeeService.DeleteWorker(worker))
                {
                    Console.WriteLine("збс");
                    ShowMessage("Сотрудник успешно удален");
                    RefreshEmployeeList();
                }
                else
                {
                    Console.WriteLine("плохо");
                    ShowMessage("Ошибка при удалении сотрудника");
                }
            }
        }
    }

    private void RefreshEmployeeList()
    {
        var selectedNode = DepartmentsTree.SelectedItem as OrganizationsNode;
        if (selectedNode != null && selectedNode.OriginalData != null)
        {
            HelperDB.context.Entry(selectedNode.OriginalData).Reload();
            HelperDB.context.Entry(selectedNode.OriginalData).Collection(h => h.Workers).Load();
            
            var workers = selectedNode.OriginalData.Workers.ToList();
            EmployeeListBox.ItemsSource = workers.Select(w => new EmployeeCardViewModel(w)).ToList();
        }
        else
        {
            EmployeeListBox.ItemsSource = null;
        }
    }

    private void ShowMessage(string message)
    {
        var messageBox = new Window
        {
            Title = "Информация",
            Width = 400,
            Height = 150
        };

        var okButton = new Button
        {
            Content = "OK",
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
            Margin = new Avalonia.Thickness(0, 20, 0, 0),
            Width = 80
        };
        okButton.Click += (sender, args) => messageBox.Close();

        messageBox.Content = new StackPanel
        {
            Margin = new Avalonia.Thickness(20),
            Children =
            {
                new TextBlock { Text = message, TextWrapping = Avalonia.Media.TextWrapping.Wrap },
                okButton
            }
        };
        
        messageBox.ShowDialog(this);
    }

    private async Task<bool> ShowConfirmDialogAsync(string message)
    {
        var tcs = new TaskCompletionSource<bool>();
        
        var yesButton = new Button 
        { 
            Content = "Да", 
            Width = 80,
            Margin = new Avalonia.Thickness(0, 0, 10, 0)
        };
        
        var noButton = new Button 
        { 
            Content = "Нет", 
            Width = 80
        };
        
        var buttonPanel = new StackPanel 
        { 
            Orientation = Avalonia.Layout.Orientation.Horizontal,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
            Margin = new Avalonia.Thickness(0, 20, 0, 0),
            Children = { yesButton, noButton }
        };
        
        var messageBox = new Window
        {
            Title = "Подтверждение",
            Width = 400,
            Height = 150,
            Content = new StackPanel
            {
                Margin = new Avalonia.Thickness(20),
                Children =
                {
                    new TextBlock { Text = message, TextWrapping = Avalonia.Media.TextWrapping.Wrap },
                    buttonPanel
                }
            }
        };

        yesButton.Click += (s, e) => { tcs.TrySetResult(true); messageBox.Close(); };
        noButton.Click += (s, e) => { tcs.TrySetResult(false); messageBox.Close(); };
        
        await messageBox.ShowDialog(this);
        return await tcs.Task;
    }
}