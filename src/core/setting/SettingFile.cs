using Godot;
using System;

namespace GameSystems.Setting;

[GlobalClass]
public partial class SettingFile : Resource
{
    private ConfigFile _configFile = new();

    public ConfigFile ConfigFile => _configFile;

    public void SetValue(string groupName, string settingName, Variant value)
    {
        _configFile.SetValue(groupName, settingName, value);
    }

    public Variant GetValue(string groupName, string settingName, Variant defaultValue = default)
    {
        return _configFile.GetValue(groupName, settingName, defaultValue);
    }

    public bool HasGroup(string groupName)
    {
        return _configFile.HasSection(groupName);
    }

    public bool HasSetting(string groupName, string settingName)
    {
        return _configFile.HasSectionKey(groupName, settingName);
    }

    public string[] GetGroups()
    {
        return _configFile.GetSections();
    }

    public string[] GetSettings(string groupName)
    {
        return _configFile.GetSectionKeys(groupName);
    }
}
