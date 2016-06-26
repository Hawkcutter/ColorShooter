using System;
using UnityEngine;

public class ColorKey
{
    public enum EColorKey { Red, Blue, Green, Yellow,       White,          Count }



    private static LinearProjectile[] coloredProjectiles;
    private static ColorKey[] colors;

    public static ColorKey GetColorKey(EColorKey key)
    {
        return colors[(int)key];
    }

    public static void InitColors(LinearProjectile redPrefab, LinearProjectile greenPrefab, LinearProjectile bluePrefab, LinearProjectile yellowPrefab)
    {
        colors = new ColorKey[(int)EColorKey.Count];

        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = new ColorKey((EColorKey)i);
        }

        coloredProjectiles = new LinearProjectile[4];
        coloredProjectiles[(int)EColorKey.Red] = redPrefab;
        coloredProjectiles[(int)EColorKey.Blue] = bluePrefab;
        coloredProjectiles[(int)EColorKey.Green] = greenPrefab;
        coloredProjectiles[(int)EColorKey.Yellow] = yellowPrefab;
    }
    public static ColorKey GetRandomColorKey()
    {
        Debug.Assert(colors != null);

        //TODO, Hack damit keine weißen gegner spawnen
        int randVal = (int)(UnityEngine.Random.value * (colors.Length-1));
        return colors[randVal];
    }
    public static Color GetColorOf(EColorKey key)
    {
        return colors[(int)key].RgbColor;
    }

    private EColorKey key;
    public EColorKey Key { get { return key; } }

    private Color color;
    public Color RgbColor { get { return color; } }

    private ColorKey(EColorKey key)
    {
        this.key = key;

        if (key == EColorKey.Red)
            color =  new Color(1.0f, 0.0f, 0.0f, 1.0f);

        else if (key == EColorKey.Blue)
            color = new Color(0.08f, 0.5f, 1.0f, 1.0f);

        else if (key == EColorKey.Green)
            color = new Color(0.0f, 1.0f, 0.0f, 1.0f);

        else if (key == EColorKey.Yellow)
            color = new Color(1.0f, 1.0f, 0.0f, 1.0f);

        else if (key == EColorKey.White)
        {
            color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }

        else
        {
            color = new Color(1.0f, 0.0f, 1.0f, 1.0f);
            Debug.Log("Not allowed!");
        }
    }

    public static LinearProjectile GetProjectileFromColor(EColorKey eColorKey)
    {
        return coloredProjectiles[(int)eColorKey];
    }
}


