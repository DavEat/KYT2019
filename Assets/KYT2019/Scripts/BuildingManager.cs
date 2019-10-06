using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : Singleton<BuildingManager>
{
    public void CreateBuilding(Building prefab)
    {
        if (GameManager.inst.money > 0)
        {
            Building b = Instantiate(prefab, transform.position, Quaternion.identity);
            SoundManager.inst.PlayButton();
        }
        else SoundManager.inst.PlayError();
    }
}
