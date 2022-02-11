using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI.Selection;

namespace Vivod_znachenii
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            var walls = new FilteredElementCollector(doc)
                .OfClass(typeof(Wall))
                .Cast<Wall>()
                .ToList();
            string wallInfo = string.Empty;
            foreach (Wall wall in walls)
            {
                string wallType = wall.get_Parameter(BuiltInParameter.ELEM_TYPE_PARAM).AsValueString();
                double wallVolume = wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble();
                wallInfo += $"{wallType}\t{wallVolume}{Environment.NewLine}";             
            }
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string txtPath = Path.Combine(desktopPath, "wallInfo.txt");
            File.WriteAllText(txtPath, wallInfo);  
            return Result.Succeeded;
        }
    }
}
