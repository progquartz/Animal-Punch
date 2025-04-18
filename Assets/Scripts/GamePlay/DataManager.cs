using UnityEngine;

public class DataManager : SingletonBehaviour<DataManager>
{
    public MapDataStorage MapDataStorage;
    public EnemyDataStorage EnemyDataStorage;
    public LootingDataStorage LootingStorage;
    public CSVLoader CSVLoader;

    private void Start()
    {
        CSVLoader = new CSVLoader();
        CSVLoader.Init(this);
    }
}
