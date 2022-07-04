using System;
using System.Collections;
using UnityEngine;
public enum MineType { Stone, Gem }
public class MineStone : MonoBehaviour
{

    public static Action OnAnimate;
    
    public int Id;
    public MineType MineType;
    private MeshRenderer _renderer;
    public int currentDurability;
    [HideInInspector] public Color BlockColor;

    [SerializeField] private Texture[] _textures;
    private int _textureId;

    public void Initialize(int id, int durability, Color color)
    {
        Id = id;
        currentDurability = MineType == MineType.Stone ? durability : 1;
          BlockColor = color;
        _renderer = GetComponent<MeshRenderer>();
       if (_renderer != null) _renderer.material.color = color;
    }
    public bool Break(int damage)
    {
        currentDurability -= damage;
        bool broken = currentDurability <= 0;
        if (broken)
        {
            Broken();
        }
        return broken;
    }

    public void Broken()
    {
        CheckAbove();
       // PlayerPrefs.SetInt(LevelManager.Instance.currentLevel +"minestone" + Id, 1);
        Destroy(gameObject);
    }

    private void CheckAbove()
    {
        Collider[] colls = Physics.OverlapBox(transform.position + new Vector3(0, 20, 0), new Vector3(1, 40, 1));
        foreach (Collider col in colls)
        {
            if (col.CompareTag("MineStone"))
            {
                if (!col.transform.GetComponent<Rigidbody>())
                {
                    Rigidbody rgd = col.gameObject.AddComponent<Rigidbody>();
                    rgd.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;
                }

            }
        }
    }

    public void UpdateTexture()
    {
        if (_textureId >= _textures.Length) return;
        Texture tex = _textures[_textureId];
        _renderer.material.SetTexture("_MainTex", tex);
        _textureId++;
    }

    private void OnEnable()
    {
        OnAnimate += Animate;
    }

    private void OnDisable()
    {
        OnAnimate -= Animate;
    }

    private void Animate()
    {
        // StartCoroutine(AnimateAction(transform.position));
    }
    
}
