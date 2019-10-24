using System.Collections;
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
    public Node[] mCrtDoorsNode;
    public Vector2 centerOffet;
    public Vector2Int[] mSize = new Vector2Int[] { Vector2Int.zero };
    public Vector2Int[] mDoors;

    public Dictionary<Goods.GoodsType, Quantity> mStock = new Dictionary<Goods.GoodsType, Quantity>();
    public Goods[] mNeed, mProduction;

    [SerializeField] protected GameObject m_warning = null;

    protected float m_nodeDiameter = 2;

    public bool m_sellProduction = true;

    [HideInInspector] public float mLastTimeResourceReceive = -1;
    [HideInInspector] public float mLastTimeResourceGetCollect = -1;

    public int construcitonCost = 10;
    public int mMaintemanceCost = 1;

    public bool constructed = false;

    public bool mVehiculTargettable = true;

    bool m_accessToExit;
    #endregion
    #region MonoFunctions
    protected override void Start()
    {
        if (constructed)
        {
            Init();
            Vector2 center = Utilities.Rotate(centerOffet, transform.eulerAngles.y);
            Locate(Grid.inst.NodeFromWorldPoint(transform.position + new Vector3(center.x, 0, center.y) * m_nodeDiameter));
        }

        //m_nodeDiameter = Grid.inst.nodeRadius * 2;
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
        for (int i = 0; i < mDoors.Length; i++)
        {
            Vector2 vecDoor = Utilities.Rotate(mDoors[i] + centerOffet, angle, centerOffet) * m_nodeDiameter;
            Gizmos.DrawSphere(new Vector3(vecDoor.x, 0, vecDoor.y) + transform.position, .5f);

        }
    }
    #endregion
    #region Functions
    public virtual void Init()
    {
        SelectionManager.buildingPlaced += BuildingPlaced;

        GameManager.inst.mBuildings.Add(this);
        GameManager.inst.AddMaintenanceCost(mMaintemanceCost);

        Node node = Grid.inst.NodeFromWorldPoint(transform.position);
    }
    protected override void SendVehiculHere(Vehicul v)
    {
        v.AssignPath(mCrtDoorsNode[0].worldPosition);
        v.pathFinished += VehiculArrivedHere;
    }
    protected override void VehiculArrivedHere(Vehicul v)
    {
        base.VehiculArrivedHere(v);
    }
    protected virtual void BuildingPlaced()
    {
        Vector3[] positions;

        m_accessToExit = true;
        m_warning.SetActive(false);

        /*if (mCrtDoorsNode != null)
            positions = new Vector3[] { mCrtDoorsNode[0].worldPosition };
        else*/
        {
            positions = new Vector3[mDoors.Length];
            for (int i = 0; i < mDoors.Length; i++)
            {
                Vector2 vecDoor = Utilities.Rotate((Vector2)mDoors[i], transform.eulerAngles.y);
                positions[i] = new Vector3(vecDoor.x + centerOffet.x, 0, vecDoor.y + centerOffet.y) * m_nodeDiameter + transform.position;
            }
        }
        CheckAccessToExit(positions);
    }
    protected virtual void CheckAccessToExit(Vector3[] doorsPosition)
    {
        for (int i = 0; i < GameManager.inst.mExits.Length; i++)
            for (int j = 0; j < doorsPosition.Length; j++)
            {
                PathRequestManager.ResquestPath(new PathRequest(doorsPosition[j], GameManager.inst.mExits[i].position, AccessToExit));
            }
    }
    protected virtual void AccessToExit(Vector3[] path, bool access)
    {
        if (!access)
        {
            //display UI to inform that there is no access to exit
            m_accessToExit = false;
        }
        else
        {
            //have access UI displayed hide it
        }
        if (m_warning != null && !m_accessToExit)
            m_warning.SetActive(true);
    }
    public override void Selection()
    {
        base.Selection();

        List<BuildingInfo.UIResourceData> needs = new List<BuildingInfo.UIResourceData>();
        for (int i = 0; i < mNeed.Length; i++)
        {
            Quantity stock = new Quantity(0, 0);
            mStock.TryGetValue(mNeed[i].type, out stock);
            needs.Add(new BuildingInfo.UIResourceData(Goods.GOODS_NAMES[(int)mNeed[i].type], mNeed[i].quantity, mNeed[i].price, stock == null ? 0 : stock.quantity));
        }

        List<BuildingInfo.UIResourceData> prods = new List<BuildingInfo.UIResourceData>();
        for (int i = 0; i < mProduction.Length; i++)
        {
            Quantity stock = new Quantity(0, 0);
            mStock.TryGetValue(mProduction[i].type, out stock);
            prods.Add(new BuildingInfo.UIResourceData(Goods.GOODS_NAMES[(int)mProduction[i].type], mProduction[i].quantity, mProduction[i].price, stock == null ? 0 : stock.quantity));
        }

        bool warningAccess = false;
        if (m_warning != null && m_warning.activeSelf)
        {
            m_warning.SetActive(false);
            warningAccess = true;
        }

        CanvasManager.inst.mBuildingInfo.DisplayItemInfos(m_name, m_description, needs.ToArray(), prods.ToArray(), m_sellProduction, warningAccess);
        CanvasManager.inst.seeThrough.active = !constructed;

        for (int i = 0; i < mDoors.Length; i++)
            GameManager.inst.doorArrows[i].Assign(transform, new Vector3(mDoors[i].x + centerOffet.x, 0, mDoors[i].y + centerOffet.y) * m_nodeDiameter);
        GameManager.inst.selectionArrow.Assign(this);
    }
    public override void Diselection()
    {
        if (!constructed)
            transform.position = BuildingManager.inst.transform.position;
        else
        {
            Vector2 center = Utilities.Rotate(centerOffet, transform.eulerAngles.y);
            transform.position = mCrtNodes[0].worldPosition - new Vector3(center.x, 0, center.y) * m_nodeDiameter;
        }

        base.Diselection();
        for (int i = 0; i < GameManager.inst.doorArrows.Length; i++)
            GameManager.inst.doorArrows[i].Unsign();
        GameManager.inst.selectionArrow.Unsign();

        CanvasManager.inst.seeThrough.active = !constructed;

        if (m_warning != null && !m_warning.activeSelf)
            BuildingPlaced();
    }
    public virtual void Relocating(Node node)
    {
        CanvasManager.inst.mBuildingInfo.HideItemInfos();

        Vector2 center = Utilities.Rotate(centerOffet, transform.eulerAngles.y);
        transform.position = node.worldPosition - new Vector3(center.x * m_nodeDiameter, -1, center.y * m_nodeDiameter);
        for (int i = 0; i < mDoors.Length; i++)
            GameManager.inst.doorArrows[i].Assign(transform, new Vector3(mDoors[i].x + centerOffet.x, 0, mDoors[i].y + centerOffet.y) * m_nodeDiameter);
        //Color = AvaibleNode(node) ? green : red;
    }
    public virtual bool Locate(Node node, bool afterDrag = false)
    {
        if (afterDrag)
            Selection();

        //Arrow.inst.Unsign();

        Node[] nodes = AvaibleNodes(node);
        Node[] doorsNode = new Node[mDoors.Length];
        for (int i = 0; i < mDoors.Length; i++)
            doorsNode[i] = GetNeighbourNode(mDoors[i], node);

        if (nodes != null) //verify is nodes are available and if door node is available
        {
            if (!constructed) //cancel construction
            {
                constructed = true;
                GameManager.inst.AddMoney(-construcitonCost);                
                BuildingManager.inst.WaitingBuilded();
                Init();

                CanvasManager.inst.seeThrough.active = !constructed;
            }

            //if available clear previous node
            ClearNodes();
            mCrtNodes = nodes;
            mCrtDoorsNode = doorsNode;
            SealNodes();

            Vector2 center = Utilities.Rotate(centerOffet, transform.eulerAngles.y);
            transform.position = mCrtNodes[0].worldPosition - new Vector3(center.x, 0, center.y) * m_nodeDiameter;

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
            {
                Vector2 center = Utilities.Rotate(centerOffet, transform.eulerAngles.y);
                transform.position = mCrtNodes[0].worldPosition - new Vector3(center.x * m_nodeDiameter, 0, center.y * m_nodeDiameter);
            }
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
            if (mCrtDoorsNode != null && !inUse)
                for (int i = 0; i < mCrtDoorsNode.Length; i++)
                    if (node == mCrtDoorsNode[i])
                    {
                        inUse = true;
                        break;
                    }
        }
        return inUse;
    }
    protected bool AvaibleNode(Node node)
    {
        if (node == null || !(node.walkable && node.buildable || NodeIsInUse(node)))
            return false;

        bool available = true;
        for (int i = 0; i < mDoors.Length; i++)
        {
            Node n = GetNeighbourNode(mDoors[i], node);
            if (n == null || !(n.walkable && n.buildable || NodeIsInUse(n)))
            {
                available = false;
                break;
            }
        }
        if (available)
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

        for (int i = 0; i < mDoors.Length; i++)
        {
            Node door = GetNeighbourNode(mDoors[i], node);
            if (door == null || !((door.walkable && door.buildable) || NodeIsInUse(door)))
                return null;
        }

        List<Node> availables = new List<Node>();
        availables.Add(node);

        for (int i = 1; i < mSize.Length; i++)
        {
            Node n = GetNeighbourNode(mSize[i], node);
            if (n == null || !((n.walkable && n.buildable) || NodeIsInUse(n)))
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
        if (mCrtDoorsNode != null)
            for (int i = 0; i < mCrtDoorsNode.Length; i++)
                mCrtDoorsNode[i].buildable = value;
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

            UpdateStockDisplay(true);

            return new Goods(type, 1);
        }
        return null;
    }
    public void AddResources(Goods goods)
    {
        if (goods == null)
        {
            Debug.Log("goods is null");
            return;
        }

        if (mStock.ContainsKey(goods.type))
        {
            mStock[goods.type].quantity += goods.quantity;
        }
        else mStock.Add(goods.type, new Quantity(goods.quantity, 0));

        CheckRecipe();

        mLastTimeResourceReceive = Time.time;

        UpdateStockDisplay(true);
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
                //Debug.Log("product: " + mProduction[i].type + " : " + mProduction[i].quantity);

                if (mProduction[i].type == Goods.GoodsType.SolarPanel)
                    GameManager.inst.numberOfSolarPanel++;

                UpdateStockDisplay(false);
            }
        }
    }
    public virtual bool DontHaveNeedInStock()
    {
        for (int i = 0; i < mNeed.Length; i++)
        {
            if (!mStock.ContainsKey(mNeed[i].type) || mStock[mNeed[i].type].quantity <= mNeed[i].quantity + 1)
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
    void UpdateStockDisplay(bool needsOnly)
    {
        if (!selected)
            return;

        List<BuildingInfo.UIResourceData> needs = new List<BuildingInfo.UIResourceData>();
        for (int i = 0; i < mNeed.Length; i++)
        {
            Quantity stock = new Quantity(0, 0);
            mStock.TryGetValue(mNeed[i].type, out stock);
            needs.Add(new BuildingInfo.UIResourceData(Goods.GOODS_NAMES[(int)mNeed[i].type], mNeed[i].quantity, mNeed[i].price, stock == null ? 0 : stock.quantity));
        }

        List<BuildingInfo.UIResourceData> prods = new List<BuildingInfo.UIResourceData>();
        if (needsOnly)
        {
            for (int i = 0; i < mProduction.Length; i++)
            {
                Quantity stock = new Quantity(0, 0);
                mStock.TryGetValue(mProduction[i].type, out stock);
                prods.Add(new BuildingInfo.UIResourceData(Goods.GOODS_NAMES[(int)mProduction[i].type], mProduction[i].quantity, mProduction[i].price, stock == null ? 0 : stock.quantity));
            }
        }

        CanvasManager.inst.mBuildingInfo.UpdateStockInfo(needs.ToArray(), prods.ToArray());

    }
    #endregion
}
