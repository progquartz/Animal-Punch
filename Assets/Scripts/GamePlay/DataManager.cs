using UnityEngine;

public class DataManager : SingletonBehaviour<DataManager>
{
    public MapDataStorage MapDataStorage;
    public EnemyDataStorage EnemyDataStorage;
    public InventoryDataStorage InventoryDataStorage;
    public CSVLoader CSVLoader;

    private void Start()
    {
        CSVLoader = new CSVLoader();
        CSVLoader.Init(this);
    }
}
