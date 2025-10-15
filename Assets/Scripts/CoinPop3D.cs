using UnityEngine;
using System.Collections;

public class CoinPop3D : MonoBehaviour
{
    public float popHeight = 1f;
    public float popTime = 0.1f;     // super fast rise
    public float spinSpeed = 360f;   // fast spin for a snappy feel
    public int coinValue = 1;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
        StartCoroutine(PopMotion());
        Destroy(gameObject, 0.2f);   // disappear very fast
    }

    IEnumerator PopMotion()
    {
        float timer = 0f;
        Vector3 target = startPos + Vector3.up * popHeight;

        while (timer < popTime)
        {
            timer += Time.deltaTime;
            float t = timer / popTime;
            transform.position = Vector3.Lerp(startPos, target, t);
            transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
