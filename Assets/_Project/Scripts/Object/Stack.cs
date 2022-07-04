using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stack : MonoBehaviour
{

    private const float scale = .5f;
    [SerializeField] private Transform startPoint;
    private Vector3 currentPosition;

    [HideInInspector] public bool IsFull { get { return mineStones.Count == maxCount; } }
    [HideInInspector] public int maxCount;


    private List<MineStone> mineStones = new List<MineStone>();

    public void AddItem(MineStone ms)
    {
        
/*
        if (!IsFull)
        {

            ms.transform.parent = startPoint;
            ms.transform.localRotation = Quaternion.identity;

            mineStones.Add(ms);
            UpdateCount();
            StartCoroutine(StackAnimEnum(ms.transform, currentPosition));
        }
        */

    }

    public void SellItem()
    {

      /*  if (mineStones.Count != 0)
        {
            MineStone temp = mineStones[mineStones.Count - 1];
            PoolManager.Instance.GetCoinUIFx(temp.transform.position);
            DataManager.Instance.AddCoin(temp.Price);
            mineStones.Remove(temp);
            Destroy(temp.gameObject);
            UpdateCount();
        }
        */

    }

    public void UpdateCount()
    {
        int count = mineStones.Count;
        if (count == 0) return;

        float x = (count - 1) % 5;
        float z = (int)((count - 1) % 25 / 5);
        float y = (int)((count - 1) / 25);

        currentPosition = new Vector3(x, y, z) * scale;

    }


    private IEnumerator StackAnimEnum(Transform tr, Vector3 pos)
    {       
        float t = 0;
        while (t < 1 && tr !=null)
        {
            t += Time.deltaTime * 3;
            tr.localScale = Vector3.Lerp(tr.localScale, Vector3.one * scale, t);
            tr.localPosition = Vector3.Lerp(tr.localPosition, pos, t);
            yield return null;
        }
    }

}
