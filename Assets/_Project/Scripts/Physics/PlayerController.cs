using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static Action OnStopParticle;

    [Header("Joystick")]
    [SerializeField] private Joystick _joystick;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private ParticleSystem particle2;

    [Header("Components")]
    protected Rigidbody _rigidbody;
    private Bulldozer _bulldozer;

    [Header("Variables")]
    [SerializeField] private float JUMP_FORCE = 1000;
    [Range(.1f, 20)] public float speed = 5;
    private float _x;
    private float _y;
    void Start()
    {
        _joystick = FindObjectOfType<Joystick>();
        _rigidbody = GetComponent<Rigidbody>();
        _bulldozer = GetComponent<Bulldozer>();
    }


    private void Update()
    {
        CheckJoystick();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void CheckJoystick()
    {
        if (!GameManager.Instance.IsPlaying()) return;

        if (_joystick != null)
        {
            _x = Mathf.Clamp(_joystick.Horizontal, -1, 1);
            _y = Mathf.Clamp(_joystick.Vertical, -1, 1);
        }
    }
    private void PlayerMovement()
    {
        if (!GameManager.Instance.IsPlaying()) return;

        float velocity = (_x != 0) ? _x : (_y != 0 ? _y : 0);

        if (velocity != 0)
        {
            transform.eulerAngles = new Vector3(0, Mathf.Atan2(_x, _y) * 180 / Mathf.PI, 0);
            Move();
            _bulldozer.BottomDiggerActive(false);
        }
        else _bulldozer.BottomDiggerActive(true);

    }

    private void Move()
    {
        Vector3 vel = transform.forward * speed;
        vel.y = _rigidbody.velocity.y;
        _rigidbody.velocity = vel;
    }

    public void ResetVelocity()
    {
        _rigidbody.velocity *= 0;
        _rigidbody.angularVelocity *= 0;
    }

    public void SetKinematic(bool kinematic)
    {
        _rigidbody.isKinematic = kinematic;
    }
    public void AddForce(Vector3 force) => _rigidbody.AddForce(force);

    private void OnEnable()
    {
        OnStopParticle = () =>
        {
            particle2.gameObject.SetActive(true);
            particle.gameObject.SetActive(false);
        };
    }
}
