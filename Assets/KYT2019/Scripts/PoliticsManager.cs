﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PoliticsManager : Singleton<PoliticsManager>
{
    public int mDumpTreated = 0;
    public PoliticsAction[] mUnlocableActions;
    public PoliticsAction[] mUnlocableBuildings;

    public float polactionCommingTimeAugmentation = 3;

    public void DumpTreated()
    {
        mDumpTreated++;

        for (int i = 0; i < mUnlocableActions.Length; i++)
        {
            if (mUnlocableActions[i].Unlocked()) continue;

            if (mUnlocableActions[i].dumptreatedNeeded <= mDumpTreated)
            {
                mUnlocableActions[i].Unlock();
                NewPoliticsActions();
            }
        }
        for (int i = 0; i < mUnlocableBuildings.Length; i++)
        {
            if (mUnlocableBuildings[i].Unlocked()) continue;

            if (mUnlocableBuildings[i].dumptreatedNeeded <= mDumpTreated)
            {
                mUnlocableBuildings[i].Unlock();
                NewBuildings();
            }
        }
    }
    public void NewPoliticsActions()
    {
        //UI
    }
    public void NewBuildings()
    {
        //UI
    }

    public float mChangeOfHavingHelp = .3f;
    public float mTimeBeforeReAskHelp = 2f;

    public int mMinPoliticalHelp = 15;
    public int mMaxPoliticalHelp = 30;

    [SerializeField] Button m_AskForHelpButton = null;

    public void AskForHelpMoney()
    {
        float r = Random.Range(0, 1f);
        if (r <= mChangeOfHavingHelp)
        {
            //receive help
            int money = Random.Range(mMinPoliticalHelp, mMaxPoliticalHelp);
            GameManager.inst.AddMoney(money);
            SoundManager.inst.PlayButton();
        }
        else
        {
            //help refused
            SoundManager.inst.PlayError();
        }

        m_AskForHelpButton.interactable = false;
        StartCoroutine(CanAskForHelpTimer(mTimeBeforeReAskHelp));
    }

    IEnumerator CanAskForHelpTimer(float time)
    {
        yield return new WaitForSeconds(time);
        m_AskForHelpButton.interactable = true;
    }

    [System.Serializable]
    public struct PoliticsAction
    {
        private bool unlock;
        public int dumptreatedNeeded;
        public Button action;
        public TextMeshProUGUI valueToHide;

        public bool Unlocked() { return unlock; }
        public void Unlock()
        {
            unlock = true;
            action.interactable = true;
            valueToHide.text = "";
        }
    }

    public void DoPoliticsAction()
    {
        DumbTruck.commingTime += polactionCommingTimeAugmentation;
    }
}
