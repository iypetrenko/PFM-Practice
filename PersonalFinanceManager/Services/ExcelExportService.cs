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
        public void ExportUserData(User user)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            var file = new FileInfo($"{user.UserName}_Data.xlsx");

            using (var package = new ExcelPackage(file))
            {
                var ws = package.Workbook.Worksheets.Add("UserData");

                // Headers
                ws.Cells[1, 1].Value = "ID";
                ws.Cells[1, 2].Value = "User Name";
                ws.Cells[1, 3].Value = "Role";

                // Data
                ws.Cells[2, 1].Value = user.Id;
                ws.Cells[2, 2].Value = user.UserName;
                ws.Cells[2, 3].Value = user.Role.ToString();

                package.Save();
            }
        }

        public void ExportUsersData(IEnumerable<User> users, string filePath)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            var file = new FileInfo(filePath);

            using (var package = new ExcelPackage(file))
            {
                var ws = package.Workbook.Worksheets.Add("UsersData");

                // Headers
                ws.Cells[1, 1].Value = "ID";
                ws.Cells[1, 2].Value = "User Name";
                ws.Cells[1, 3].Value = "Role";

                int row = 2;
                foreach (var user in users)
                {
                    ws.Cells[row, 1].Value = user.Id;
                    ws.Cells[row, 2].Value = user.UserName;
                    ws.Cells[row, 3].Value = user.Role.ToString();
                    row++;
                }

                package.Save();
            }
        }
    }
}
