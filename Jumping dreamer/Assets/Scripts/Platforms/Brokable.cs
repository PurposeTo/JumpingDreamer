using UnityEngine;

public class Brokable : MonoBehaviour
{
    private int touchesToBroke = 6;

    private int touchCount;


    private void OnEnable()
    {
        touchCount = 0;
    }


    private void OnDisable()
    {
        MakeItMoreBreakable();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerPresenter _))
        {
            if (collision.enabled)
            {
                touchCount++;
            }

            if (touchCount >= touchesToBroke) Breakdown();
        }
    }


    private void Breakdown()
    {
        gameObject.SetActive(false);
    }


    private void MakeItMoreBreakable()
    {
        if (touchesToBroke > 1) touchesToBroke--;
    }
}
