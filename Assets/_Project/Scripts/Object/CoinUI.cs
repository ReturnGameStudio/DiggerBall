using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    Vector2 endPos;

    public void CoinAnimation(Vector3 startPos, string name)
    {
        GameObject g = GameObject.Find(name);
        this.transform.parent = g.transform.parent.parent;
        endPos = g.transform.position;
        transform.localScale = Vector3.one;

        StartCoroutine(CoinAnimationEnum(startPos));
    }

    private IEnumerator CoinAnimationEnum(Vector3 startPos)
    {
        startPos = Camera.main.WorldToScreenPoint(startPos);
        transform.position = startPos;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, endPos, t);
            yield return null;
        }
    }

}
