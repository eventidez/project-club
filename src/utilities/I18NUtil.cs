using Godot;
using System;

public partial class I18NUtil : GodotObject
{
    public static string GetString(string key)
    {
        var json = ResourceLoader.Load<Json>("res://configs/i18n/i18n.json");
        foreach (var item in json.Data.AsGodotArray())
        {
            var dict = item.AsGodotDictionary<string, string>();
            if (dict.TryGetValue("Id", out var value) == false)
            {
                continue;
            }
            
            if (value.Equals(key) == false)
            {
                continue;
            }

            return dict["schinese"];
        }

        return "";
    }
}
