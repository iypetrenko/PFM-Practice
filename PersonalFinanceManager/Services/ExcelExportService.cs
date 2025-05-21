using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using PersonalFinanceManager.Model;

namespace PersonalFinanceManager.Services
{
    public class ExcelExportService
    {
        private readonly PersonalFinanceManagerContext _context;

        public ExcelExportService(PersonalFinanceManagerContext context = null)
        {
            _context = context ?? new PersonalFinanceManagerContext();
        }

        public void ExportUserData(User user)
        {
            ExcelPackage.License.SetNonCommercialOrganization("My Noncommercial organization");

            // Получение пути к рабочему столу
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var file = new FileInfo(Path.Combine(desktopPath, $"{user.UserName}_Data.xlsx"));

            using (var package = new ExcelPackage(file))
            {
                // Создаем лист с информацией о пользователе
                var userInfoSheet = package.Workbook.Worksheets.Add("Информация о пользователе");

                // Headers
                userInfoSheet.Cells[1, 1].Value = "ID";
                userInfoSheet.Cells[1, 2].Value = "Имя пользователя";
                userInfoSheet.Cells[1, 3].Value = "Роль";

                // Data
                userInfoSheet.Cells[2, 1].Value = user.Id;
                userInfoSheet.Cells[2, 2].Value = user.UserName;
                userInfoSheet.Cells[2, 3].Value = user.Role.ToString();

                // Форматирование заголовков
                using (var range = userInfoSheet.Cells[1, 1, 1, 3])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Автоподбор ширины столбцов
                userInfoSheet.Cells.AutoFitColumns();

                // Получаем данные о категориях расходов пользователя
                var expenseCategories = _context.ExpenseCategories
                    .Where(ec => ec.UserId == user.Id)
                    .ToList();

                // Создаем лист для категорий бюджета
                var categoriesSheet = package.Workbook.Worksheets.Add("Категории бюджета");

                // Headers для категорий
                categoriesSheet.Cells[1, 1].Value = "ID";
                categoriesSheet.Cells[1, 2].Value = "Название категории";
                categoriesSheet.Cells[1, 3].Value = "Месячный бюджет";
                categoriesSheet.Cells[1, 4].Value = "Фактические траты";
                categoriesSheet.Cells[1, 5].Value = "Остаток";

                // Форматирование заголовков
                using (var range = categoriesSheet.Cells[1, 1, 1, 5])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Заполняем данные о категориях
                int row = 2;
                decimal totalBudget = 0;
                decimal totalExpenses = 0;

                foreach (var category in expenseCategories)
                {
                    // Получаем траты по данной категории
                    var items = _context.Items
                        .Where(i => i.ToDoListId == category.Id)
                        .ToList();

                    decimal categoryExpenses = items.Sum(i => i.Price);
                    decimal balance = category.MonthlyBudget - categoryExpenses;

                    // Данные о категории
                    categoriesSheet.Cells[row, 1].Value = category.Id;
                    categoriesSheet.Cells[row, 2].Value = category.Name;
                    categoriesSheet.Cells[row, 3].Value = category.MonthlyBudget;
                    categoriesSheet.Cells[row, 4].Value = categoryExpenses;
                    categoriesSheet.Cells[row, 5].Value = balance;

                    // Форматирование денежных значений
                    categoriesSheet.Cells[row, 3].Style.Numberformat.Format = "#,##0.00 ₽";
                    categoriesSheet.Cells[row, 4].Style.Numberformat.Format = "#,##0.00 ₽";
                    categoriesSheet.Cells[row, 5].Style.Numberformat.Format = "#,##0.00 ₽";

                    // Цветовой индикатор для остатка
                    if (balance < 0)
                    {
                        categoriesSheet.Cells[row, 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        categoriesSheet.Cells[row, 5].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightPink);
                    }
                    else if (balance < category.MonthlyBudget * 0.2m) // остаток менее 20% от бюджета
                    {
                        categoriesSheet.Cells[row, 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        categoriesSheet.Cells[row, 5].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);
                    }

                    totalBudget += category.MonthlyBudget;
                    totalExpenses += categoryExpenses;

                    row++;
                }

                // Итоговая строка
                categoriesSheet.Cells[row, 1, row, 2].Merge = true;
                categoriesSheet.Cells[row, 1].Value = "ИТОГО:";
                categoriesSheet.Cells[row, 1].Style.Font.Bold = true;
                categoriesSheet.Cells[row, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                categoriesSheet.Cells[row, 3].Value = totalBudget;
                categoriesSheet.Cells[row, 4].Value = totalExpenses;
                categoriesSheet.Cells[row, 5].Value = totalBudget - totalExpenses;

                // Форматирование итоговой строки
                categoriesSheet.Cells[row, 3].Style.Numberformat.Format = "#,##0.00 ₽";
                categoriesSheet.Cells[row, 4].Style.Numberformat.Format = "#,##0.00 ₽";
                categoriesSheet.Cells[row, 5].Style.Numberformat.Format = "#,##0.00 ₽";
                categoriesSheet.Cells[row, 3].Style.Font.Bold = true;
                categoriesSheet.Cells[row, 4].Style.Font.Bold = true;
                categoriesSheet.Cells[row, 5].Style.Font.Bold = true;

                // Автоподбор ширины столбцов
                categoriesSheet.Cells.AutoFitColumns();

                // Получаем все траты пользователя
                var allItems = new List<Item>();
                foreach (var category in expenseCategories)
                {
                    var items = _context.Items
                        .Where(i => i.ToDoListId == category.Id)
                        .ToList();

                    allItems.AddRange(items);
                }

                // Создаем лист для подробной информации о тратах
                var expensesSheet = package.Workbook.Worksheets.Add("Траты");

                // Headers для трат
                expensesSheet.Cells[1, 1].Value = "ID";
                expensesSheet.Cells[1, 2].Value = "Название";
                expensesSheet.Cells[1, 3].Value = "Описание";
                expensesSheet.Cells[1, 4].Value = "Категория";
                expensesSheet.Cells[1, 5].Value = "Дата покупки";
                expensesSheet.Cells[1, 6].Value = "Стоимость";

                // Форматирование заголовков
                using (var range = expensesSheet.Cells[1, 1, 1, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Заполняем данные о тратах
                row = 2;
                if (allItems.Any())
                {
                    // Сортируем траты по дате (от новых к старым)
                    allItems = allItems.OrderByDescending(i => i.BuyDate).ToList();

                    foreach (var item in allItems)
                    {
                        expensesSheet.Cells[row, 1].Value = item.Id;
                        expensesSheet.Cells[row, 2].Value = item.Name;
                        expensesSheet.Cells[row, 3].Value = item.Description;

                        // Находим название категории
                        var category = expenseCategories.FirstOrDefault(c => c.Id == item.ToDoListId);
                        expensesSheet.Cells[row, 4].Value = category?.Name ?? "Неизвестная категория";

                        expensesSheet.Cells[row, 5].Value = item.BuyDate;
                        expensesSheet.Cells[row, 5].Style.Numberformat.Format = "dd.MM.yyyy";

                        expensesSheet.Cells[row, 6].Value = item.Price;
                        expensesSheet.Cells[row, 6].Style.Numberformat.Format = "#,##0.00 ₽";

                        row++;
                    }

                    // Добавляем итоговую строку
                    expensesSheet.Cells[row, 1, row, 5].Merge = true;
                    expensesSheet.Cells[row, 1].Value = "ИТОГО:";
                    expensesSheet.Cells[row, 1].Style.Font.Bold = true;
                    expensesSheet.Cells[row, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                    expensesSheet.Cells[row, 6].Formula = $"SUM(F2:F{row - 1})";
                    expensesSheet.Cells[row, 6].Style.Numberformat.Format = "#,##0.00 ₽";
                    expensesSheet.Cells[row, 6].Style.Font.Bold = true;
                }
                else
                {
                    // Если нет трат, выводим сообщение
                    expensesSheet.Cells[row, 1, row, 6].Merge = true;
                    expensesSheet.Cells[row, 1].Value = "У пользователя нет зарегистрированных трат";
                    expensesSheet.Cells[row, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                // Автоподбор ширины столбцов
                expensesSheet.Cells.AutoFitColumns();

                package.Save();

                // Вывод информации о месте сохранения файла
                Console.WriteLine($"Файл сохранен на рабочий стол: {file.FullName}");
            }
        }

        public void ExportUsersData(IEnumerable<User> users, string fileName = null)
        {
            ExcelPackage.License.SetNonCommercialOrganization("My Noncommercial organization");

            // Получение пути к рабочему столу
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Если имя файла не указано, используем стандартное имя
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "UsersData.xlsx";
            }

            var file = new FileInfo(Path.Combine(desktopPath, fileName));

            using (var package = new ExcelPackage(file))
            {
                // Создаем лист с информацией о пользователях
                var usersSheet = package.Workbook.Worksheets.Add("Пользователи");

                // Headers
                usersSheet.Cells[1, 1].Value = "ID";
                usersSheet.Cells[1, 2].Value = "Имя пользователя";
                usersSheet.Cells[1, 3].Value = "Роль";
                usersSheet.Cells[1, 4].Value = "Кол-во категорий";
                usersSheet.Cells[1, 5].Value = "Кол-во трат";
                usersSheet.Cells[1, 6].Value = "Общая сумма трат";
                usersSheet.Cells[1, 7].Value = "Общий бюджет";

                // Форматирование заголовков
                using (var range = usersSheet.Cells[1, 1, 1, 7])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                int row = 2;
                foreach (var user in users)
                {
                    usersSheet.Cells[row, 1].Value = user.Id;
                    usersSheet.Cells[row, 2].Value = user.UserName;
                    usersSheet.Cells[row, 3].Value = user.Role.ToString();

                    // Получаем категории пользователя
                    var categories = _context.ExpenseCategories
                        .Where(ec => ec.UserId == user.Id)
                        .ToList();

                    usersSheet.Cells[row, 4].Value = categories.Count;

                    // Получаем все траты пользователя через категории
                    var userItemsCount = 0;
                    decimal totalExpenses = 0;
                    decimal totalBudget = 0;

                    foreach (var category in categories)
                    {
                        var items = _context.Items
                            .Where(i => i.ToDoListId == category.Id)
                            .ToList();

                        userItemsCount += items.Count;
                        totalExpenses += items.Sum(i => i.Price);
                        totalBudget += category.MonthlyBudget;
                    }

                    usersSheet.Cells[row, 5].Value = userItemsCount;
                    usersSheet.Cells[row, 6].Value = totalExpenses;
                    usersSheet.Cells[row, 7].Value = totalBudget;

                    // Форматирование денежных значений
                    usersSheet.Cells[row, 6].Style.Numberformat.Format = "#,##0.00 ₽";
                    usersSheet.Cells[row, 7].Style.Numberformat.Format = "#,##0.00 ₽";

                    row++;
                }

                // Итоговая строка
                usersSheet.Cells[row, 1, row, 4].Merge = true;
                usersSheet.Cells[row, 1].Value = "ИТОГО:";
                usersSheet.Cells[row, 1].Style.Font.Bold = true;
                usersSheet.Cells[row, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                usersSheet.Cells[row, 5].Formula = $"SUM(E2:E{row - 1})";
                usersSheet.Cells[row, 6].Formula = $"SUM(F2:F{row - 1})";
                usersSheet.Cells[row, 7].Formula = $"SUM(G2:G{row - 1})";

                usersSheet.Cells[row, 5].Style.Font.Bold = true;
                usersSheet.Cells[row, 6].Style.Font.Bold = true;
                usersSheet.Cells[row, 6].Style.Numberformat.Format = "#,##0.00 ₽";
                usersSheet.Cells[row, 7].Style.Font.Bold = true;
                usersSheet.Cells[row, 7].Style.Numberformat.Format = "#,##0.00 ₽";

                // Автоподбор ширины столбцов
                usersSheet.Cells.AutoFitColumns();

                // Создаем вкладку со сводной информацией по всем пользователям
                AddSummarySheet(package, users);

                package.Save();

                // Вывод информации о месте сохранения файла
                Console.WriteLine($"Файл сохранен на рабочий стол: {file.FullName}");
            }
        }

        private void AddSummarySheet(ExcelPackage package, IEnumerable<User> users)
        {
            var summarySheet = package.Workbook.Worksheets.Add("Сводная информация");

            // Заголовок листа
            summarySheet.Cells[1, 1].Value = "Сводная информация по всем пользователям";
            summarySheet.Cells[1, 1, 1, 2].Merge = true;
            summarySheet.Cells[1, 1].Style.Font.Bold = true;
            summarySheet.Cells[1, 1].Style.Font.Size = 14;

            // Общая статистика
            summarySheet.Cells[3, 1].Value = "Общее количество пользователей:";
            summarySheet.Cells[3, 2].Value = users.Count();

            int totalCategories = 0;
            int totalItems = 0;
            decimal totalBudget = 0;
            decimal totalExpenses = 0;

            foreach (var user in users)
            {
                var categories = _context.ExpenseCategories
                    .Where(ec => ec.UserId == user.Id)
                    .ToList();

                totalCategories += categories.Count;
                totalBudget += categories.Sum(c => c.MonthlyBudget);

                foreach (var category in categories)
                {
                    var items = _context.Items
                        .Where(i => i.ToDoListId == category.Id)
                        .ToList();

                    totalItems += items.Count;
                    totalExpenses += items.Sum(i => i.Price);
                }
            }

            summarySheet.Cells[4, 1].Value = "Общее количество категорий:";
            summarySheet.Cells[4, 2].Value = totalCategories;

            summarySheet.Cells[5, 1].Value = "Общее количество записей о тратах:";
            summarySheet.Cells[5, 2].Value = totalItems;

            summarySheet.Cells[6, 1].Value = "Общий бюджет всех пользователей:";
            summarySheet.Cells[6, 2].Value = totalBudget;
            summarySheet.Cells[6, 2].Style.Numberformat.Format = "#,##0.00 ₽";

            summarySheet.Cells[7, 1].Value = "Общая сумма трат всех пользователей:";
            summarySheet.Cells[7, 2].Value = totalExpenses;
            summarySheet.Cells[7, 2].Style.Numberformat.Format = "#,##0.00 ₽";

            summarySheet.Cells[8, 1].Value = "Процент использования бюджета:";
            if (totalBudget > 0)
            {
                summarySheet.Cells[8, 2].Value = (totalExpenses / totalBudget) * 100;
                summarySheet.Cells[8, 2].Style.Numberformat.Format = "0.00%";
            }
            else
            {
                summarySheet.Cells[8, 2].Value = "Н/Д";
            }

            // Автоподбор ширины столбцов
            summarySheet.Cells.AutoFitColumns();

            // Выделение ячеек с суммарной информацией
            using (var range = summarySheet.Cells[3, 1, 8, 1])
            {
                range.Style.Font.Bold = true;
            }
        }
    }
}