using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ColorSchemeData")]
public class ColorSchemeData : ScriptableObject
{
    [SerializeField]
    private Color defaultColor = Color.black;

    [SerializeField]
    private Color[] skyColors =
    {
        new Color(131f/255f, 151f/255f, 255f/255f), //Голубой
        new Color(139f/255f, 255f/255f, 131f/255f), //Зеленый
        new Color(255f/255f, 131f/255f, 200f/255f), //Лиловый
        new Color(131f/255f, 255f/255f, 226f/255f), //Бирюзовый
        new Color(175f/255f, 131f/255f, 255f/255f) //Синий
    };


    public Color GetDefaultColorScheme()
    {
        return defaultColor;
    }


    public Color GetRandomColorSchemeExcluding(Color currentColor)
    {
        List<Color> colorsExcluding = skyColors.ToList();
        colorsExcluding.Remove(currentColor);
        return GameLogic.GetRandomItem(colorsExcluding.ToArray());
    }
}
