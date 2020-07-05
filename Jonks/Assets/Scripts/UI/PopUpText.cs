using System.Collections;
using TMPro;
using UnityEngine;

public class PopUpText : MonoBehaviour, IPooledObject
{
    private TextMeshPro textMeshPro;

    private float BaseTextAlphaColor = 1f;
    private readonly float lifeTime = 1f;
    private readonly float delayTime = 0.4f;
    private readonly float velocity = 1f;
    private Coroutine routineLife;


    private void Awake()
    {
        textMeshPro = gameObject.GetComponent<TextMeshPro>();
    }


    private void OnDisable()
    {
        if (routineLife != null)
        {
            StopCoroutine(routineLife);
            routineLife = null;
        }
    }

    void IPooledObject.OnObjectSpawn()
    {
        //textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, BaseTextAlphaColor);
        if (routineLife == null)
        {
            routineLife = StartCoroutine(EnumeratorLife());
        }
    }


    private IEnumerator EnumeratorLife()
    {
        float CurrentDelayTime = delayTime;
        while (CurrentDelayTime > 0f)
        {
            yield return null;
            CurrentDelayTime -= Time.deltaTime;
            transform.Translate(Vector3.up * velocity * Time.deltaTime);
        }


        float CurrentLifeTime = lifeTime;
        float CurrentTextAlphaColor;
        while (CurrentLifeTime > 0f)
        {
            CurrentTextAlphaColor = Mathf.Lerp(0f, BaseTextAlphaColor, CurrentLifeTime / lifeTime);
            textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, CurrentTextAlphaColor);

            transform.Translate(Vector3.up * velocity * Time.deltaTime);

            yield return null;
            CurrentLifeTime -= Time.deltaTime;

        }

        routineLife = null;

        gameObject.SetActive(false);
    }
}
