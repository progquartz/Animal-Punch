using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum GameEventType
{
    OnDash,
    OnFeverEnd,
    OnFeverStart,
    OnLevelUp,
}


[System.Serializable]
public class TutorialTrigger
{
    public GameEventType eventType;
    public int triggerCount;
    public string tutorialPageKey;
    [SerializeField]
    private int currentCount;
    [SerializeField]
    private bool isTriggered = false;

    public void TriggerEvent()
    {
        if(isTriggered) return;

        triggerCount++;
        if(triggerCount >= currentCount)
        {
            isTriggered = true;
            if (tutorialPageKey != null)
            {
                SpawnUI();
            }
        }
    }

    public void SpawnUI()
    {
        // uimanager에서 소환.
    }
}

public class TutorialManager : SingletonBehaviour<TutorialManager>
{
    private bool isRegisteredEvent = false;
    private bool isTutorialNeedSkip = false;
    public List<TutorialTrigger> triggers;

    protected override void Awake()
    {
        IsDestroyOnLoad = true;
        base.Awake();
        // 만약 플레이어가 튜토리얼 시청 이력이 있다!
        // 그러면 이벤트를 register 할 이유조차 없음.

        /// 현재 튜토리얼이 필요 없는 것 같아서 대기.
        if(true)
        {
            isRegisteredEvent = false;
            isTutorialNeedSkip = true;
            return;
        }

        // 아래부터 제대로 된 코드.
        if (IsTutorialDone())
        {
            isRegisteredEvent = false;
            isTutorialNeedSkip = true;
            return;
        }


        // 튜토리얼이 진행된 적이 없다면, 이벤트 등록.
        RegisterEvent();

    }


    public void OnPressSkipTutorialButton()
    {
        isTutorialNeedSkip = true;
        
        ReleaseEvent();
    }

    public void SetTutorialDone(bool isDone)
    {
        PlayerPrefs.SetInt("IsTutorialDone", isDone ? 1 : 0);
        PlayerPrefs.Save();
    }

    public bool IsTutorialDone()
    {
        if(PlayerPrefs.GetInt("IsTutorialDone", 0) == 0)
        {
            return false;
        }
        else 
        {
            return true;
        }
    }
    

    private void RegisterEvent()
    {
        isRegisteredEvent = true;
        Player.Instance.OnDash += OnDash;
        Player.Instance.OnFeverEnd += OnFeverEnd;
        Player.Instance.OnFeverStart += OnFeverStart;
        Player.Instance.OnLevelUp += OnLevelUp;
    }

    private void ReleaseEvent()
    {
        isRegisteredEvent = false;
        Player.Instance.OnDash -= OnDash;
        Player.Instance.OnFeverEnd -= OnFeverEnd;
        Player.Instance.OnFeverStart -= OnFeverStart;
        Player.Instance.OnLevelUp -= OnLevelUp;
    }

    private void OnTriggerEvent(GameEventType eventType)
    {
        foreach(TutorialTrigger trigger in triggers)
        {
            if(trigger.eventType == eventType)
            {
                trigger.TriggerEvent();
            }
        }
    }

    private void OnDash()
    {
        OnTriggerEvent(GameEventType.OnDash);
    }

    private void OnFeverEnd()
    {
        OnTriggerEvent(GameEventType.OnFeverEnd);
    }

    private void OnFeverStart()
    {
        OnTriggerEvent(GameEventType.OnFeverStart);
    }

    private void OnLevelUp()
    {
        OnTriggerEvent(GameEventType.OnLevelUp);
    }

    

    private void OnDestroy()
    {
        ReleaseEvent();
    }



}
