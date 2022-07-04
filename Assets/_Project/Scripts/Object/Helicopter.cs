using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour
{
    private Bulldozer bulldozer;
    [SerializeField] private Transform child;
    private Animator animator;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        bulldozer = FindObjectOfType<Bulldozer>();
        Release();
        GameManager.Instance.WinAction += HoldOn;
    }

    private void Release()
    {
        StartCoroutine(ReleaseAnim());
    }
    private void HoldOn()
    {
        StartCoroutine(HoldAnim());
    }
    private IEnumerator ReleaseAnim()
    {
        
        child.localPosition = new Vector3(-30, 20, 0);
        child.localScale = bulldozer.transform.localScale * .2f;

        bulldozer.transform.position = new Vector3(-30,20,0);

        yield return new WaitForSeconds(.2f);
        bulldozer.Busy(true);
        bulldozer.transform.position = child.position;
        bulldozer.transform.parent = child;

        yield return new WaitForSeconds(.5f);
        Vector3 pos = new Vector3(1, 20, 0);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            child.localPosition = Vector3.Lerp(child.localPosition, pos, t);
            yield return null;
        }
        animator.Play("ClawRelease");
        bulldozer.transform.parent = null;

        // bulldozer.Busy(false);
        UIManager.Instance.BarActivate(true);


        GameManager.Instance.Play();
        yield return new WaitForSeconds(2f);
        child.gameObject.SetActive(false);
    }

    private IEnumerator HoldAnim()
    {
        child.gameObject.SetActive(true);
        child.localPosition = new Vector3(0, 20, 0);
        child.localScale = bulldozer.transform.localScale * .2f;

        bulldozer.Busy(true);
        yield return new WaitForSeconds(.2f);

        Vector3 pos = bulldozer.transform.position;


        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            child.localPosition = Vector3.Lerp(child.localPosition, pos, t);
            yield return null;
        }
        animator.Play("ClawHold");


        bulldozer.transform.position = child.position;
        bulldozer.transform.parent = child;

        pos = new Vector3(0, 20, 0);
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            child.localPosition = Vector3.Lerp(child.localPosition, pos, t);
            yield return null;
        }
    }
}
