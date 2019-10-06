using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoliticsManager : MonoBehaviour
{
    public float mChangeOfHavingHelp = .3f;
    public float mTimeBeforeReAskHelp = 2f;

    public int mMinPoliticalHelp = 15;
    public int mMaxPoliticalHelp = 30;

    [SerializeField] Button m_AskForHelpButton;

    public void AskForHelpMoney()
    {
        float r = Random.Range(0, 1f);
        if (r <= mChangeOfHavingHelp)
        {
            //receive help
            int money = Random.Range(mMinPoliticalHelp, mMaxPoliticalHelp);
            GameManager.inst.AddMoney(money);
        }
        else
        {
            //help refused
        }

        m_AskForHelpButton.interactable = false;
        StartCoroutine(CanAskForHelpTimer(mTimeBeforeReAskHelp));
    }

    IEnumerator CanAskForHelpTimer(float time)
    {
        yield return new WaitForSeconds(time);
        m_AskForHelpButton.interactable = true;
    }
}
