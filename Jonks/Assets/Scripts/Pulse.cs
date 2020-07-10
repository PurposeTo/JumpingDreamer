using UnityEngine;

public class Pulse : MonoBehaviour
{
    private float period = 1f;
    [Range(0.1f, float.MaxValue)] private float extraSize = 1f;
    private AnimationCurve animationCurve;

    private Vector3 defaultSize;

    private float t;


    private void Start()
    {
        defaultSize = transform.localScale;
        InitializeCurve();

    }


    private void Update()
    {
        t = animationCurve.Evaluate(Time.time * period);
        Vector3 newSize = defaultSize + Vector3.one * t * extraSize;
        transform.localScale = newSize;

    }


    private void InitializeCurve()
    {
        animationCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        animationCurve.preWrapMode = WrapMode.PingPong;
        animationCurve.postWrapMode = WrapMode.PingPong;
    }
}
