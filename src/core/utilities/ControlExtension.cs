using Godot;
using System;

public static class ControlExtension
{
    public static void SetActive(this Control control, bool value)
    {
        if (value && control.Visible)
        {
            control.Visible = false;
        }

        control.Visible = value;
    }
}
