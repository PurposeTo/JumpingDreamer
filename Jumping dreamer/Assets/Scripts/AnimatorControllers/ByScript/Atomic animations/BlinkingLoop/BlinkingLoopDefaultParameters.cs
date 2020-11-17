using System;

[Obsolete]
public class BlinkingLoopDefaultParameters : AnimationByScriptDefaultParameters
{
    public bool IsHasALimitedDuration { get; set; } = false; // Есть ли ограниченная длительность у анимации
    public int AmountOfLoopsToExit { get; set; } = 1; // Количество повторений анимации
    public float AnimationDuration { get; set; } = 4f;
    public float LowerAlphaValue { get; set; } = 0.25f; // Нижнее значение альфа-канала при мигании
}
