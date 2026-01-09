using Godot;
using System;
using System.Collections.Generic;
using System.IO;

[Tool]
public partial class DialogueToYarnTool : Resource
{
    [ExportToolButton("Create Json")]
    public Callable ClickButton => Callable.From(CreateJsons);

    public static string NewLine => System.Environment.NewLine;

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
            // GD.Print(excelPath);
            var yarnText = GetYarnText(excelPath, sheetName);
            var yarnOutPath = Path.Combine(dirPath, sheetName + ".yarn");

            using var jsonFile = Godot.FileAccess.Open(yarnOutPath, Godot.FileAccess.ModeFlags.Write);
            jsonFile.StoreString(yarnText);

            var absPath = Path.Combine(dirPath, sheetName + ".yarnproject");
            var jsonProject = new Yarn.Compiler.Project();
            jsonProject.SourceFilePatterns = [sheetName + ".yarn"];
            jsonProject.SaveToFile(absPath);

            // GD.Print("Create Json File: ", fileOutPath);
        }

        EditorInterface.Singleton.GetResourceFilesystem().Scan();
    }

    // private void ToDialogue(Variant data)
    // {
    //     foreach (var row in data.AsGodotArray<Godot.Collections.Dictionary>())
    //     {
    //     }
    // }

    private string GetYarnText(string excelPath, string sheetName)
    {
        var yarnText = string.Empty;

        ExcelReader.ReadExcel(excelPath, sheetName, (types, row) =>
        {
            if (row.TryGetValue("Label", out var value) && string.IsNullOrWhiteSpace(value.AsString()) == false)
            {
                if (value.AsString() == "*")
                {
                    yarnText += "===" + NewLine;

                }
                else
                {
                    yarnText += "title: " + value.ToString() + NewLine + "---" + NewLine;
                }
            }

            if (row.TryGetValue("Face", out value) && string.IsNullOrWhiteSpace(value.ToString()) == false)
            {
                yarnText += $"<<Face {value.AsString()}>>" + NewLine;
            }

            if (row.TryGetValue("Fg", out value) && string.IsNullOrWhiteSpace(value.ToString()) == false)
            {
                foreach (var command in value.AsGodotArray())
                {
                    GD.Print(command);
                    yarnText += $"<<Fg {command}>>" + NewLine;
                }
            }
            // if (row.TryGetValue("Commands", out value) && string.IsNullOrWhiteSpace(value.ToString()) == false)
            // {
            //     using var reader = new StringReader(value.ToString());
            //     var line = reader.ReadLine();
            //     while (line != null)
            //     {
            //         yarnText += $"<<{line}>>" + System.Environment.NewLine;
            //         line = reader.ReadLine();
            //     }
            // }

            if (row.TryGetValue("Type", out value) && value.AsInt32() == 2)
            {
                yarnText += $"<<Heart \"{row["Content"].AsString()}\">>" + NewLine;
            }
            else
            {
                ToContent(ref yarnText, row);
            }

            ToOption(row, 0, ref yarnText);
            ToOption(row, 1, ref yarnText);
            ToOption(row, 2, ref yarnText);
            ToOption(row, 3, ref yarnText);
        });

        return yarnText;
    }

    private void ToContent(ref string yarnText, Godot.Collections.Dictionary<string, Variant> row)
    {
        if (row.TryGetValue("Content", out var value) && string.IsNullOrWhiteSpace(value.ToString()) == false)
        {
            var content = value.ToString();
            if (row.TryGetValue("Name", out value) && string.IsNullOrWhiteSpace(value.ToString()) == false)
            {
                content = value.ToString() + ": " + content;
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                return;
            }

            var texts = ReadLine(content);
            content = texts[0];
            var index = 1;
            while (index < texts.Length)
            {
                content += $"[r/]{texts[index]}";
                index++;
            }
            // content.Replace(NewLine, "\\n");
            yarnText += content + System.Environment.NewLine;
        }
    }

    private void ToOption(Godot.Collections.Dictionary<string, Variant> row, int index, ref string yarnText)
    {
        if (row.TryGetValue($"Option{index}", out var value) == false)
        {
            return;
        }

        yarnText += "-> " + value.AsString() + NewLine;
        // "    "
        var next = row[$"Option{index}_Next"].AsString();
        using var reader = new StringReader(next.ToString());
        var line = reader.ReadLine();
        while (line != null)
        {
            if (line.StartsWith("Jump"))
            {
                line = "jump" + line[4..];
            }
            yarnText += $"\t<<{line}>>" + NewLine;
            line = reader.ReadLine();
        }
    }

    public string[] ReadLine(string text)
    {
        using var reader = new StringReader(text);
        var texts = new List<string>();
        var line = reader.ReadLine();
        while (line != null)
        {
            texts.Add(line);
            line = reader.ReadLine();
        }

        return [.. texts];
    }

    public string[] ToArray(string text)
    {
        var results = text.Split(',');
        for (int i = 0; i < results.Length; i++)
        {
            results[i] = results[i].Trim();
        }

        return results;
    }
}
