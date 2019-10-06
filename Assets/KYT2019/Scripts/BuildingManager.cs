using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : Singleton<BuildingManager>
{
    public void CreateBuilding(Building prefab)
    {
        Building b = Instantiate(prefab, transform.position, Quaternion.identity);
    }
}
