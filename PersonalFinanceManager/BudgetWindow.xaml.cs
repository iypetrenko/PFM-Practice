using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PersonalFinanceManager.Consts;
using PersonalFinanceManager.Model;
using PersonalFinanceManager.Repository;
using PersonalFinanceManager.Repository.Interface;
using Microsoft.Win32;
using OfficeOpenXml;

namespace PersonalFinanceManager
{


    public partial class BudgetWindow : Window
    {
        private void NavigateToMainPage()
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToMainPage();
        }

        public Brush PickBrush()
        {
            var r = new Random();
            var properties = typeof(Brushes).GetProperties();
            var count = properties.Count();
            var colour = properties
            .Select(x => new { Property = x, Index = r.Next(count) })
            .OrderBy(x => x.Index)
            .First();
            return (SolidColorBrush)colour.Property.GetValue(colour, null);
        }

        private List<SpendingCategories> GetUserExpenses()
        {
            var expenseCategories = _categoryList.GetExpenseCategoriesList();
            var items = _itemList.GetAllItems();
            List<SpendingCategories> output = new List<SpendingCategories>();

            foreach(var cat in expenseCategories)
            {
                if (cat.UserId == SessionInfo.UserId)
                {
                    var aux = new SpendingCategories();
                    aux.Name = cat.Name;
                    aux.MonthlyBudget = cat.MonthlyBudget;

                    foreach(var item in items)
                    {
                        if (item.ToDoListId == cat.Id)
                            aux.Expenses += item.Price;
                    }

                    output.Add(aux);
                }
            }

            return output;
        }
        private async Task SaveExcelFile(List<SpendingCategories> myCategories, FileInfo file)
        {
            if(file.Exists)
            {
                file.Delete();
            }

            using (var package = new ExcelPackage(file))
            {
                var ws = package.Workbook.Worksheets.Add("BudgetReport");

                var range = ws.Cells["A1"].LoadFromCollection(myCategories, true);
                range.AutoFitColumns();

                await package.SaveAsync();
            }
        }

        private async void Export_ClickAsync(object sender, RoutedEventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var myCategories = GetUserExpenses();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var file = new FileInfo(path + "\\BudgetData.xlsx");

            await SaveExcelFile(myCategories, file);
        }


        private readonly IItemRepository _itemList = new ItemRepository();
        private readonly IExpenseCategoryRepository _categoryList = new ExpenseCategoryRepository();
        private List<Category> Categories { get; set; }

        public BudgetWindow()
        {
            InitializeComponent();

            float pieWidth = 650, pieHeight = 650, centerX = pieWidth / 2, centerY = pieHeight / 2, radius = pieWidth / 2;
            mainCanvas.Width = pieWidth;
            mainCanvas.Height = pieHeight;

            var expenseCategories = _categoryList.GetExpenseCategoriesList();
            var items = _itemList.GetAllItems();
            Categories = new List<Category>();

            decimal grandTotalSpent = 0;
            decimal budgetLimit = 0;

            foreach (var expenseCategory in expenseCategories)
            {
                budgetLimit += expenseCategory.MonthlyBudget;
            }

            BudgetText.DataContext = new Limits() { Limit = "Виділений бюджет: " + budgetLimit };

            foreach (var item in items)
            {
                grandTotalSpent += item.Price;
            }

            SpentText.DataContext = new Limits() { Limit = "Загальні витрати: " + grandTotalSpent };

            foreach (var expenseCategory in expenseCategories)
            {
                var auxItems = _itemList.GetItems(expenseCategory.Id);

                decimal categoryTotal = 0;

                foreach(var item in auxItems)
                {
                    categoryTotal += item.Price;
                }

                Category aux = new Category();
                aux.Title = expenseCategory.Name;
                aux.Percentage = (float)(categoryTotal / grandTotalSpent) * 100;
                aux.ColorBrush = PickBrush();

                Categories.Add(aux);

            }

            detailsItemsControl.ItemsSource = Categories;

            float angle = 0, prevAngle = 0;
            foreach (var category in Categories)
            {
                double line1X = (radius * Math.Cos(angle * Math.PI / 180)) + centerX;
                double line1Y = (radius * Math.Sin(angle * Math.PI / 180)) + centerY;

                angle = category.Percentage * (float)360 / 100 + prevAngle;
                Debug.WriteLine(angle);

                double arcX = (radius * Math.Cos(angle * Math.PI / 180)) + centerX;
                double arcY = (radius * Math.Sin(angle * Math.PI / 180)) + centerY;

                var line1Segment = new LineSegment(new Point(line1X, line1Y), false);
                double arcWidth = radius, arcHeight = radius;
                bool isLargeArc = category.Percentage > 50;
                var arcSegment = new ArcSegment()
                {
                    Size = new Size(arcWidth, arcHeight),
                    Point = new Point(arcX, arcY),
                    SweepDirection = SweepDirection.Clockwise,
                    IsLargeArc = isLargeArc,
                };
                var line2Segment = new LineSegment(new Point(centerX, centerY), false);

                var pathFigure = new PathFigure(
                    new Point(centerX, centerY),
                    new List<PathSegment>()
                    {
                        line1Segment,
                        arcSegment,
                        line2Segment,
                    },
                    true);

                var pathFigures = new List<PathFigure>() { pathFigure, };
                var pathGeometry = new PathGeometry(pathFigures);
                var path = new System.Windows.Shapes.Path()
                {
                    Fill = category.ColorBrush,
                    Data = pathGeometry,
                };
                mainCanvas.Children.Add(path);

                prevAngle = angle;

                var outline1 = new Line()
                {
                    X1 = centerX,
                    Y1 = centerY,
                    X2 = line1Segment.Point.X,
                    Y2 = line1Segment.Point.Y,
                    Stroke = Brushes.White,
                    StrokeThickness = 5,
                };
                var outline2 = new Line()
                {
                    X1 = centerX,
                    Y1 = centerY,
                    X2 = arcSegment.Point.X,
                    Y2 = arcSegment.Point.Y,
                    Stroke = Brushes.White,
                    StrokeThickness = 5,
                };

                mainCanvas.Children.Add(outline1);
                mainCanvas.Children.Add(outline2);
            }
        }
    }
    public class Category
    {
        public float Percentage { get; set; }
        public string Title { get; set; }
        public Brush ColorBrush { get; set; }
    }
    public class Limits
    {
        public string Limit { get; set; }
    }
    public class SpendingCategories
    {
        public string Name { get; set; }
        public decimal MonthlyBudget { get; set; }
        public decimal Expenses { get; set; }
    }
}
