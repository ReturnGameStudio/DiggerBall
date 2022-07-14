using System.Collections;
using UnityEngine;

public class Bulldozer : MonoBehaviour
{

    [SerializeField] private Transform bulldozerBlade;
    [SerializeField] private Transform bottomDigger;
    [SerializeField] private Animation anim;

    private bool _isDigging;
    private bool _isBusy;
    [HideInInspector]public bool initialExcavation;

    private float currentTime;

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
        //print(Color.Lerp(Color.white, Color.black, 0)); 
    }

    public void InitProperties()
    {
        machineLevel = DataManager.Instance.MachineLevel + 1;

        machineScale = 1 + UpgradeManager.Instance.GetFeature("Size").CurrentLevel * .15f;
        transform.localScale = Vector3.one * machineScale;
        CameraManager.Instance.UpdateCamera(UpgradeManager.Instance.GetFeature("Size").CurrentLevel);

        movementSpeed = UpgradeManager.Instance.GetFeature("Speed").CurrentLevel;
        playerController.speed = 6.5f + movementSpeed * .75f;

        maxRollingTime = UpgradeManager.Instance.GetFeature("Rolling Time").CurrentLevel * 10;
        currentTime = maxRollingTime;

        PlayerCosmetic.Instance.UpdateChildObjects();

        Debug.Log("Size= " + UpgradeManager.Instance.GetFeature("Size").CurrentLevel);
        Debug.Log("Speed= " + UpgradeManager.Instance.GetFeature("Speed").CurrentLevel);
        Debug.Log("Rolling Time= " + UpgradeManager.Instance.GetFeature("Rolling Time").CurrentLevel);
        print("machine level: " + machineLevel);

        anim.Play();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("MineStone")) 
            Dig(other.transform.GetComponent<MineStone>());
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("MineStone")) _isDigging = false;
    }


    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsPlaying()) return;

        fxController.TurnBlade(movementSpeed);

        //if (!_isDigging) // kazım işlemi yapılmıyorsa
            //TurnUpTheHeat(-4 * Time.deltaTime); // topu soğut (-4 sıcaklık ekleyerek soğutuyor, daha dogrusu süreyi artırıyor)


        //test();
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
        _isDigging = true;
        if (digCoroutine == null)
        { 
            digCoroutine = StartCoroutine(DigEnum(ms)); 
            fxController.ShakeBulldozer(movementSpeed, .25f);
        }
        _isDigging = false;
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

            //TurnUpTheHeat(.33f);

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

        if (!activate) _isDigging = false;
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

    private void TurnUpTheHeat(float value)
    {
        if (initialExcavation) return;

        currentTime -= value;

        if (currentTime <= 0)
        {
            currentTime = 0;

            //LEVEL FAIL
           GameManager.Instance.GameOver();
        }
        else if (currentTime > maxRollingTime) currentTime = maxRollingTime;

        float v = 1 - (currentTime / (float)maxRollingTime);
        fxController.ChangeColor(v);
    }

    public void InitialExcavation(int percent)
    {
        StartCoroutine(InitialExcavationEnum(percent));
    }
    private IEnumerator InitialExcavationEnum(int percent)
    {
        initialExcavation = true;
        Busy(false);
        playerController.AddForce(Vector3.down* 850);        

        bottomDigger.gameObject.SetActive(true);
        BoxCollider bottomCol = bottomDigger.GetComponent<BoxCollider>();
        Vector3 tempSize = bottomCol.size;

        float max = ((float)percent/100) * 7.5F;
        if (max < 3) max = 3;

        bottomCol.size = new Vector3(max,tempSize.y,max);
        yield return new WaitForSeconds(1);

        bottomCol.size = tempSize; 

        initialExcavation = false;
        yield return null;
    }

}
