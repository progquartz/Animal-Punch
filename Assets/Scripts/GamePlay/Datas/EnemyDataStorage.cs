using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDataStorage : MonoBehaviour
{
    public GameObject MovingPrefab;
    public GameObject NotMovingPrefab;
    public List<EnemyDataSO> _enemyData;
    public Dictionary<string, EnemyDataSO> enemyDataList;
    

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        enemyDataList = new Dictionary<string, EnemyDataSO>();
        foreach(EnemyDataSO data in _enemyData)
        {
            enemyDataList.Add(data.ActorKey, data);
        }
    }

    public GameObject GetEnemyBasePrefab(string enemyKey)
    {
        if(enemyDataList.ContainsKey(enemyKey))
        {
            // enemymoving을 리턴
            if (enemyDataList[enemyKey].IsEnemyHasCondition)
            {
                return MovingPrefab;
            }
            // enemyNotMoving을 리턴
            else
            {
                return NotMovingPrefab;
            }
        }
        Logger.LogError($"{enemyKey}값을 기반으로 한 데이터가 존재하고 있지 않습니다.");
        return null;
    }

    public EnemyDataSO GetEnemyData(string enemyKey)
    {
        if (enemyDataList.ContainsKey(enemyKey))
        {
            return enemyDataList[enemyKey];
        }
        else
        {
            Init();
            if (!enemyDataList.ContainsKey(enemyKey))
            {
                // 초기화를 했음에도 정상적으로 데이터가 로드되지 않았습니다.
                Logger.LogError($"key : {enemyKey}로 정상적으로 데이터가 로드되지 않습니다.");
                return null;
            }
            else
            {
                return enemyDataList[enemyKey];
            }
        }
    }

    public bool IsEnemyMoving(string enemyKey)
    {
        if(enemyDataList.ContainsKey(enemyKey))
        {
            return enemyDataList[enemyKey].IsEnemyHasCondition;
        }
        return false;
    }
}
