using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Trojantrading.Models;

namespace Trojantrading.DAL
{
    public interface IExcelManager
    {
        FileInfo CreateExcelFile(string fileName);
        ExcelPackage OrdesSummarySheet(ExcelPackage pck, List<Order> orders);
    }
    public class ExcelManager: IExcelManager
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ExcelManager(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }


        public FileInfo CreateExcelFile(string fileName)
        {

            string wwwrootPath = _hostingEnvironment.WebRootPath;

            FileInfo file = new FileInfo(Path.Combine(wwwrootPath, fileName));

            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(wwwrootPath, fileName));
            }

            return file;
        }


        public ExcelPackage OrdesSummarySheet(ExcelPackage pck, List<Order> orders)
        {

            #region headers

            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("SUMMARY");
            ws.PrinterSettings.FitToWidth = 1;
            ws.PrinterSettings.FitToPage = true;
            ws.PrinterSettings.FitToHeight = 0;
            using (ExcelRange rng = ws.Cells[ws.Cells.Start.Row, ws.Cells.Start.Column, ws.Cells.End.Row, ws.Cells.End.Column])
            {
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rng.Style.Fill.BackgroundColor.SetColor(Color.White);
            }

            // Create Headers
            List<string> headerList = new List<string>();
            headerList.Add("ID");
            headerList.Add("CUSTOMER");
            headerList.Add("CREATED DATE");
            headerList.Add("AMOUNT");
            headerList.Add("STATUS");

            // Capture Number Of Columns
            // We'll Use This Later When Creating Rows
            int iColCount = headerList.Count;

            ws.Column(1).Width = 15; // A ID
            ws.Column(2).Width = 30; // B CUSTOMER
            ws.Column(3).Width = 20; // C CREATED DATE
            ws.Column(4).Width = 15; // D AMOUNT
            ws.Column(5).Width = 15; // E STATUS

            // Loop Through The Specifications
            ws.Row(1).Height = 80;


            int rowCounter = 3;
            // write table header
            char cellPosition = 'A';
            foreach (var header in headerList)
            {
                ws.Cells[cellPosition.ToString() + rowCounter].Value = header;
                cellPosition++;
            }

            cellPosition--;
            // Format Column Titles
            using (ExcelRange rng = ws.Cells["A3:" + (cellPosition).ToString() + "3"])
            {
                rng.Style.Font.Bold = true;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rng.Style.Fill.BackgroundColor.SetColor(Color.Maroon);
                rng.Style.Font.Color.SetColor(Color.White);
            }
            #endregion

            #region item details
            rowCounter++;

            foreach (var order in orders)
            {

                ws.Cells["A" + rowCounter].Value = order.Id;
                ws.Cells["B" + rowCounter].Value = order.User.BussinessName;
                ws.Cells["C" + rowCounter].Value = order.CreatedDate;
                ws.Cells["D" + rowCounter].Value = order.TotalPrice;
                ws.Cells["E" + rowCounter].Value = order.OrderStatus;

                //ws.Cells["I" + rowCounter].Value = orderTotal.ToString("C");

                rowCounter++;
            }
            //ws.Cells["H" + rowCounter].Value = "TOTAL:";
            //ws.Cells["H" + rowCounter].Style.Font.Bold = true;
            //ws.Cells["H" + rowCounter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            //ws.Cells["I" + rowCounter].Value = totalAmount.ToString("C");
            //ws.Cells["I" + rowCounter].Style.Font.Bold = true;
            //ws.Cells["I3:I" + rowCounter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            #endregion

            return pck;
        }


    }
}
