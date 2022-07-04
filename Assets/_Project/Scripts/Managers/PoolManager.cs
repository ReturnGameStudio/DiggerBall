using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[System.Serializable]
public class Pool
{
    public GameObject ObjectPrefab;
    [Range(1, 50)] public int ObjectCount;
    [HideInInspector] public GameObject[] Objects;
    [HideInInspector] public int Index = 0;
}

public class PoolManager : Manager<PoolManager>
{

    [Header("Pools")]
    [SerializeField] private Pool CrushParticlePool;
    [SerializeField] private Pool TextPool;
    [SerializeField] private Pool CoinUIEffectPool;
    [SerializeField] private Pool GemUIEffectPool;
    [SerializeField] private Pool BlockUIEffectPool;
    private void Awake()
    {
        base.Awake();
        CreatePool(TextPool);
        CreatePool(CoinUIEffectPool);
        CreatePool(GemUIEffectPool);
        CreatePool(BlockUIEffectPool);
        CreatePool(CrushParticlePool);
    }

    public void CreatePool(Pool pool)
    {
        pool.Objects = new GameObject[pool.ObjectCount];
        for (int i = 0; i < pool.ObjectCount; i++)
        {
            pool.Objects[i] = Instantiate(pool.ObjectPrefab);
            pool.Objects[i].transform.parent = this.transform;
            pool.Objects[i].SetActive(false);
        }
    }
    public GameObject GetObject(Pool pool, Vector3 position, Quaternion? rotation = null)
    {
        GameObject g = pool.Objects[pool.Index];
        g.transform.position = position;

        if (rotation != null)
            g.transform.rotation = rotation.Value;

        g.SetActive(true);

        // if(GameManager.Instance.GameStatus!=GameStatus.Finish)
        StartCoroutine(DeactivateObject((GameObject)g));

        pool.Index++;
        if (pool.Index > pool.ObjectCount - 1)
            pool.Index = 0;

        return g;
    }

    public void GetCrushParticle(Vector3 position, Color color)
    {
        GameObject g = GetObject(CrushParticlePool, position);

        foreach (var ps in g.transform.GetComponentsInChildren<ParticleSystemRenderer>())
        {
            ps.material.color = color;
        }
    }

    public void GetText3D(Vector3 position, string text)
    {
        GetObject(TextPool, position).GetComponentInChildren<TextMeshPro>().text = text;
    }
    public void GetCoinUIFx(Vector3 startPos)
    {
        GetObject(CoinUIEffectPool, Vector3.zero).GetComponent<CoinUI>().CoinAnimation(startPos, "CoinImage");
    }
    public void GetGemUIFx(Vector3 startPos)
    {
        GetObject(GemUIEffectPool, Vector3.zero).GetComponent<CoinUI>().CoinAnimation(startPos, "GemImage");
    }
    public void GetBlockUIFx(Vector3 startPos, Color c)
    {
        GameObject g = GetObject(BlockUIEffectPool, Vector3.zero);
        g.GetComponent<CoinUI>().CoinAnimation(startPos, "BlockImage");
       // g.GetComponent<Image>().color = c;
    }

    private IEnumerator DeactivateObject(GameObject g)
    {
        yield return new WaitForSeconds(2f);
        g.SetActive(false);
    }


}
