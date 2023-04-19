using System;

public class Util 
{
    public enum Color
    {
        Red
    }
    public static string Colored(Color color, string text) {
        string colorString = "";
        string reset = "\x1b[0m";

        switch (color)
        {
            case Color.Red:
                colorString = "\x1b[91m";
                break;
        }
        
        return $"{colorString}{text}{reset}";
    }
}