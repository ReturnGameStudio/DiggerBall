using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class LayerCreator : MonoBehaviour
{
    [System.Serializable]
    private class BlockItem
    {
        public MineStone Prefab;
        public int Count;
        [HideInInspector] public int[] ItemCoords;
    }


    //----------------------------------------

    [Header("Layer Properties")]
    [SerializeField] private bool isGroundLayer;

    [Space(10)]
    public int LayerIndex;
    [SerializeField] private int layerWidth = 20;
    [SerializeField] private int layerHeight = 20;
    //public int BlockCount { get { return layerHeight * layerWidth; } }

    [Header("Block Properties")]
   /* [SerializeField]*/ private int blockDurability;
    public Color blockColor;
    [SerializeField] private Texture2D map;
    [SerializeField] private bool useMapColors;

    private bool _created;

    [Header("Prefabs")]
    [SerializeField] private MineStone mineStonePrefab;
    [Header("Block Items")]
    [SerializeField] private BlockItem[] items;


    public void CreateLayer()
    {
        if (_created) return;
        _created = true;
        StartCoroutine(CreateLayerEnum());
    }

    public int count;
    private IEnumerator CreateLayerEnum()
    {
        switch (LayerIndex)
        {
            case 0: blockDurability = 1; break;
            case 1: blockDurability = 1; break;
            case 2: blockDurability = 2; break;
            case 3: blockDurability = 3; break;
            case 4: blockDurability = 4; break;
            case 5: blockDurability = 4; break;
            case 6: blockDurability = 4; break;
            case 7: blockDurability = 5; break;
            case 8: blockDurability = 5; break;
            case 9: blockDurability = 6; break;
            case 10: blockDurability = 6; break;
            default:
                break;
        }

        float cubeScale = LayerManager.Instance.CubeScale; 
        count = LayerIndex * (layerWidth * layerHeight);

        //--------------------
        int maxBlock = layerHeight * layerWidth;
        Random.InitState(LayerIndex * 2);
        for (int i = 0; i < items.Length; i++)
        {
            BlockItem bI = items[i];
            bI.ItemCoords = new int[bI.Count];
            for (int a = 0; a < bI.ItemCoords.Length; a++)
            {
                bI.ItemCoords[a] = Random.Range(count, count + maxBlock + 1);
                
            }
            
        }
        //---------------

        MeshFilter tempGroundCube = null;
        if (isGroundLayer) tempGroundCube = GameObject.CreatePrimitive(PrimitiveType.Cube).GetComponent<MeshFilter>();

        for (int x = 0; x < layerWidth; x++)
        {
            for (int z = 0; z < layerHeight; z++)
            {
                //------------------------------
                bool createItem = false;
                MineStone itemPrefab = null;
                for (int i = 0; i < items.Length; i++)
                {
                    BlockItem bI = items[i];
                    for (int j = 0; j < bI.ItemCoords.Length; j++)
                    {
                        if (bI.ItemCoords[j] == count)
                        {
                            createItem = true;
                            itemPrefab = bI.Prefab;
                            break;
                        }
                    }
                }
                //-----------------------------

                Vector3 pos = transform.position + new Vector3(x * cubeScale, -LayerIndex * cubeScale, z * cubeScale) + new Vector3(cubeScale, 0, cubeScale);
                if (GetPixel(x, z) && !isGroundLayer)
                {
                    LayerManager.Instance.BlockCountInLevel++;

                    //if (PlayerPrefs.GetInt(LevelManager.Instance.currentLevel + "minestone" + count) != 1) //this block broken
                    // {
                    MineStone temp = Instantiate(!createItem ? mineStonePrefab : itemPrefab, pos, Quaternion.identity);
                    temp.Initialize(count, blockDurability,useMapColors ? GetPixelColor(x,z): blockColor);
                    temp.transform.localScale = Vector3.one * cubeScale;
                    temp.transform.localScale -= Vector3.one * .075f;
                        temp.transform.parent = this.transform;
                        // }
                }
                else
                {
                    GameObject blockCollider = new GameObject("collider" + x + "x" + z);
                    blockCollider.transform.localScale = Vector3.one * cubeScale ;
                    
                    blockCollider.AddComponent<BoxCollider>();
                    blockCollider.transform.position = pos;
                    blockCollider.transform.parent = this.transform;

                    if (isGroundLayer && GetPixel(x,z))
                    {
                        blockCollider.AddComponent<MeshRenderer>().material.color = Color.gray;
                        blockCollider.AddComponent<MeshFilter>().mesh = tempGroundCube.mesh;
                    }
                    else
                    {
                        blockCollider.transform.localScale = new Vector3(1, 100, 1);
                    }
                }

                count++;
            }
            yield return null;
        }

        if (tempGroundCube != null) Destroy(tempGroundCube.gameObject);

    }


    private bool GetPixel(int x, int y)
    {
        if (map == null) return true;
        int w = map.width / layerWidth;
        int h = map.height / layerHeight;
        Color c = map.GetPixel(x * w, y * h);
        return c.a != 0;
    }
    private Color GetPixelColor(int x, int y)
    {
        if (map == null) return Color.white;
        int w = map.width / layerWidth;
        int h = map.height / layerHeight;
        Color c = map.GetPixel(x * w, y * h);
        return c;
    }

    public void Destruct(int percent = 100)
    {
        if (!gameObject.activeSelf) return;
        StartCoroutine(DestructEnum(percent));
    }
    private IEnumerator DestructEnum(int percent)
    {
        yield return new WaitForSeconds(1);
        Transform[] childs = new Transform[transform.childCount];
        for (int i = 0; i < childs.Length; i++) childs[i] = transform.GetChild(i);

        float max = ((float)((float)percent / 100) * (float)childs.Length); 

        for (int i = 0; i < max; i++)
        {
            if (childs[i] != null) Destroy(childs[i].gameObject);
            if (i % 5 == 0) yield return null;
        }

        if (percent == 100) this.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        /* 
         Gizmos.color = blockColor;
          Vector3 pos = transform.position + new Vector3(layerWidth / 2, -LayerIndex, layerHeight / 2) * cubeScale;
          Gizmos.DrawWireCube(pos, new Vector3(layerWidth, .8f, layerHeight) * cubeScale);
      */
    }
}
