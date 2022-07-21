using System.Collections;
using UnityEngine;

public class Bulldozer : MonoBehaviour
{

    [SerializeField] private Transform bulldozerBlade;
    [SerializeField] private Transform bottomDigger;
    [SerializeField] private Animation anim;

    private bool _isBusy;
    [HideInInspector] public bool initialExcavation;


    private int machineLevel;
    private float machineScale;
    private int movementSpeed;
    private int maxRollingTime;

    private Coroutine digCoroutine;

    private PlayerController playerController;
    private PlayerFXController fxController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        fxController = GetComponent<PlayerFXController>();
    }
    private void Start()
    {
        InitProperties();

    }

    public void InitProperties()
    {
        machineLevel = DataManager.Instance.MachineLevel + 1;

        machineScale = 1 + UpgradeManager.Instance.GetFeature("Size").CurrentLevel * .15f;
        transform.localScale = Vector3.one * machineScale;
        CameraManager.Instance.UpdateCamera(machineLevel);

        movementSpeed = UpgradeManager.Instance.GetFeature("Speed").CurrentLevel;
        playerController.speed = 6.5f + movementSpeed * .75f;

        //  maxRollingTime = UpgradeManager.Instance.GetFeature("Rolling Time").CurrentLevel * 10;
        //        currentTime = maxRollingTime;

        PlayerCosmetic.Instance.UpdateChildObjects();

        anim.Play();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("MineStone"))
            Dig(other.transform.GetComponent<MineStone>());
    }


    private void Update()
    {
        if (!GameManager.Instance.IsPlaying()) return;

        fxController.TurnBlade(movementSpeed);

        test();
    }

    #region test
    //------------ for test

    private void test()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F))
        {
            UpUpUpUp();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {

            UIManager.Instance.ShopActivate(true);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            ShopManager.Instance.BuyNextItem();
        }
#endif
    }

    public void UpUpUpUp()
    {
        DataManager.Instance.MachineLevel += 2;
        machineLevel = DataManager.Instance.MachineLevel + 1;
        movementSpeed += 2;
    }

    //------------
    #endregion


    public void Dig(MineStone ms)
    {
        if (digCoroutine == null)
        {
            digCoroutine = StartCoroutine(DigEnum(ms));
            fxController.ShakeBulldozer(movementSpeed, .25f);
        }
    }

    private IEnumerator DigEnum(MineStone ms)
    {
        if (_isBusy) yield break;

        ms.UpdateTexture();
        bool b = ms.Break(machineLevel);
        if (b)
        {
            switch (ms.MineType)
            {
                case MineType.Stone:
                    PoolManager.Instance.GetCrushParticle(ms.transform.position, ms.BlockColor);
                    PoolManager.Instance.GetBlockUIFx(ms.transform.position, ms.BlockColor);

                    DataManager.Instance.AddBlock(1);
                    break;
                case MineType.Gem:
                    PoolManager.Instance.GetGemUIFx(ms.transform.position);
                    DataManager.Instance.AddGem(1);
                    break;
            }

            Vibration.Vibrate(10);

            DataManager.Instance.AddToTotalBlocks(1);


            yield break;
        }


        yield return new WaitForSeconds(1);

        digCoroutine = null;
    }

    public void BottomDiggerActive(bool activate)
    {
        if (initialExcavation) return;

        if (activate != bottomDigger.gameObject.activeSelf)
            bottomDigger.gameObject.SetActive(activate);


    }

    public void Busy(bool busy)
    {
        _isBusy = busy;

        foreach (Collider col in transform.GetComponentsInChildren<Collider>())
        {
            col.enabled = !_isBusy;
        }
        playerController.SetKinematic(_isBusy);

    }

    public void GoTopOrBottom(bool top)
    {
        fxController.GoTopOrBottom(top);
    }




}
