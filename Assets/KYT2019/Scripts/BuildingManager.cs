using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : Singleton<BuildingManager>
{
    Building m_waitingBuild = null;
    public void CreateBuilding(Building prefab)
    {
        DestroyWaitingBuilding();

        if (GameManager.inst.money > 0)
        {
            m_waitingBuild = Instantiate(prefab, transform.position, Quaternion.identity);
            SoundManager.inst.PlayButton();
        }
        else SoundManager.inst.PlayError();
    }

    public void WaitingBuilded()
    {
        m_waitingBuild = null;
    }

    public void DestroyWaitingBuilding()
    {
        if (m_waitingBuild != null && m_waitingBuild.gameObject.activeSelf)
        {
            m_waitingBuild.gameObject.SetActive(false);
            Destroy(m_waitingBuild.gameObject);
        }
    }
}
