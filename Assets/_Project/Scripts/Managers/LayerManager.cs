using UnityEngine;

public class LayerManager : Manager<LayerManager>
{

    public float CubeScale { get { return 3f; } }
    public int BlockCountInLevel;
    LayerCreator[] creators;

    void Start()
    {
        CreateLayers(); 
    }

    private void CreateLayers()
    {
        creators = transform.GetComponentsInChildren<LayerCreator>();
        for (int i = 0; i < creators.Length; i++)
        {
            creators[i].CreateLayer();
        }
    }

    public void DestructAllLayers()
    {
        for (int i = 0; i < creators.Length; i++)
        {
            creators[i].Destruct();
        }
    }

    public void DestructFirstLayer(int percent)
    {
        creators[0].Destruct(percent);
    }
}
