using Godot;
using System;
using System.IO;

[Tool]
public partial class ExcelTool : Resource
{
    [ExportToolButton("Create Json")]
    public Callable ClickButton => Callable.From(CreateJsons);

    public void CreateJsons()
    {
        var dirPath = Path.GetDirectoryName(ProjectSettings.GlobalizePath(ResourcePath));

        var settingPath = Path.Combine(dirPath, "_settings.json");

        using var file = Godot.FileAccess.Open(settingPath, Godot.FileAccess.ModeFlags.Read);
        var json = Json.ParseString(file.GetAsText());
        var tables = json.AsGodotDictionary()["tables"].AsGodotArray();
        foreach (var item in tables)
        {
            var table = item.AsGodotDictionary<string, string>();
            var excelPath = Path.GetFullPath(Path.Combine(dirPath, table["path"]));
            var sheetName = table["sheetName"];
            GD.Print(excelPath);

            var tableJson = ExcelReader.ReadJson(excelPath, sheetName);
            GD.Print(tableJson);

            var fileOutPath = Path.Combine(dirPath, sheetName + ".json");
            // GD.Print(fileOutPath);

            // CreateText(Path.Combine(outPath, fileName + ".json"), json);
            using var jsonFile = Godot.FileAccess.Open(fileOutPath, Godot.FileAccess.ModeFlags.Write);
            jsonFile.StoreString(tableJson);

            GD.Print("Create Json File: ", fileOutPath);
        }

        EditorInterface.Singleton.GetResourceFilesystem().Scan();
    }
}
