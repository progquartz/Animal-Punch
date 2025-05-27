using System.Collections.Generic;
using UnityEngine;

public class CSVLoader 
{
    private DataManager owner;
    public string levelExpFilePath = "CSVData/levelEXP";
    public string titleLevelExpFilePath = "CSVData/titleLevelEXP";



    public void Init(DataManager owner)
    {
        this.owner = owner;
        LoadExpData();
        LoadTitleExpData();

    }
    public void LoadExpData()
    {
        // Resources 폴더에서 CSV 파일을 TextAsset 형태로 로드
        TextAsset csvFile = Resources.Load<TextAsset>(levelExpFilePath);
        if (csvFile == null)
        {
            Debug.LogError("CSV 파일을 찾을 수 없습니다: " + levelExpFilePath);
            return;
        }

        // CSV 파일의 텍스트를 줄 단위로 분리
        string[] lines = csvFile.text.Split(new[] { "\r\n", "\n", "\r" }, System.StringSplitOptions.RemoveEmptyEntries);

        // 각 줄을 int 형으로 파싱하여 리스트에 추가
        foreach (string line in lines)
        {
            // 앞뒤 공백 제거 후 파싱 시도
            if (int.TryParse(line.Trim(), out int exp))
            {
                owner.LootingStorage.levelExpList.Add(exp);
            }
            else
            {
                Debug.LogWarning("파싱에 실패한 라인: " + line);
            }
        }

        Debug.Log("성공적으로 " + owner.LootingStorage.levelExpList.Count + "개의 인게임 경험치 데이터를 로드했습니다.");
    }

    public void LoadTitleExpData()
    {
        // Resources 폴더에서 CSV 파일을 TextAsset 형태로 로드
        TextAsset csvFile = Resources.Load<TextAsset>(titleLevelExpFilePath);
        if (csvFile == null)
        {
            Debug.LogError("CSV 파일을 찾을 수 없습니다: " + titleLevelExpFilePath);
            return;
        }

        // CSV 파일의 텍스트를 줄 단위로 분리
        string[] lines = csvFile.text.Split(new[] { "\r\n", "\n", "\r" }, System.StringSplitOptions.RemoveEmptyEntries);

        // 각 줄을 int 형으로 파싱하여 리스트에 추가
        foreach (string line in lines)
        {
            // 앞뒤 공백 제거 후 파싱 시도
            if (int.TryParse(line.Trim(), out int exp))
            {
                owner.PlayerDataStorage.ExpDatas.Add(exp);
            }
            else
            {
                Debug.LogWarning("파싱에 실패한 라인: " + line);
            }
        }

        Debug.Log("성공적으로 " + owner.LootingStorage.levelExpList.Count + "개의 타이틀 경험치 데이터를 로드했습니다.");
    }
}
