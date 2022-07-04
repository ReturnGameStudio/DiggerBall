using UnityEngine;

public class PlayerCosmetic : Manager<PlayerCosmetic>
{
    [SerializeField] private Blade[] accessories;
    [HideInInspector] public Blade CurrentAccessory;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Transform _particle;

    //--------- for test
    int tempId=1;
    public void SelectNext()
    {
        SelectAccessory(tempId );
        tempId++;
    }
    //------------


    public void SelectAccessory(int id)
    {
        
        for (int i = 0; i < accessories.Length; i++)
            accessories[i].gameObject.SetActive(false);

        CurrentAccessory = accessories[id];
        CurrentAccessory.gameObject.SetActive(true);
        meshRenderer.material.color = CurrentAccessory.BodyColor;
        UIManager.Instance.ChangeBallColorUI(CurrentAccessory.BodyColor);

        foreach (var ps in _particle.GetComponentsInChildren<ParticleSystem>())
        {
            ps.startColor = CurrentAccessory.ParticleColor;
        }

        UpdateChildObjects();
    }

    public void UpdateChildObjects()
    {
        if (CurrentAccessory == null) return;

        for (int i = 0; i < CurrentAccessory.BladeChilds.Length; i++)
            CurrentAccessory.BladeChilds[i].SetActive(false);

        int c = UpgradeManager.Instance.GetFeature("Damage").CurrentLevel;
        for (int i = 0; i < c; i++)
            CurrentAccessory.BladeChilds[i].SetActive(true);
    }
}
