using Godot;
using System;

namespace Game;

public partial class MainGameRepo : Resource, IMainGameRepo
{
    private int _day = 1;

    public int Day
    {
        get => _day;
        set
        {
            _day = value;
        }
    }

    public void NextDay()
    {
        Day += 1;
    }
}
