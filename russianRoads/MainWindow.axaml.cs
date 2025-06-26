using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;
using russianRoads.Classes;
using russianRoads.Models;
using Avalonia.Interactivity;

namespace russianRoads;

public partial class MainWindow : Window
{
    private List<OrganizationsHierarchy> organisations =
        HelperDB.context.OrganizationsHierarchies
            .Include(h => h.HierOrgan)
            .Include(h => h.HierSuborgan)
            .Include(h => h.HierSubsuborgan)
            .ToList();
    public MainWindow()
    {
        InitializeComponent();
        var hierarchy = CreateHierarchy(organisations);
        DepartmentsTree.ItemsSource = hierarchy;
    }

    private List<OrganizationsNode> CreateHierarchy(List<OrganizationsHierarchy> organisations)
    {
        // Корневой узел
        var root = new OrganizationsNode
        {
            Name = "Дороги России",
            Children = new List<OrganizationsNode>()
        };

        // Группируем по HierOrgan (Organization)
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
                OriginalData = organHierarchy, // Сохраняем ссылку на OrganizationsHierarchy
                Children = new List<OrganizationsNode>()
            };

            // Группируем по HierSuborgan (Suborganization)
            var suborgans = organGroup
                .Where(h => h.HierSuborgan != null
                            && h.HierSuborgan.SuborganName != "нет подорганизации"
                            && h.HierSuborgan.SuborganName != "Нет подорганизации"
                            && h.HierSuborgan.SuborganName != "Нет Подорганизации")
                .GroupBy(h => h.HierSuborgan.SuborganId)
                .ToList();

            foreach (var suborganGroup in suborgans)
            {
                var suborganHierarchy = suborganGroup.FirstOrDefault();
                var suborgan = suborganHierarchy?.HierSuborgan;
                if (suborgan == null) continue;

                // Пропускаем "нет подорганизации" на всякий случай
                if (suborgan.SuborganName == "нет подорганизации" ||
                    suborgan.SuborganName == "Нет подорганизации" ||
                    suborgan.SuborganName == "Нет Подорганизации")
                    continue;

                var suborganNode = new OrganizationsNode
                {
                    Name = suborgan.SuborganName,
                    OriginalData = suborganHierarchy, // Сохраняем ссылку на OrganizationsHierarchy
                    Children = new List<OrganizationsNode>()
                };

                // Группируем по HierSubsuborgan (Subsuborganization)
                var subsuborgans = suborganGroup
                    .Where(h => h.HierSubsuborgan != null
                                && h.HierSubsuborgan.SubsuborganName != "нет подподорганизации"
                                && h.HierSubsuborgan.SubsuborganName != "Нет подподорганизации"
                                && h.HierSubsuborgan.SubsuborganName != "Нет Подподорганизации")
                    .GroupBy(h => h.HierSubsuborgan.SubsuborganId)
                    .ToList();

                foreach (var subsuborganGroup in subsuborgans)
                {
                    var subsuborganHierarchy = subsuborganGroup.FirstOrDefault();
                    var subsuborgan = subsuborganHierarchy?.HierSubsuborgan;
                    if (subsuborgan == null) continue;

                    // Пропускаем "нет подподорганизации" на всякий случай
                    if (subsuborgan.SubsuborganName == "нет подподорганизации" ||
                        subsuborgan.SubsuborganName == "Нет подподорганизации" ||
                        subsuborgan.SubsuborganName == "Нет Подподорганизации")
                        continue;

                    var subsuborganNode = new OrganizationsNode
                    {
                        Name = subsuborgan.SubsuborganName,
                        OriginalData = subsuborganHierarchy, // Сохраняем ссылку на OrganizationsHierarchy
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
        var selectedNode = DepartmentsTree.SelectedItem as OrganizationsNode;
        if (selectedNode != null && selectedNode.OriginalData != null)
        {
            var workers = selectedNode.OriginalData.Workers.ToList();
            EmployeeListBox.ItemsSource = workers.Select(w => new EmployeeCardVM(w)).ToList();
        }
        else
        {
            EmployeeListBox.ItemsSource = null;
        }
    }

    // Вспомогательный класс для отображения карточки работника
    public class EmployeeCardVM
    {
        public string DepartmentAndRole { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Extra { get; set; }

        public EmployeeCardVM(Worker w)
        {
            DepartmentAndRole = w.WorkerPost?.PostName ?? "";
            FullName = w.WorkerFio;
            Phone = w.WorkerWorkphone;
            Email = w.WorkerEmail;
            Extra = w.WorkerPersonalphone ?? "";
        }
    }
}