using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;



public enum ScoreRank
{
    Bronze,
    Silver,
    Gold,
    Platinum, 
    Diamond,
    Master,
}

[System.Serializable]
public struct RankData
{
    public int minScore;
    public ScoreRank rankKey;
    public string rankName;
    public Sprite rankSprite;
}

/// <summary>
///  게임 플레이가 아닌, 외부에서 필요한 데이터들을 로드하여 사용하는 공간.
/// </summary>
public class PlayerDataStorage : DataStorage
{
    public List<int> ExpDatas;
    [SerializeField] private List<RankData> _rankDatas;


    public bool isRankDataSorted = false;

    public void CheckResources()
    {
        Debug.Log($"PlayerDataStorage - \n / ExpDatas.Count = {ExpDatas.Count}");
    }

    public List<RankData> GetAllRankData()
    {
        if(!isRankDataSorted)
        {
            _rankDatas.Sort((x, y) => x.minScore.CompareTo(y.minScore));
            isRankDataSorted=true;
        }
        return _rankDatas;
    }

    public RankData GetRankData(ScoreRank rankKey)
    {
        foreach(RankData rankData in _rankDatas)
        {
            if(rankData.rankKey == rankKey)
                return rankData;
        }
        return new RankData();
    }

    public RankData GetRankData(int score)
    {
        List<RankData> rankDatas = GetAllRankData();
        for (int i = rankDatas.Count - 1; i >= 0; i--)
        {
            if (score >= rankDatas[i].minScore)
            {
                return rankDatas[i];
            }
        }
        return new RankData();
    }
}
