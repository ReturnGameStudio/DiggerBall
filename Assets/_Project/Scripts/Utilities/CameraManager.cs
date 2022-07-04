using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : Manager<CameraManager>
{
   [SerializeField] private CinemachineVirtualCamera _cm;
    private CinemachineTransposer _cb;
    private Vector3 _startPos;
    private void Start()
    {
       
        _cb = _cm.GetCinemachineComponent<CinemachineTransposer>();
        _startPos = _cb.m_FollowOffset;
    }

    public void UpdateCamera(float a)
    {
       if(_cb!=null)
        _cb.m_FollowOffset = _startPos + new Vector3(0,a,-a*.6f);
    }
}
