using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool autoSellProduction = true;
    public float maintenanceEachSec = 10;

    public int money;
    public int maintenance = 0;

    public Transform[] mExits;

    public List<Building> mSorters = new List<Building>();
    public List<Building> mBuildings = new List<Building>();
    public List<SkyTrash> mSkyTrashs = new List<SkyTrash>();

    // who can not walkable cause of a building
    public Node[] mWalkableNode;

    public void Start()
    {
        StartCoroutine(MaintenanceLogic());

        mWalkableNode = Grid.inst.GetAllWalkableNode();
    }
    public void RemoveSkyTrash(SkyTrash trash)
    {
        mSkyTrashs.Remove(trash);
        if (mSkyTrashs.Count <= 0)
        {
            CanvasManager.inst.mBuild.gameObject.SetActive(false);
            CanvasManager.inst.mPolitics.gameObject.SetActive(false);
            CanvasManager.inst.mBuildingInfo.gameObject.SetActive(false);
            CanvasManager.inst.mLoseWin.gameObject.SetActive(true);
            CanvasManager.inst.mLoseWinText.text = "Congrats dump cleared !";
            Debug.Log("congrats dump cleared");
        }
    }
    public Node FindNearestWalkableBuildableNodePos(Vector3 position)
    {
        Node bestNode = null;
        float minDst = 9999;
        for (int i = 0; i < mWalkableNode.Length; i++)
        {
            if (mWalkableNode[i].walkable && mWalkableNode[i].buildable)
            {
                float tmp_dst = (position - mWalkableNode[i].worldPosition).sqrMagnitude;
                if (tmp_dst < minDst)
                {
                    minDst = tmp_dst;
                    bestNode = mWalkableNode[i];
                }
            }
        }
        return bestNode;
    }
    IEnumerator MaintenanceLogic()
    {
        while(true)
        {
            yield return new WaitForSeconds(maintenanceEachSec);
            AddMoney(-maintenance);
        }
    }
    public void AddMoney(int value)
    {
        money += value;
        CanvasManager.inst.mCash.text = money.ToString();

        if (money < -60)
        {
            CanvasManager.inst.mBuild.gameObject.SetActive(false);
            CanvasManager.inst.mPolitics.gameObject.SetActive(false);
            CanvasManager.inst.mBuildingInfo.gameObject.SetActive(false);
            CanvasManager.inst.mLoseWin.gameObject.SetActive(true);
            CanvasManager.inst.mLoseWinText.text = "Too bad you lose because you have too many debt";
            Debug.Log("Too bad you lose because you have too many debt");
        }
    }
    public void AddMaintenanceCost(int value)
    {
        maintenance += value;
        CanvasManager.inst.mUpKeep.text = maintenance.ToString();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
    internal static Selectable FindNearestObject(List<Selectable> objects , Vector3 position)
    {
        Selectable bestObject = null;
        float minDst = 9999;
        foreach(Selectable o in objects)
        {
            float tmp_dst = (position - o.transform.position).sqrMagnitude;
            if (tmp_dst < minDst)
            {
                minDst = tmp_dst;
                bestObject = o;
            }
        }
        return bestObject;
    }
    internal static Building FindNearestObject(List<Building> objects, Vector3 position)
    {
        Building bestObject = null;
        float minDst = 9999;
        foreach (Building o in objects)
        {
            float tmp_dst = (position - o.transform.position).sqrMagnitude;
            if (tmp_dst < minDst)
            {
                minDst = tmp_dst;
                bestObject = o;
            }
        }
        return bestObject;
    }
    internal Building FindNearestBuildingWithObjectInside(Vector3 position, Goods.GoodsType type)
    {
        Building bestObject = null;
        float minDst = 9999;
        foreach (Building o in mBuildings)
        {
            if (!o.HaveProductionInStock(type))
                continue;

            float tmp_dst = (position - o.transform.position).sqrMagnitude;
            if (tmp_dst < minDst)
            {
                minDst = tmp_dst;
                bestObject = o;
            }
        }
        return bestObject;
    }
    internal SkyTrash FindNearestSkyTrash(Vector3 position, Action action)
    {
        SkyTrash bestObject = null;
        float minDst = 9999;
        {
            foreach (SkyTrash o in mSkyTrashs)
            {
                if (o.m_vehiculs.Count > 0) continue;

                float tmp_dst = (position - o.transform.position).sqrMagnitude;
                if (tmp_dst < minDst)
                {
                    minDst = tmp_dst;
                    bestObject = o;
                }
            }
        };
        return bestObject;
    }


    internal Building FindLonguestWaitingBuilding()
    {
        Building bestObject = null;
        float maxTime = -2;
        foreach (Building o in mBuildings)
        {            
            if (o.mLastTimeResourceGetCollect > maxTime)
            {
                maxTime = o.mLastTimeResourceGetCollect;
                bestObject = o;
            }
        }
        return bestObject;
    }
    internal Building FindLonguestWaitingBuilding(Goods.GoodsType type)
    {
        Building bestObject = null;
        float maxTime = -2;
        foreach (Building o in mBuildings)
        {
            if (o is Sorter) continue;

            for (int i = 0; i < o.mNeed.Length; i++)
            {
                if (o.mNeed[i].type == type)
                    if (o.mLastTimeResourceReceive > maxTime)
                    {
                        maxTime = o.mLastTimeResourceReceive;
                        bestObject = o;
                    }
            }
        }
        return bestObject;
    }

    public void ReloadScene()
    {
        //Scene scene = SceneManager.GetActiveScene();
        //SceneManager.LoadScene(scene.name);
        Application.Quit();
    }
}
