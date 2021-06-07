using AMMCrawler.ServiceLayer.Abstractions;
using AMMCrawler.ServiceLayer.DTO;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AMMCrawler.Export
{
    public class ExcelExporter
    {
        private IGraphService _graphService;
        private int[] _chances =
        {
            0,
            10,
            20,
            30,
            40,
            50,
            60,
            70,
            80,
            90
        };

        public ExcelExporter(IGraphService service)
        {
            _graphService = service;
        }


        public async Task ExportGraphsWithRemovalChance(string appName)
        {
            EdgesCompleteDataDto[] edges = await _graphService.GetEdgesData(appName);
            foreach (int chance in _chances)
            {
                var edgesCopy = new EdgesCompleteDataDto[edges.Length];
                edges.CopyTo(edgesCopy, 0);
                edgesCopy = RemoveNodes(chance, edgesCopy);
                string fileName = string.Format("{0}_{1}", appName, chance);
                await Export(fileName, edgesCopy);
            }

        }

        private EdgesCompleteDataDto[] RemoveNodes(int chance, EdgesCompleteDataDto[] edgesCopy)
        {
            HashSet<int> ids = edgesCopy.Select(e => e.Source).ToHashSet();
            foreach (int id in ids)
                if (NeedRemoveNode(chance))
                    edgesCopy = edgesCopy.Where(e => e.Source != id && e.Target != id).ToArray();
            return edgesCopy;
        }

        public async Task Export(string fileName, EdgesCompleteDataDto[] edges)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(fileName);

                ws.Cells[1, 1].Value = "Source";
                ws.Cells[1, 2].Value = "Target";
                ws.Cells[1, 3].Value = "Label";
                ws.Cells[1, 4].Value = "TargetLabel";

                for (int i = 0; i < edges.Length; i++)
                {
                    EdgesCompleteDataDto edge = edges[i];
                    ws.Cells[i + 2, 1].Value = edge.Source;
                    ws.Cells[i + 2, 2].Value = edge.Target;
                    ws.Cells[i + 2, 3].Value = edge.Label;
                    ws.Cells[i + 2, 4].Value = edge.TargetLabel;
                }
               
                var csvFile = xlPackage.ConvertToCsv();
                await File.WriteAllBytesAsync(fileName + ".csv", csvFile);
            }
        }

        public bool NeedRemoveNode(int chance)
        {
            int generatedValue = RandomNumberGenerator.GetInt32(1, 100);
            return chance >= generatedValue;
        }
    }
}
