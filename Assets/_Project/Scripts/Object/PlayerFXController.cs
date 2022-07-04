using System.Collections;
using UnityEngine;

public class PlayerFXController : MonoBehaviour
{
    private Animator _animator;
    private bool _shake;

    [SerializeField] private ParticleSystem levelUpParticle;
    [SerializeField] private Transform bulldozerBody;

    [Space(10)]
    private MeshRenderer _renderer;
    private Color _originalColor;
    [SerializeField] private Color Red;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _renderer = bulldozerBody.GetComponent<MeshRenderer>();
        Invoke("GetOriginalColor", .1f);
    }

    public void LevelUp() 
    {
        if(levelUpParticle==null)return;
       levelUpParticle.Play();
        _animator.Play("bladeScaleAnimation");
        Invoke("GetOriginalColor",.1f);
    }

    public void TurnBlade(float speed) 
    {
        if (!GameManager.Instance.IsPlaying()) return;
        bulldozerBody.Rotate(Time.deltaTime * 180 * (speed), 0, 0);
    }


    public void ShakeBulldozer(float duration, float magnitude) { if (_shake) return; StartCoroutine(Shake(duration, magnitude)); } 
    private IEnumerator Shake(float duration, float magnitude)
    {

        Vector3 pos = bulldozerBody.transform.localPosition;

        float maxDist = .25f;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            _shake = true;
            float x = UnityEngine.Random.Range(-maxDist, maxDist) * magnitude;
            float z = UnityEngine.Random.Range(-maxDist, maxDist) * magnitude;
            bulldozerBody.transform.localPosition = new Vector3(pos.x + x, pos.y, pos.z + z);

            elapsed += Time.deltaTime;

            yield return null;

        }

        bulldozerBody.transform.localPosition = pos;
        _shake = false;

    }


    public void GoTopOrBottom(bool top)
    {
        if (top) StartCoroutine(GoTop());
        else LevelUp();

    }
    private IEnumerator GoTop()
    {
        Vector3 pos = new Vector3(0, 20, 0);
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / 3;
            transform.position = Vector3.Lerp(transform.position, pos, t);
            yield return null;
        }
    }
    
    private void GetOriginalColor()
    {
        _originalColor = PlayerCosmetic.Instance.CurrentAccessory.BodyColor;
    }

    public void ChangeColor(float value)
    {
        Color red = Color.red;
        _renderer.material.color = Color.Lerp(_originalColor, Red,value);
    }


}
