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
        InputManager.Instance.MouseUpAction += Jump;
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

    private void Jump()
    {
        if (_bulldozer.initialExcavation) return;

        
        if (!isJump && GameManager.Instance.IsPlaying())
        {
            // SetKinematic(true);
            var force = GetComponent<ConstantForce>().force;
            force.y = -100;
            GetComponent<ConstantForce>().force = force;
            _rigidbody.AddForce(Vector3.up * JUMP_FORCE);
            isJump = true;
            //StartCoroutine(Wait());
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(4f);
        isJump = false;
    }


    private void LateUpdate()
    {
        if (isJump)
        { 
            if (!CheckGround(rayDistance*2))
            {
                isAir = true;
            }
        }
    }


    public bool isJump;
    private bool isAir=false;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float rayDistance;
    private bool CheckGround(float dist)
    {
        return Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out var hit, dist, layerMask);
    }

    private bool isStart;
    [SerializeField] private ParticleSystem explo;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MineStone") && isAir)
        {
            explo.Play();
            isAir = false;
            isJump = false;
            var force = GetComponent<ConstantForce>().force;
            force.y = -20;
            GetComponent<ConstantForce>().force = force;
            
            var cols = Physics.OverlapBox(transform.position+2*Vector3.up, new Vector3(7.5f, 1.5f, 7.5f));
            foreach (Collider col in cols)
            {
                if (col.CompareTag("MineStone"))
                {
                    _rigidbody.isKinematic = true;
                    PoolManager.Instance.GetCrushParticle(col.transform.position,col.GetComponent<MineStone>().BlockColor);
                    Destroy(col.gameObject);
                    _rigidbody.isKinematic = false;
                    DataManager.Instance.AddToTotalBlocks(1);
                    DataManager.Instance.AddBlock(1);
                }
            }
        }

        if (collision.gameObject.CompareTag("MineStone") && !isStart)
        {
            StartCoroutine(UIManager.Instance.Fuel());
            isStart = true;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position+2*Vector3.up, new Vector3(15.5f, 3, 15.5f));
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 7.5f);
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
