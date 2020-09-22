using UnityEngine;

/// <summary>
/// Реализация смерти лежит в самом PlayerHealth. Данный скрипт нужен только для обозначения
/// </summary>
public class KillingZone : MonoBehaviour
{
    private Collider2D[] collider2Ds;


    private void Start()
    {
        collider2Ds = gameObject.GetComponentsInChildren<Collider2D>();
    }


    public void SetIsTriggerZone(bool isTrigger)
    {
        foreach (Collider2D collider2D in collider2Ds)
        {
            collider2D.isTrigger = isTrigger;
        }
    }
}
