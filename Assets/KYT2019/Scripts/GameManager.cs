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

    public int numberOfSolarPanel = 0;

    public Transform[] mExits;

    public List<Building> mSorters = new List<Building>();
    public List<Building> mBuildings = new List<Building>();
    public List<SkyTrash> mSkyTrashs = new List<SkyTrash>();

    // who can not walkable cause of a building
    public Node[] mWalkableNode;

    public Arrow[] doorArrows;
    public ArrowDst destinationArrow;
    public ArrowSel selectionArrow;

    public VehiculPurchaceManager tractor;
    public VehiculPurchaceManager truck;

    public bool tuto = false;

    public void Start()
    {
        mWalkableNode = Grid.inst.GetAllWalkableNode();

        StartCoroutine(MaintenanceLogic());
    }
    public void RemoveSkyTrash(SkyTrash trash)
    {
        mSkyTrashs.Remove(trash);
        if (!tuto && mSkyTrashs.Count <= 0)
        {
            GameFinish("Congrats dump cleared !");
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

        if (!tuto && money < -60)
        {
            GameFinish("Too bad you lose because you have too many debt");
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
        {
            if (Time.timeScale == 0 && m_pauseMenu.activeSelf)
            {
                m_pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
            else if (!m_mainMenu.activeSelf)
            {
                m_pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
    internal static Selectable FindNearestObject(List<Selectable> objects , Vector3 position)
    {
        Selectable bestObject = null;
        float minDst = 9999;
        lock (objects)
        {

            foreach (Selectable o in objects)
            {
                float tmp_dst = (position - o.transform.position).sqrMagnitude;
                if (tmp_dst < minDst)
                {
                    minDst = tmp_dst;
                    bestObject = o;
                }
            }
        }
        return bestObject;
    }
    internal static Building FindNearestObject(List<Building> objects, Vector3 position)
    {
        Building bestObject = null;
        float minDst = 9999;
        lock (objects)
        {
            foreach (Building o in objects)
            {
                //if (!o.constructed) continue;

                float tmp_dst = (position - o.transform.position).sqrMagnitude;
                if (tmp_dst < minDst)
                {
                    minDst = tmp_dst;
                    bestObject = o;
                }
            }
        }
        return bestObject;
    }
    internal Building FindNearestBuildingWithObjectInside(Vector3 position, Goods.GoodsType type)
    {
        Building bestObject = null;
        float minDst = 9999;
        lock (mBuildings)
        {
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
        }
        return bestObject;
    }
    internal SkyTrash FindNearestSkyTrash(Vector3 position)
    {
        SkyTrash bestObject = null;
        float minDst = 9999;
        lock(mSkyTrashs)
        {
            //try
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
                if (bestObject == null)
                    foreach (SkyTrash o in mSkyTrashs)
                    {
                        if (o.mNumberOfTrash <= o.mReservedTrash) continue;

                        float tmp_dst = (position - o.transform.position).sqrMagnitude;
                        if (tmp_dst < minDst)
                        {
                            minDst = tmp_dst;
                            bestObject = o;
                        }
                    }
            }
            //catch(Exception e) { Debug.Log(e.Message); }
        };
        return bestObject;
    }


    internal Building FindLonguestWaitingBuilding()
    {
        Building bestObject = null;
        float maxTime = -2;
        lock (mBuildings)
        {
            foreach (Building o in mBuildings)
            {
                if (o.mLastTimeResourceGetCollect > maxTime)
                {
                    maxTime = o.mLastTimeResourceGetCollect;
                    bestObject = o;
                }
            }
        }
        return bestObject;
    }
    internal Building FindLonguestWaitingBuilding(Goods.GoodsType type)
    {
        Building bestObject = null;
        float maxTime = -2;
        lock (mBuildings)
        {
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
        }
        return bestObject;
    }
    public void GameFinish(string message)
    {
        CanvasManager.inst.mBuild.gameObject.SetActive(false);
        CanvasManager.inst.mPolitics.gameObject.SetActive(false);
        CanvasManager.inst.mBuildingInfo.gameObject.SetActive(false);
        CanvasManager.inst.mVehiculeDepot.gameObject.SetActive(false);
        CanvasManager.inst.mLoseWin.gameObject.SetActive(true);
        CanvasManager.inst.mLoseWinText.text = message;
        Debug.Log(message);
        CanvasManager.inst.mNumberOfSolarPanel.text = string.Format("You built {0} solar panels", numberOfSolarPanel);
    }
    public void ReloadScene()
    {
        DumbTruck.commingTime = 5;
        SelectionManager.selection = null;
        SelectionManager.buildingPlaced = null;
        Time.timeScale = 1;

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void GoMainMenu()
    {
        DumbTruck.commingTime = 5;
        SelectionManager.selection = null;
        SelectionManager.buildingPlaced = null;
        Time.timeScale = 1;

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(0);
    }

    [SerializeField] GameObject m_trashTruck = null;
    [SerializeField] GameObject m_mainMenu = null;
    [SerializeField] GameObject m_pauseMenu = null;
    public void StartGame()
    {
        Time.timeScale = 1;
        m_trashTruck.SetActive(true);
    }
    public void LoadTutorail()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    public void Resume()
    {
        Time.timeScale = 1;
    }
    public void Quit()
    {
        Application.Quit();
    }
}
