﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : VehiculTarget
{
    public class Quantity
    {
        public int quantity;
        public int reserved;

        public Quantity(int quantity, int reserved)
        {
            this.quantity = quantity;
            this.reserved = reserved;
        }
    }

    #region Vars
    public Node[] mCrtNodes;
    public Node mCrtDoorNode;
    public Vector2 centerOffet;
    public Vector2Int[] mSize = new Vector2Int[] { Vector2Int.zero };
    public Vector2Int mDoor;

    public Dictionary<Goods.GoodsType, Quantity> mStock = new Dictionary<Goods.GoodsType, Quantity>();
    public Goods[] mNeed, mProduction;

    [SerializeField] protected GameObject m_warning = null;

    float m_nodeDiameter = 2;

    public bool m_sellProduction = true;

    [HideInInspector] public float mLastTimeResourceReceive = -1;
    [HideInInspector] public float mLastTimeResourceGetCollect = -1;

    public int construcitonCost = 10;
    public int mMaintemanceCost = 1;
    public bool constructed = false;
    #endregion
    #region MonoFunctions
    protected override void Awake()
    {
        SelectionManager.buildingPlaced += BuildingPlaced;
    }
    protected override void Start()
    {
        if (constructed)
            Init();

        //m_nodeDiameter = Grid.inst.nodeRadius * 2;
    }
    public virtual void Init()
    {
        GameManager.inst.mBuildings.Add(this);
        GameManager.inst.AddMaintenanceCost(mMaintemanceCost);

        Node node = Grid.inst.NodeFromWorldPoint(transform.position);
        Locate(node);
    }



    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float angle = transform.eulerAngles.y;

        for (int i = 0; i < mSize.Length; i++)
        {
            Vector2 vecSize = Utilities.Rotate(mSize[i] + centerOffet, angle, centerOffet) * m_nodeDiameter;
            Gizmos.DrawSphere(new Vector3(vecSize.x, 0, vecSize.y) + transform.position, .5f);
        }
        Gizmos.color = Color.green;
        Vector2 vecDoor = Utilities.Rotate(mDoor + centerOffet, angle, centerOffet) * m_nodeDiameter;
        Gizmos.DrawSphere(new Vector3(vecDoor.x, 0, vecDoor.y) + transform.position, .5f);
    }
    #endregion
    #region Functions
    protected override void SendVehiculHere(Vehicul v)
    {
        v.AssignPath(mCrtDoorNode.worldPosition);
        v.pathFinished += VehiculArrivedHere;
    }
    protected override void VehiculArrivedHere(Vehicul v)
    {
        base.VehiculArrivedHere(v);
    }
    protected virtual void BuildingPlaced()
    {
        Vector3 position;

        if (mCrtDoorNode != null)
            position = mCrtDoorNode.worldPosition;
        else
        {
            Vector2 vecDoor = Utilities.Rotate((Vector2)mDoor, transform.eulerAngles.y) * m_nodeDiameter;
            position = new Vector3(vecDoor.x + centerOffet.x, 0, vecDoor.y + centerOffet.y) * m_nodeDiameter + transform.position;
        }

        for (int i = 0; i < GameManager.inst.mExits.Length; i++)
            PathRequestManager.ResquestPath(new PathRequest(position, GameManager.inst.mExits[i].position, AccessToExit));
    }
    protected virtual void AccessToExit(Vector3[] path, bool access)
    {
        if (!access)
        {
            //display UI to inform that there is no access to exit
        }
        else
        {
            //is no access UI displayed hide it
        }

        if (m_warning != null)
            m_warning.SetActive(!access);
    }
    public override void Selection()
    {
        base.Selection();

        List<BuildingInfo.UIResourceData> needs = new List<BuildingInfo.UIResourceData>();
        for (int i = 0; i < mNeed.Length; i++)
            needs.Add(new BuildingInfo.UIResourceData(mNeed[i].type.ToString(), mNeed[i].quantity));

        List<BuildingInfo.UIResourceData> prods = new List<BuildingInfo.UIResourceData>();
        for (int i = 0; i < mProduction.Length; i++)
            prods.Add(new BuildingInfo.UIResourceData(mProduction[i].type.ToString(), mProduction[i].quantity, mProduction[i].price));

        CanvasManager.inst.mBuildingInfo.DisplayItemInfos(m_name, m_description, needs.ToArray(), prods.ToArray(), m_sellProduction);
    }
    public virtual void Relocating(Node node)
    {
        CanvasManager.inst.mBuildingInfo.HideItemInfos();

        transform.position = node.worldPosition - new Vector3(centerOffet.x * m_nodeDiameter, -1, centerOffet.y * m_nodeDiameter);
        Arrow.inst.Assign(transform, new Vector3(mDoor.x + centerOffet.x, 0, mDoor.y + centerOffet.y) * m_nodeDiameter);
        //Color = AvaibleNode(node) ? green : red;
    }
    public virtual bool Locate(Node node, bool afterDrag = false)
    {
        if (afterDrag)
            Selection();

        Arrow.inst.Unassign();

        Node[] nodes = AvaibleNodes(node);
        Node doorNode = GetNeighbourNode(mDoor, node);
        if (nodes != null && doorNode != null) //verify is nodes are available and if door node is available
        {
            if (!constructed) //cancel construction
            {
                constructed = true;
                GameManager.inst.AddMoney(-construcitonCost);                
                Init();
            }

            //if available clear previous node
            ClearNodes();
            mCrtNodes = nodes;
            mCrtDoorNode = doorNode;
            SealNodes();

            transform.position = mCrtNodes[0].worldPosition - new Vector3(centerOffet.x, 0, centerOffet.y) * m_nodeDiameter;

            SoundManager.inst.PlayBuild();

            SelectionManager.buildingPlaced.Invoke();

            return true;
        }
        else
        {
            /*if (!constructed) //cancel construction
            {
                transform.position = Vector3.one * 1000;
                Destroy(gameObject, .1f);
            }*/

            SoundManager.inst.PlayError();

            if (!constructed)
                transform.position = BuildingManager.inst.transform.position;

            if (mCrtNodes != null)
                transform.position = mCrtNodes[0].worldPosition - new Vector3(centerOffet.x, 0, centerOffet.y) * m_nodeDiameter;
        }
        return false;
    }
    protected Node GetNeighbourNode(Vector2Int relativePos, Node center)
    {
        float angle = transform.eulerAngles.y;
        Vector2Int vec = Utilities.Rotate(relativePos, angle);
        return Grid.inst.NodeFromGridPoint(center.gridX + vec.x, center.gridY + vec.y);
    }
    protected bool NodeIsInUse(Node node)
    {
        bool inUse = false;
        if (node != null)
        {
            if (mCrtNodes != null)
                for (int i = 0; i < mCrtNodes.Length; i++)
                    if (mCrtNodes[i] == node)
                    {
                        inUse = true;
                        break;
                    }
            if (mCrtDoorNode != null && !inUse)
                if (node == mCrtDoorNode)
                    inUse = true;
        }
        return inUse;
    }
    protected bool AvaibleNode(Node node)
    {
        if (node == null || !(node.walkable && node.buildable || NodeIsInUse(node)))
            return false;

        Node door = GetNeighbourNode(mDoor, node);
        if (door == null || !(door.walkable && door.buildable || NodeIsInUse(door)))
            return false;

        bool available = true;
        for (int i = 1; i < mSize.Length; i++)
        {
            Node n = GetNeighbourNode(mSize[i], node);
            if (n == null || !(n.walkable && n.buildable || NodeIsInUse(n)))
            {
                available = false;
                break;
            }
        }
        return available;
    }
    protected Node[] AvaibleNodes(Node node)
    {
        if (node == null || !(node.walkable && node.buildable || NodeIsInUse(node)))
            return null;

        Node door = GetNeighbourNode(mDoor, node);
        if (door == null || !(door.walkable && door.buildable || NodeIsInUse(door)))
            return null;

        List<Node> availables = new List<Node>();
        availables.Add(node);

        for (int i = 1; i < mSize.Length; i++)
        {
            Node n = GetNeighbourNode(mSize[i], node);
            if (n == null || !(n.walkable && n.buildable || NodeIsInUse(n)))
            {
                return null;
            }
            else availables.Add(n);
        }
        return availables.ToArray();
    }
    protected void SealNodes()
    {
        NodesInteractatcion(false);
    }
    protected void ClearNodes()
    {
        NodesInteractatcion(true);
    }
    protected void NodesInteractatcion(bool value)
    {
        if (mCrtNodes != null)
            for (int i = 0; i < mCrtNodes.Length; i++)
            {
                mCrtNodes[i].walkable = value;
                mCrtNodes[i].buildable = value;
            }
        if (mCrtDoorNode != null)
            mCrtDoorNode.buildable = value;
    }
    public Goods GetResources()
    {
        int index = Random.Range(0, mProduction.Length);
        if (mProduction.Length > 0 && mStock.ContainsKey(mProduction[index].type))
        {
            mLastTimeResourceGetCollect = Time.time;

            mStock[mProduction[index].type].quantity -= 1;
            mStock[mProduction[index].type].reserved -= 1;
            return new Goods(mProduction[index].type, 1);
        }
        return null;
    }
    public Goods GetResources(Goods.GoodsType type)
    {
        if (mProduction.Length > 0 && mStock.ContainsKey(type))
        {
            mLastTimeResourceGetCollect = Time.time;

            mStock[type].quantity -= 1;
            mStock[type].reserved -= 1;
            return new Goods(type, 1);
        }
        return null;
    }
    public void AddResources(Goods goods)
    {
        if (mStock.ContainsKey(goods.type))
        {
            mStock[goods.type].quantity += goods.quantity;
        }
        else mStock.Add(goods.type, new Quantity(goods.quantity, 0));

        CheckRecipe();

        mLastTimeResourceReceive = Time.time;
    }
    protected void CheckRecipe()
    {
        bool canCook = true;
        for (int i = 0; i < mNeed.Length; i++)
        {
            if (!mStock.ContainsKey(mNeed[i].type) || mStock[mNeed[i].type].quantity < mNeed[i].quantity)
                canCook = false;
        }
        
        if (canCook)
        {
            for (int i = 0; i < mNeed.Length; i++)
                mStock[mNeed[i].type].quantity -= mNeed[i].quantity;

            for (int i = 0; i < mProduction.Length; i++)
            {
                if (m_sellProduction && GameManager.inst.autoSellProduction)
                    GameManager.inst.AddMoney(mProduction[i].price);
                else if (mStock.ContainsKey(mProduction[i].type))
                {
                    mStock[mProduction[i].type].quantity += mProduction[i].quantity;
                }
                else mStock.Add(mProduction[i].type, new Quantity(mProduction[i].quantity, 0));
                Debug.Log("product: " + mProduction[i].type + " : " + mProduction[i].quantity);
            }
        }
    }
    public bool DontHaveNeedInStock()
    {
        for (int i = 0; i < mNeed.Length; i++)
        {
            if (!mStock.ContainsKey(mProduction[i].type) || mStock[mProduction[i].type].quantity <= 0)
                return true;
        }
        return false;
    }
    public bool HaveProductionInStock()
    {
        for (int i = 0; i < mProduction.Length; i++)
        {
            if (mStock.ContainsKey(mProduction[i].type) && mStock[mProduction[i].type].quantity > mStock[mProduction[i].type].reserved)
                return true;
        }
        return false;
    }
    public bool HaveProductionInStock(Goods.GoodsType type)
    {
        for (int i = 0; i < mProduction.Length; i++)
        {
            if (mProduction[i].type == type)
                if (mStock.ContainsKey(mProduction[i].type) && mStock[mProduction[i].type].quantity > mStock[mProduction[i].type].reserved)
                    return true;
        }
        return false;
    }
    #endregion
}
