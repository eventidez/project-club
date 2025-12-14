using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using MiniExcelLibs;

[Tool, GlobalClass]
public partial class ExcelReader : RefCounted
{
    public const string NameCell = "##var";
    public const string TypeCell = "##type";

    public static string ToGlobalPath(string path)
    {
        path = Combine(System.Environment.CurrentDirectory, path);
        return Path.GetFullPath(path);
    }

    public static string Combine(params string[] paths)
    {
        return Path.Combine(paths);
    }

    public static IEnumerable<T> Read<T>(string path, string sheetname = null, string startCell = "A1", bool hasHeader = true) where T : class, new()
    {
        if (File.Exists(path) == false)
        {
            throw new FileNotFoundException($"File '{path}' is invalid.");
        }

        return MiniExcel.Query<T>(path, sheetname, ExcelType.UNKNOWN, startCell, null, hasHeader);
    }

    public static IEnumerable<IDictionary<string, object>> Read(string path, string sheetname = null, string startCell = "A1", bool hasHeader = true)
    {
        if (File.Exists(path) == false)
        {
            throw new FileNotFoundException($"File '{path}' is invalid.");
        }

        return MiniExcel.Query(path, hasHeader, sheetname, ExcelType.UNKNOWN, startCell, null).Cast<IDictionary<string, object>>();
    }

    public static void Log(string excelPath, string startCell = "A1")
    {
        foreach (var row in Read(excelPath, startCell: startCell))
        {
            var text = "";
            foreach (var column in row)
            {
                text += $"( '{column.Key}': '{column.Value}' )";
            }

            GD.Print(text);
        }
    }

    public static Godot.Collections.Array<string> GetColumns(string excelPath, string name)
    {
        var results = new Godot.Collections.Array<string>();
        ReadRow(excelPath, (row, types) =>
        {
            results.Add(row[name]);
        });

        return results;
    }

    public static void ToResources(string excelPath, string outPath, string typeName)
    {
        var type = Type.GetType(typeName);
        if (type == null)
        {
            return;
        }

        var results = new Godot.Collections.Array<Resource>();
        ReadRow(excelPath, (row) =>
        {
            var id = row["Id"];
            var resourcePath = Combine(outPath, id + ".tres");
            var res = CreateOrGetResource(resourcePath, type);
            res.ResourceName = id;
            results.Add(res);
            return res;
        });

        foreach (var res in results)
        {
            ResourceSaver.Save(res);
        }
        // var startCell = GetStartCell(excelPath);
        // var colTypes = new Godot.Collections.Dictionary<string, string>();
        // var rowIndex = 0;
        // foreach (var row in Read(excelPath, startCell: startCell).Select(ToGodotDictionary))
        // {
        //     rowIndex++;
        //     var first = row[NameCell];

        //     if (rowIndex == 1 && first.Equals(TypeCell))
        //     {
        //         colTypes = row;
        //         continue;
        //     }

        //     if (first.StartsWith("##"))
        //     {
        //         continue;
        //     }

        //     var id = row["Id"];
        //     var resourcePath = Combine(outPath, id + ".tres");
        //     var res = CreateOrGetResource(resourcePath, type);
        //     res.ResourceName = id;
        //     foreach (var col in row)
        //     {
        //         res.Set(col.Key, ToVariant(col.Value, colTypes[col.Key]));
        //     }

        //     // GD.Print();
        //     ResourceSaver.Save(res);
        // }
    }

    private static void ReadRow(string excelPath, Func<Godot.Collections.Dictionary<string, string>, Resource> createResource)
    {
        var startCell = GetStartCell(excelPath);
        var types = new Godot.Collections.Dictionary<string, string>();
        var rowIndex = 0;
        foreach (var row in Read(excelPath, startCell: startCell).Select(ToGodotDictionary))
        {
            rowIndex++;
            var first = row[NameCell];
            if (rowIndex == 1 && first.Equals(TypeCell))
            {
                types = row;
                continue;
            }

            if (first.StartsWith("##"))
            {
                continue;
            }

            var res = createResource(row);
            SetResource(res, row, types);
        }
    }

    private static void ReadRow(string excelPath, Action<Godot.Collections.Dictionary<string, string>, Godot.Collections.Dictionary<string, string>> callback)
    {
        var startCell = GetStartCell(excelPath);
        var types = new Godot.Collections.Dictionary<string, string>();
        var rowIndex = 0;
        foreach (var row in Read(excelPath, startCell: startCell).Select(ToGodotDictionary))
        {
            rowIndex++;
            var first = row[NameCell];
            if (rowIndex == 1 && first.Equals(TypeCell))
            {
                types = row;
                continue;
            }

            if (first.StartsWith("##"))
            {
                continue;
            }

            callback(row, types);
        }
    }

    private static string GetStartCell(string excelPath)
    {
        var rowIndex = 0;
        foreach (var row in MiniExcel.Query(excelPath).Cast<IDictionary<string, object>>())
        {
            rowIndex++;
            if (row.TryGetValue("A", out var rowA) == false || rowA == null)
            {
                continue;
            }

            var aValue = rowA.ToString();
            if (string.IsNullOrEmpty(aValue))
            {
                continue;
            }

            if (NameCell.StartsWith(aValue))
            {
                return "A" + rowIndex;
            }
        }

        return "A1";
    }

    private static Godot.Collections.Dictionary<string, string> ToGodotDictionary(IDictionary<string, object> dict)
    {
        var row = new Godot.Collections.Dictionary<string, string>();
        foreach (var kv in dict)
        {
            row[kv.Key] = kv.Value == null ? "" : kv.Value.ToString();
        }

        return row;
    }

    private static Resource CreateOrGetResource(string resourcePath, Type type)
    {
        if (ResourceLoader.Exists(resourcePath))
        {
            return ResourceLoader.Load(resourcePath);
        }

        var res = (Resource)Activator.CreateInstance(type);
        res.ResourcePath = resourcePath;
        return res;
    }

    private static void SetResource(Resource resource, Godot.Collections.Dictionary<string, string> row, Godot.Collections.Dictionary<string, string> types)
    {
        foreach (var col in row)
        {
            if (col.Key.StartsWith('#'))
            {
                continue;
            }

            resource.Set(col.Key, ConvertValue(col.Value, types[col.Key]));
        }
    }

    public static string ReadJson(string excelFile, string sheetName = null)
    {
        var table = new Godot.Collections.Array();
        ReadExcel(excelFile, sheetName, (types, row) =>
        {
            // var dict = new Godot.Collections.Dictionary();
            // foreach (var pair in row)
            // {
            //     dict[pair.Key] = pair.Value;
            // }

            table.Add(row);
        });

        return Json.Stringify(table, "\t");
    }

    public static void ReadExcel(string excelFile,
          Action<Godot.Collections.Dictionary<string, string>, Godot.Collections.Dictionary<string, Variant>> callback)
    {
        ReadExcel(excelFile, null, callback);
    }

    public static void ReadExcel(string excelFile, string sheetName,
        Action<Godot.Collections.Dictionary<string, string>, Godot.Collections.Dictionary<string, Variant>> callback)
    {
        var colIndex = 0;
        var headers = new Godot.Collections.Dictionary<string, string>();
        var types = new Godot.Collections.Dictionary<string, string>();
        foreach (var row in MiniExcel.Query(excelFile, sheetName: sheetName).Cast<IDictionary<string, object>>())
        {
            colIndex++;
            var aValue = row["A"];
            if (colIndex == 1 && aValue.Equals(NameCell))
            {
                foreach (var pair in row)
                {
                    if (pair.Value == null)
                    {
                        continue;
                    }

                    var header = pair.Value.ToString();
                    if (string.IsNullOrWhiteSpace(header) || header.StartsWith("#"))
                    {
                        continue;
                    }

                    headers.Add(pair.Key, header);
                }

                continue;
            }

            if (colIndex == 2 && aValue.Equals(TypeCell))
            {
                types.Clear();
                foreach (var pair in row)
                {
                    if (headers.TryGetValue(pair.Key, out var header) == false)
                    {
                        continue;
                    }

                    if (pair.Value == null)
                    {
                        continue;
                    }

                    types.Add(header, pair.Value.ToString());
                }

                continue;
            }

            if (aValue != null && aValue.ToString().StartsWith("##"))
            {
                continue;
            }

            var dict = new Godot.Collections.Dictionary<string, Variant>();
            foreach (var pair in row)
            {
                if (headers.TryGetValue(pair.Key, out var header) == false)
                {
                    continue;
                }

                if (pair.Value == null)
                {
                    continue;
                }
                var valueText = pair.Value.ToString();
                if (string.IsNullOrWhiteSpace(valueText.ToString()))
                {
                    continue;
                }

                try
                {
                    dict[header] = ConvertValue(valueText, types[header]);
                }
                catch (Exception)
                {
                    GD.PrintErr($"'{pair.Key}{colIndex}' in '{excelFile}'");
                }
            }

            if (dict.Count == 0)
            {
                continue;
            }

            callback(types, dict);
        }
    }
    private static Variant ConvertValue(string valueText, string type)
    {
        var typeParams = type.Split(',').Select((t) => t.Trim()).ToArray();
        Variant value = valueText;
        if (typeParams.Length == 1)
        {
            var text = valueText.ToString();
            value = typeParams[0] switch
            {
                "int" => int.Parse(valueText),
                "float" => float.Parse(valueText),
                "bool" => bool.Parse(valueText),
                "vec2" => ToVector2(text),
                _ => text,
            };
        }
        else if (typeParams.Length == 2
            && typeParams[0] == "list" && typeParams[1] == "string")
        {
            value = new Godot.Collections.Array<string>(valueText.ToString().Split(','));
        }

        return value;
    }

    private static Variant ToVariant(string value, string type)
    {
        switch (type)
        {
            case "bool":
                return bool.Parse(value);
            case "int":
                return int.Parse(value);
        }

        if (type == "list")
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new Godot.Collections.Array<string>();
            }
            else
            {
                return new Godot.Collections.Array<string>(value.Split(','));
            }
        }

        if (type == "resource:card")
        {
            return ResourceLoader.Load($"res://resources/cards/{value}.tres");
        }

        return value;
    }

    private static Vector2 ToVector2(string text)
    {
        var list = text.Split(',');
        return new Vector2(float.Parse(list[0]), float.Parse(list[1]));
    }
}
