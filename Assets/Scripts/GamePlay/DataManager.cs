using UnityEngine;

public class DataManager : SingletonBehaviour<DataManager>
{
    public MapDataStorage MapDataStorage;
    public EnemyDataStorage EnemyDataStorage;
    public LootingDataStorage LootingStorage;
    public CSVLoader CSVLoader;

    protected override void Init()
    {
        base.Init();
        LoadCSV();
    }

    private void LoadCSV()
    {
        CSVLoader = new CSVLoader();
        CSVLoader.Init(this);
    }
}
