using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : Manager<InputManager>
{

    public Action MouseUpAction;

    private float _startPos;
    public float swipeDelta;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GameManager.Instance.GameStatus == GameStatus.Null)
            {
                GameManager.Instance.Play();
            }

            _startPos = Input.mousePosition.x;
        }
        else if (Input.GetMouseButton(0))
        {
            swipeDelta = Mathf.Clamp(Mathf.Lerp(swipeDelta, Input.mousePosition.x - _startPos, Time.deltaTime* 3f), -1, 1);
            _startPos = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            swipeDelta = 0;
            MouseUpAction?.Invoke();
        }

    }
}
