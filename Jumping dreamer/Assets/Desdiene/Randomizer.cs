using System.Collections.Generic;
using UnityEngine;

namespace Desdiene
{
    public static class Randomizer
    {
        /// <summary>
        /// Перемешивает массив элементов 
        /// </summary>
        public static void Shuffle<T>(T[] deck)
        {
            for (int i = 0; i < deck.Length; i++)
            {
                int randomIndex = Random.Range(0, deck.Length);

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
            int numberOfRandomIntems = Random.Range(1, deck.Length + 1);

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
            return deck[Random.Range(0, deck.Length)];
        }


        public static void Swap<T>(ref T x, ref T y)
        {
            T temp = x;
            x = y;
            y = temp;
        }


        public static T[] GetAllEnumValues<T>() where T : System.Enum
        {
            return (T[])System.Enum.GetValues(typeof(T));
        }


        public static T GetRandomEnumItem<T>() where T : System.Enum
        {
            return GetRandomItem(GetAllEnumValues<T>());
        }
    }
}
