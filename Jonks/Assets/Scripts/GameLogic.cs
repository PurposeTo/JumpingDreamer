using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameLogic
{
    /// <summary>
    /// Возвращает нормализованный ортогональный вектор
    /// </summary>
    public static Vector2 GetOrthoNormalizedVector2(Vector2 normal)
    {
        return new Vector2(normal.y, -normal.x).normalized;
    }

    /// <summary>
    /// Возвращает кватернион, повернутый перпендикулярно цели
    /// </summary>
    public static Quaternion GetOrthoRotation(Vector3 currentPosition, Vector3 target)
    {
        Vector3 difference = (currentPosition - target).normalized;

        // Вычисляем кватернион нужного поворота. Вектор forward говорит вокруг какой оси поворачиваться
        Quaternion quaternionRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: difference);

        return quaternionRotation;
    }

    /// <summary>
    /// Перемешивает массив элементов 
    /// </summary>
    public static void Shuffle<T>(T[] deck)
    {
        for (int i = 0; i < deck.Length; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, deck.Length);

            Swap(ref deck[i], ref deck[randomIndex]);
        }
    }


    public static void Swap<T>(ref T x, ref T y)
    {
        T temp = x;
        x = y;
        y = temp;
    }


    public static float ClampValueByAnotherValue(float valueToClamp, float valueWhichClamping, float howManyTimes)
    {
        return (valueToClamp + (valueWhichClamping * howManyTimes)) / howManyTimes;
    }
}
