using System;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : SingletonBehaviour<DataManager>
{
    private MapDataStorage _mapDataStorage;
    private EnemyDataStorage _enemyDataStorage;
    private LootingDataStorage _lootingDataStorage;
    private SoundDataStorage _soundDataStorage;
    private PlayerDataStorage _playerDataStorage;

    public MapDataStorage MapDataStorage 
    { get { if (_mapDataStorage == null)
                LoadStorageDatas();
            return _mapDataStorage; }}
    public EnemyDataStorage EnemyDataStorage
    { get { if (_enemyDataStorage == null)
                LoadStorageDatas();
            return _enemyDataStorage; }}
    public LootingDataStorage LootingStorage
    { get { if (_lootingDataStorage == null)
                LoadStorageDatas();
            return _lootingDataStorage; }}
    public SoundDataStorage SoundStorage
    { get { if (_soundDataStorage == null)
                LoadStorageDatas();
    return _soundDataStorage; }}
    public PlayerDataStorage PlayerDataStorage
    { get { if (_playerDataStorage == null)
                LoadStorageDatas();
    return _playerDataStorage; }}

    public CSVLoader CSVLoader;


    protected override void Init()
    {
        base.Init();
        LoadCSV();
        LoadStorageDatas();
        InitStorages();
        CheckingResource();

        Debug.Log("DataManager Init");
    }

    private void LoadCSV()
    {
        CSVLoader = new CSVLoader();
        CSVLoader.Init(this);
    }

    private void LoadStorageDatas()
    {
        GetStorageData<MapDataStorage>(ref _mapDataStorage);
        GetStorageData<EnemyDataStorage>(ref _enemyDataStorage);
        GetStorageData<LootingDataStorage>(ref _lootingDataStorage);
        GetStorageData<SoundDataStorage>(ref _soundDataStorage);
        GetStorageData<PlayerDataStorage>(ref _playerDataStorage);
    }

    
    private void GetStorageData<T>(ref T target) where T : DataStorage
    {
        Type uiType = typeof(T);

        if(target == null)
        {
            var prefab = Resources.Load<DataStorage>($"Prefabs/DataStorage/{uiType}");
            DataStorage instance = Instantiate(prefab, gameObject.transform);
            target = instance.GetComponent<T>();
        }

    }
    
    private void InitStorages()
    {
        EnemyDataStorage.Init();
    }

    private void CheckingResource()
    {
        MapDataStorage.CheckResource();
        EnemyDataStorage.CheckResources();
        LootingStorage.CheckResources();
        SoundStorage.CheckResources();
        PlayerDataStorage.CheckResources();    
    }
}
