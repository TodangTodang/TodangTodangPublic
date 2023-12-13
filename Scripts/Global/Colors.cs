using UnityEngine;

public class Colors
{
    private static Color _grey;
    private static Color _moonYellow;
    private static Color _pink;
    private static Color _lightBlue;
    private static Color _deepBlue;
    private static Color _coral;
    private static Color _textBlue;

    public static Color Grey 
    { 
        get 
        { 
            if (_grey == Color.clear) _grey = new Color32(87, 87, 87, 255);
            return _grey;
        } 
    }
    public static Color MoonYellow
    {
        get
        {
            if (_moonYellow == Color.clear) _moonYellow = new Color32(255, 208, 114, 255);
            return _moonYellow;
        }
    }
    public static Color Pink
    {
        get
        {
            if (_pink == Color.clear) _pink = new Color32(255, 189, 175, 255);
            return _pink;
        }
    }
    public static Color LightBlue
    {
        get
        {
            if (_lightBlue == Color.clear) _lightBlue = new Color32(226, 234, 255, 255);
            return _lightBlue;
        }
    }
    public static Color DeepBlue
    {
        get
        {
            if (_deepBlue == Color.clear) _deepBlue = new Color32(181, 197, 254, 255);
            return _deepBlue;
        }
    }
    public static Color Coral
    {
        get
        {
            if (_coral == Color.clear) _coral = new Color32(255, 109, 96, 255);
            return _coral;
        }
    }
    public static Color TextBlue
    {
        get
        {
            if (_textBlue == Color.clear) _textBlue = new Color32(86, 99, 147, 255);
            return _textBlue;
        }
    }
}
