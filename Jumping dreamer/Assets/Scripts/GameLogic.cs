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
    /// Генерирует массив векторов, точки которых описывают круг, радиус которого равен единице
    /// </summary>
    /// <param name="distanceAngle">Угол, на расстоянии которого будут распологаться точки</param>
    public static Vector2[] GetVector2sDirectionsAroundCircle(float distanceAngle)
    {
        List<Vector2> vector2sDirections = new List<Vector2>();

        int dotsCount = (int)(360f / distanceAngle);

        for (int i = 0; i < dotsCount; i++)
        {
            float rotateAngle = distanceAngle * i;
            Quaternion rotation = Quaternion.Euler(0, 0, rotateAngle);
            Vector2 direction = rotation * Vector3.up;

            vector2sDirections.Add(direction);
        }

        return vector2sDirections.ToArray();
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


    /// <summary>
    /// Получить случайные элементы массива
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="deck">Массив элементов для выбора. Не может быть пустым</param>
    /// <returns></returns>
    public static T[] GetRandomItems<T>(T[] deck)
    {
        if (deck == null || deck.Length == 0) throw new System.Exception("Deck can't being empty!");

        List<T> listOfReturnedItems = new List<T>();
        Shuffle(deck);

        // Минимальное включительное значение - 1, а максимально включительное значение - deck.Length
        int numberOfRandomIntems = UnityEngine.Random.Range(1, deck.Length + 1);

        for (int i = 0; i < numberOfRandomIntems; i++) listOfReturnedItems.Add(deck[i]);

        return listOfReturnedItems.ToArray();
    }


    /// <summary>
    /// Получить случайный элемент из массива
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="deck"></param>
    /// <returns></returns>
    public static T GetRandomItem<T>(T[] deck)
    {
        return deck[UnityEngine.Random.Range(0, deck.Length)];
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


    public static Vector2 ClampVectorByMagnitude(Vector2 vector, float minMagnitude)
    {
        float scale = vector.magnitude < minMagnitude
            ? minMagnitude / vector.magnitude
            : 1f;
        return vector * scale;
    }


    public static T InstantiateAndGetComponent<T>(GameObject prefab) where T : Component
    {
        return UnityEngine.Object.Instantiate(prefab).GetComponent<T>();
    }


    public static T SpawnFromPoolAndGetComponent<T>(GameObject prefabKey, Vector3 position, Quaternion rotation, Transform parent = null)
        where T : Component
    {
        return ObjectPooler.Instance.SpawnFromPool(prefabKey, position, rotation, parent).GetComponent<T>();
    }
}
