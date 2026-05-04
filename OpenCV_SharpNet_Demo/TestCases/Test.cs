using ClosedXML.Excel;
using OpenCV_SharpNet_Ver_2_CameraIntegration.Models.GS1_QC;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCV_SharpNet.TestCases
{
    public static class Test
    {
        public static Dictionary<string, double> Dict_ProcessingTime = new();
        public static Dictionary<string, double> Dict_RejectedImage = new();
        public static Dictionary<string, double> Dict_TakenImage = new();
        public static string OutPutExcelPath = @"D:\py\TimeTakenReport";

        static double AvgThrTime = 2;
        static double ThrTime = 4;
        static double AvgTime = double.NegativeZero;
        static double MaxTime = double.NegativeZero;

        public static void AddProcessingTime(string name, double time)
        {
            //create unique key for the dict
            Guid guid = Guid.NewGuid();
            name = guid.ToString() + "_" + name;

            if (Dict_ProcessingTime.ContainsKey(name))
                Dict_ProcessingTime[name] = time;
            else
                Dict_ProcessingTime.Add(name, time);
        }

        public static void RejectAndTakenImage()
        {
            var getMinTIme = Dict_ProcessingTime.Min(x => x.Value);

            MaxTime = getMinTIme + ThrTime;
            AvgTime = getMinTIme + AvgThrTime;

            Dict_RejectedImage = Dict_ProcessingTime.Where(P => P.Value > MaxTime).ToDictionary(P => P.Key, P => P.Value);

            //Dict_TakenImage = Dict_ProcessingTime.Where(P => P.Value <= AvgTime).ToDictionary(P => P.Key, P => P.Value);

            Dict_TakenImage = Dict_ProcessingTime.Where(P => P.Value <= MaxTime).ToDictionary(P => P.Key, P => P.Value);
        }

        private static void GenerateExel(string StrFileName,Dictionary<string, double> dict = null)
        {
            // Create workbook and worksheet
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Data");

                // Add headers
                worksheet.Cell(1, 1).Value = "ImageName";
                worksheet.Cell(1, 2).Value = "TimeTaken";

                // Fill rows
                int row = 2;
                foreach (var kvp in dict)
                {
                    worksheet.Cell(row, 1).Value = kvp.Key;
                    worksheet.Cell(row, 2).Value = kvp.Value;
                    row++;
                }

                // Save to file
                workbook.SaveAs(Path.Combine(OutPutExcelPath, StrFileName));
            }
        }
        public static void GenerateExel()
        {
            //genrate excel path
            if(!Directory.Exists(OutPutExcelPath))
                Directory.CreateDirectory(OutPutExcelPath);

            GenerateExel($"TotalImageList_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx",Dict_ProcessingTime);
            GenerateExel($"RejectedImageList_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx",Dict_RejectedImage);
            GenerateExel($"TakenImageList_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx",Dict_TakenImage);


            MessageBox.Show($"Excel files generated.{Environment.NewLine}Path: {OutPutExcelPath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void ExportGridViewToExcel(DataGridView gridView, string filePath)
        {
            // Convert GridView to DataTable
            DataTable dt = new DataTable();

            // Add columns
            foreach (DataGridViewColumn headerCell in gridView.Columns)
            {
                dt.Columns.Add(headerCell.HeaderText);
            }

            // Add rows
            foreach (DataGridViewRow row in gridView.Rows)
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    dr[i] = row.Cells[i].Value;
                }
                dt.Rows.Add(dr);
            }

            // Export DataTable to Excel using ClosedXML
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "GridViewData");
                wb.SaveAs(filePath);
            }

            MessageBox.Show($"Excel files generated.{Environment.NewLine}Path: {filePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //public static void ExportCombined(DataGridView gridView, List<GS1_QC_CheckResult> results, string filePath)
        //{
        //    DataTable dt = new DataTable("CombinedData");

        //    // --- Add GridView columns ---
        //    foreach (DataGridViewColumn col in gridView.Columns)
        //    {
        //        dt.Columns.Add(col.HeaderText);
        //    }

        //    // --- Add QC columns (if not already present) ---
        //    string[] qcCols = { "DecodeQC", "SC", "AN", "GN", "MOD", "FPD", "UEC", "OverAll" };
        //    foreach (var qcCol in qcCols)
        //    {
        //        if (!dt.Columns.Contains(qcCol))
        //            dt.Columns.Add(qcCol);
        //    }

        //    // --- Add GridView rows ---
        //    foreach (DataGridViewRow row in gridView.Rows)
        //    {
        //        if (!row.IsNewRow)
        //        {
        //            var dr = dt.NewRow();
        //            for (int i = 0; i < gridView.Columns.Count; i++)
        //            {
        //                dr[i] = row.Cells[i].Value?.ToString() ?? string.Empty;
        //            }
        //            dt.Rows.Add(dr);
        //        }
        //    }

        //    // --- Add QC rows ---
        //    foreach (var r in results)
        //    {
        //        var dr = dt.NewRow();
        //        dr["DecodeQC"] = r.Decode;
        //        dr["SC"] = r.SC;
        //        dr["AN"] = r.AN;
        //        dr["GN"] = r.GN;
        //        dr["MOD"] = r.MOD;
        //        dr["FPD"] = r.FPD;
        //        dr["UEC"] = r.UEC;
        //        dr["OverAll"] = r.OverAll;
        //        dt.Rows.Add(dr);
        //    }

        //    // --- Export to Excel ---
        //    using (XLWorkbook wb = new XLWorkbook())
        //    {
        //        wb.Worksheets.Add(dt, "CombinedData");
        //        wb.SaveAs(filePath);
        //    }
        //}


        public static void ExportSideBySide(DataGridView gridView, List<GS1_QC_CheckResult> results, string filePath)
        {
            DataTable dt = new("CombinedData");

            // --- Add GridView columns ---
            foreach (DataGridViewColumn col in gridView.Columns)
            {
                dt.Columns.Add(col.HeaderText);
            }

            // --- Add QC columns ---
            dt.Columns.Add("Decode");
            dt.Columns.Add("SC (Symbol Contrast)");
            dt.Columns.Add("AN (Axial Nonuniformity)");
            dt.Columns.Add("GN (Grid Nonuniformity)");
            dt.Columns.Add("MOD (Modulation)");
            dt.Columns.Add("FPD (Fixed Pattern Damage)");
            dt.Columns.Add("UEC (Unused Error Correction)");
            dt.Columns.Add("PG (Print Growth)");
            dt.Columns.Add("AngleOf Distortion");
            dt.Columns.Add("Quiet Zone");
            dt.Columns.Add("OverAll");

            // --- Merge rows side by side ---
            for (int i = 0; i < gridView.Rows.Count; i++)
            {
                var row = gridView.Rows[i];
                if (row.IsNewRow) continue;

                var dr = dt.NewRow();

                // Fill GridView values
                for (int j = 0; j < gridView.Columns.Count; j++)
                {
                    dr[j] = row.Cells[j].Value?.ToString() ?? string.Empty;
                }

                // Fill QC values if available
                if (i < results.Count)
                {
                    var qc = results[i];
                    dr["Decode"] = qc.Decode;
                    dr["SC (Symbol Contrast)"] = qc.SC;
                    dr["AN (Axial Nonuniformity)"] = qc.AN;
                    dr["GN (Grid Nonuniformity)"] = qc.GN;
                    dr["MOD (Modulation)"] = qc.MOD;
                    dr["FPD (Fixed Pattern Damage)"] = qc.FPD;
                    dr["UEC (Unused Error Correction)"] = qc.UEC;
                    dr["PG (Print Growth)"] = qc.PG;
                    dr["AngleOf Distortion"] = qc.AS9132_Distortion;
                    dr["Quiet Zone"] = qc.AS9132_QuietZone;
                    dr["OverAll"] = qc.OverAll;
                }

                dt.Rows.Add(dr);
            }

            // --- Export to Excel ---
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "CombinedData");
                wb.SaveAs(filePath);
            }

            MessageBox.Show($"Excel files generated.{Environment.NewLine}Path: {filePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
