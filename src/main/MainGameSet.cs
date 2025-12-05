using Godot;
using System;

public partial class MainGameSet : Resource
{
    private int _day = 1;

    public int Day
    {
        get => _day; set
        {
            _day = value;
            ValueChanged?.Invoke();
        }
    }

    public Action ValueChanged;
}
