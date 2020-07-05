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

}
