using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;


    private void Start()
    {
        UIManager.Instance.UpgradeActivate(true);
    }

    public IEnumerator StartBarAction()
    {
        _slider.gameObject.SetActive(true);
        GameManager.isGameStarted=true;
        if (_slider == null) yield break;
        while (enabled)
        {
            _slider.value = Mathf.PingPong(Time.time * .7f, 1);
            yield return null;
        }
    }

    public void Throw()
    {
        int max = 1;

        if (_slider.value >= 0f && _slider.value <= .225f)
        {
            max = 20;
        }
        else if (_slider.value > .225f && _slider.value <= .45f)
        {
            max = 60;
        }
        else if (_slider.value > .45f && _slider.value <= .55f)
        {
            max = 100;
        }
        else if (_slider.value > .55f && _slider.value <= .775f)
        {
            max = 50;
        }
        else if (_slider.value > .775f && _slider.value <=1f)
        {
            max = 20;
        }


        Bulldozer b = GameManager.Instance.Player.GetComponent<Bulldozer>();
        b.InitialExcavation(max);

        UIManager.Instance.BarActivate(false);
        UIManager.Instance.GameActivate(true);
    }
}
