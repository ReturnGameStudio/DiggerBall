using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    private Transform _target;
    void Start()
    {
        _target = GameManager.Instance.Player.transform;
    }

    void FixedUpdate()
    {
        if (_target != null)
        {
            transform.position = Vector3.Slerp(transform.position, _target.position, Time.deltaTime * 10f);

        }
    }
}
