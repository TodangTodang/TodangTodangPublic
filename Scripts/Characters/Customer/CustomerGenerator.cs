using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CustomerGenerator : MonoBehaviour
{
    [Header("스폰 기본정보")]
    [SerializeField] private Transform[] _spawnPos;
    [SerializeField] private Transform[] _seatPoses;
    [SerializeField] private Transform[] _exitPos;
    
    [Header("스폰 수와 시간")] 
    [SerializeField] private int _spawnAmount;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private float _spawnRandomDelay;
    [SerializeField] private float _rushHourDelay;
    [SerializeField] private float _rushHourRandomDelay;
    
    private float _currentSpawnDelay;
    private float _currentRandomDelay;
    private float _currentSelectedRandomDelay;
    private float _currentWaitingTime;

    [Header("손님 데이터")]
    [SerializeField] private int _spawnCount = 0;
    
    private CustomerDataSO _dataSO;
    private CustomerDecoDataSO[] _customerDecoDataSo;
    private List<CustomerFoodPrice> _foods;
    private List<CustomerFoodPrice> _teas;

    private WaitUntil _waitSeatOpen;
    private WaitUntil _waitSpawnDelay;

    private bool[] _isPlacedInSeat;
    private EffectManager _effectManager;

    private Action OnSpawned;
    private Action<CustomerEmotionType,List<IngredientInfoSO>,int> OnBoughted;
    private Action OnExited;
    [FormerlySerializedAs("_isCustom")] public bool _isCustomMode;
    [FormerlySerializedAs("_isDump")] public bool _isStreetGenerator=false;
    

    private void Awake()
    {
        _waitSeatOpen = new WaitUntil(() => _spawnCount < _seatPoses.Length);
        _waitSpawnDelay = new WaitUntil(isCanSpawn);
        _isPlacedInSeat = new bool[_seatPoses.Length];
    }

    private void Start()
    {
        if (_isStreetGenerator)
        {
            Init(null,null,null,false);
            Invoke(nameof(StartSpawn), 1f);
        }
    }

    private bool isCanSpawn()
    {
        if (_spawnCount == 0)
            return true;
        if (_currentSpawnDelay + _currentSelectedRandomDelay > _currentWaitingTime)
        {
            _currentWaitingTime += Time.deltaTime;
        }
        else
        {
            return true;
        }
        return false;
    }
    

    public void Init(Action OnSpawned, Action OnExited, Action<CustomerEmotionType,List<IngredientInfoSO>,int> OnBoughted, bool isCustom)
    {
        this.OnSpawned = OnSpawned;
        this.OnExited = OnExited;
        this.OnBoughted = OnBoughted;
        _isPlacedInSeat = new bool[3];

        DataManager dataManager = DataManager.Instance;
#if UNITY_EDITOR
        DebugUtil.AssertNullException(dataManager,nameof(dataManager));
#endif
       
       if(!dataManager.isInited)
            dataManager.Init();
      
        
        _foods = new List<CustomerFoodPrice>();
        _teas = new List<CustomerFoodPrice>();
        _teas.Add(null);
        _teas.Add(null);
        _teas.Add(null);
        
        MakeAvailableFoodTeaList(_foods,_teas);
        
        _dataSO = dataManager.GetDefaultData<CustomerDataSO>(Strings.DefaultDataName.CUSTOMER_DATA_SO);
    #if UNITY_EDITOR
        DebugUtil.AssertNullException(_dataSO,nameof(_dataSO));
    #endif
        _customerDecoDataSo = dataManager.GetDefaultDataArray<CustomerDecoDataSO>();
#if UNITY_EDITOR
        DebugUtil.AssertNullException(_customerDecoDataSo != null,nameof(_customerDecoDataSo));
#endif
        this._isCustomMode = isCustom;
        _currentSpawnDelay = _spawnDelay;
        _currentRandomDelay = _spawnRandomDelay;
    }

    private void MakeAvailableFoodTeaList(List<CustomerFoodPrice> foods, List<CustomerFoodPrice> teas)
    {
        GameManager gameManager = GameManager.Instance;
#if UNITY_EDITOR
        DebugUtil.AssertNullException(gameManager,nameof(gameManager));
#endif
        
        PlayerData playerData = gameManager.GetPlayerData();
#if UNITY_EDITOR
        DebugUtil.AssertNullException(playerData!=null,nameof(playerData));
#endif
        
        List<RecipeInfoData> recipe = playerData.GetInventory<RecipeInfoData>();
#if UNITY_EDITOR
        DebugUtil.AssertNullException(recipe!=null,nameof(recipe));
#endif
        foreach (var recipeInfoData in recipe)
        {
            if (recipeInfoData.Level >= 1)
            {
                switch (recipeInfoData.DefaultData.Type)
                {
                    case Enums.FoodType.Ricecake:
                        IngredientInfoSO food = recipeInfoData.DefaultData.ResultSO[0];
                        int currentPrice = recipeInfoData.DefaultData.Price[recipeInfoData.Level-1];
                        CustomerFoodPrice foodPrice = new CustomerFoodPrice(food, currentPrice);
                            
                        _foods.Add(foodPrice);
                        
                        break;
                    case Enums.FoodType.Tea:
                        IngredientInfoSO tea = recipeInfoData.DefaultData.ResultSO[0];
                        int teaCurrentPrice = recipeInfoData.DefaultData.Price[recipeInfoData.Level-1];
                        CustomerFoodPrice teaPrice = new CustomerFoodPrice(tea, teaCurrentPrice);
                        _teas.Add(teaPrice);
                        
                        IngredientInfoSO teaIce = recipeInfoData.DefaultData.ResultSO[1];
                        int teaIceCurrentPrice = recipeInfoData.DefaultData.Price[recipeInfoData.Level-1] + 50;
                        CustomerFoodPrice teaIcePrice = new CustomerFoodPrice(teaIce, teaIceCurrentPrice);
                        _teas.Add(teaIcePrice);
                        break;
                }
            }
        }
    }

    public void StartSpawn()
    {
        if(!_isCustomMode)
            StartCoroutine(SpawnOperation());
    }

    public void StopSpawn()
    {
        StopAllCoroutines();
    }

    public void StartRushHour()
    {
        _currentSpawnDelay = _rushHourDelay;
        _currentRandomDelay = _rushHourRandomDelay;
    }

    public void EndRushHour()
    {
        _currentSpawnDelay = _spawnDelay;
        _currentRandomDelay = _spawnRandomDelay;
    }

    private IEnumerator SpawnOperation()
    {
        while (_spawnAmount > _spawnCount)
        {
            Customer ai;
            SpawnCustomer(out ai);
            ai.StartCustomer(false,gameObject);
            ++_spawnCount;
            OnSpawned?.Invoke();
            _currentWaitingTime = 0;
            _currentSelectedRandomDelay = Random.Range(-_currentRandomDelay, _currentRandomDelay);
#if UNITY_EDITOR
            Debug.Log($"{_currentSpawnDelay}+{_currentRandomDelay} {_currentSelectedRandomDelay} {_currentSpawnDelay + _currentSelectedRandomDelay}");
#endif
            yield return _waitSeatOpen;
            yield return _waitSpawnDelay;
        }
        
    }

    public void SpawnCustomCustomer()
    {
        Customer ai;
        SpawnCustomer(out ai);
        ai.StartCustomer(true,gameObject);
        ++_spawnCount;
        OnSpawned?.Invoke();
    }

    private void SpawnCustomer(out Customer ai)
    {
        ai = null;
        int seatIdx = FindSeatIdx();
        if (_spawnCount > _seatPoses.Length || seatIdx == -1)
            return;

        int selectedSpawnPos = Random.Range(0, _spawnPos.Length);
        ResourceManager resourceManager = ResourceManager.Instance;
#if UNITY_EDITOR
        DebugUtil.AssertNullException(resourceManager,nameof(resourceManager));
#endif
        
        GameObject obj = resourceManager.Instantiate("Customer/CustomerBase",_spawnPos[selectedSpawnPos]);
        obj.transform.localPosition = Vector3.zero;
        ai = InitCustomerObject(obj, seatIdx);

        _isPlacedInSeat[seatIdx] = true;
    }

    private Customer InitCustomerObject(GameObject obj, int seatIdx)
    {
        Customer ai;
        if (!obj.TryGetComponent<Customer>(out ai))
        {
            ai = obj.AddComponent<Customer>();
            ai.InitComponents();
            float extraEndurance = 0;
            float extraPayRate = 0;
            GetExtraEffect(out extraEndurance, out extraPayRate);
            ai.InitCustomerData(_dataSO, extraEndurance, extraPayRate);
        }

        CustomerWear wear = null;
        if (obj.TryGetComponent<CustomerWear>(out wear))
        {
            int foodRan = Random.Range(0, _foods.Count);
            int teaRan = Random.Range(0, _teas.Count);
            int randomDecoIdx = Random.Range(0, _customerDecoDataSo.Length);
            int selectedExitPos = Random.Range(0, _exitPos.Length);
            ai.InitPos(seatIdx, _seatPoses[seatIdx].position, _exitPos[selectedExitPos].position);
        
            if(_teas[teaRan] != null)
                ai.InitChooseFood(OnBoughted, _foods[foodRan].Food,_foods[foodRan].CurrentPrice, _teas[teaRan].Food,_teas[teaRan].CurrentPrice);
            else
                ai.InitChooseFood(OnBoughted, _foods[foodRan].Food,_foods[foodRan].CurrentPrice);
            
           
            CustomerDecoDataSO decoDataSO = _customerDecoDataSo[randomDecoIdx];
            wear.SetBodyMesh(decoDataSO.BodyMesh, _customerDecoDataSo[randomDecoIdx].BodyMaterial);
            wear.SetFaceMesh(decoDataSO.FaceSet);
            for (int i = 0; i < decoDataSO.Accessories.Length; ++i)
            {
                wear.SetAccessory(decoDataSO.Accessories[i]);
            }
        }
#if UNITY_EDITOR
        else
        {

            DebugUtil.AssertNotAllocateInInspector(wear,nameof(wear));
        }
#endif

        return ai;
    }

    private void GetExtraEffect(out float extraEndurance, out float extraPayRate)
    {
        EffectManager effectManager = EffectManager.Instance;
#if UNITY_EDITOR
        DebugUtil.AssertNullException(effectManager,nameof(effectManager));
#endif
        extraEndurance = effectManager.GetCustomerEffectRate(CustomerEffectType.Endurance);
        extraPayRate = effectManager.GetCustomerEffectRate(CustomerEffectType.CustomerPayrate);
    }

    private int FindSeatIdx()
    {
        for (int i = 0; i < _isPlacedInSeat.Length; i++)
        {
            if (!_isPlacedInSeat[i])
            {
                return i;
            }
        }
        return -1;
    }

    private void OnTriggerEnter(Collider other)
    {
        Customer ai;
        if (other.TryGetComponent(out ai) && ai._spawnObject == gameObject)
        {
            if (ai.GetState() is CustomerEnterState)
                return;
                
            _isPlacedInSeat[ai.PlacedIdx] = false;
            --_spawnCount;
            OnExited?.Invoke();
            ai.AccessaryOff();
            ResourceManager.Instance.Destroy(other.gameObject);    
        }
        
    }
}
