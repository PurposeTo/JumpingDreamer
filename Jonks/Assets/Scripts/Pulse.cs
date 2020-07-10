using UnityEngine;

public class Pulse : MonoBehaviour
{
    [SerializeField] private bool unscaledTime = false;
    [SerializeField] private float period = 1f;
    [SerializeField, Range(0.1f, 10f)] private float extraSize = 1f;
    private AnimationCurve animationCurve;

    private Vector3 defaultSize;

    private float t;

    private float time = 0f;


    private void OnEnable()
    {
        time = 0f;
    }


    private void Start()
    {
        defaultSize = transform.localScale;
        InitializeCurve();
    }



    private void Update()
    {
        Pulsing();
    }


    private void InitializeCurve()
    {
        animationCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        animationCurve.preWrapMode = WrapMode.PingPong;
        animationCurve.postWrapMode = WrapMode.PingPong;
    }


    private void Pulsing()
    {
        if (unscaledTime)
        {
            time += Time.unscaledDeltaTime;
        }
        else
        {
            time += Time.deltaTime;
        }

        t = animationCurve.Evaluate(time * period);
        Vector3 newSize = defaultSize + Vector3.one * t * extraSize;
        transform.localScale = newSize;

    }
}
