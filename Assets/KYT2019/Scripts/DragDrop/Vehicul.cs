using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Vehicul : Selectable
{
    #region Vars
    public bool mCanBeSelect = true;

    [SerializeField] protected float m_movementSpeed = 1;
    [SerializeField] protected float m_rotationSpeed = 1;
    [SerializeField] protected float m_stoppingDst = 1;

    protected Path m_path;
    protected Vector3 m_destination;
    protected bool m_moving = false;

    Vector3[] m_waypoints;

    [HideInInspector] public VehiculTarget mVehiculTarget;

    public Building mBuilding;

    public bool pathfinding = false;
    #endregion
    #region Events
    public delegate void PathFinished(Vehicul v);
    public PathFinished pathFinished;
    public object locker = new object();

    public int mConstrucitonCost = 10;
    public int mMaintemanceCost = 1;

    Collider m_collider;

    bool m_waitingToAccess;
    #endregion
    #region MonoFunctions
    protected override void Start()
    {
        GameManager.inst.AddMoney(-mConstrucitonCost);
        GameManager.inst.AddMaintenanceCost(mMaintemanceCost);

        m_collider = GetComponent<Collider>();
    }
    #endregion
    #region Functions
    public override void Selection()
    {
        base.Selection();

        if (m_moving)
            GameManager.inst.destinationArrow.Assign(m_destination);

        GameManager.inst.selectionArrow.Assign(this);

        m_collider.enabled = false;
    }
    public override void Diselection()
    {
        base.Diselection();

        GameManager.inst.destinationArrow.Unsign();
        GameManager.inst.selectionArrow.Unsign(true);

        m_collider.enabled = true;
    }
    public virtual void AssignPath(Vector3 position, Building b = null)
    {
        if ((position - transform.position).sqrMagnitude < m_stoppingDst * m_stoppingDst)
            PathCompleted();
        else
        {
            pathfinding = true;
            m_destination = position;
            mBuilding = b;
            PathRequestManager.ResquestPath(new PathRequest(transform.position, position, OnPathFound));
        }
    }
    protected virtual void ReassignPath()
    {
        AssignPath(m_destination, mBuilding);
    }
    public void OnPathFound(Vector3[] wayPoints, bool pathSuccessfull)
    {
        if (pathSuccessfull)
        {
            int turnDst = 1;
            m_waypoints = wayPoints;
            m_destination = wayPoints[wayPoints.Length - 1];
            m_path = new Path(wayPoints, transform.position, turnDst, m_stoppingDst);
            m_moving = true;

            if (selected)
                GameManager.inst.destinationArrow.Assign(m_destination);

            StopCoroutine("MoveTo");
            StartCoroutine("MoveTo");

            /*StopCoroutine("FollowPath");
            StartCoroutine(FollowPath());*/

            if (m_waitingToAccess)
            {
                m_waitingToAccess = false;
                SelectionManager.buildingPlaced -= ReassignPath;
            }
        }
        else if (!m_waitingToAccess)
        {
            m_waitingToAccess = true;
            SelectionManager.buildingPlaced += ReassignPath;
        }
        pathfinding = false;
    }
    IEnumerator MoveTo()
    {
        int pathCrtIndex = 0;
        Transform t = transform;
        Vector3 crtDirection = (m_waypoints[0] - t.position).normalized;
        transform.LookAt(m_waypoints[0]);

        while (pathCrtIndex != -1)
        {
            if ((m_waypoints[pathCrtIndex] - t.position).sqrMagnitude < .2f)
            {
                pathCrtIndex++;
                if (pathCrtIndex >= m_waypoints.Length)
                    pathCrtIndex = -1;
                else
                {
                    Quaternion targetRotation = Quaternion.LookRotation(m_waypoints[pathCrtIndex] - t.position);
                    t.rotation = targetRotation;
                    crtDirection = (m_waypoints[pathCrtIndex] - t.position).normalized;
                }
            }
            else
            {
                t.position = Vector3.MoveTowards(t.position, m_waypoints[pathCrtIndex], Time.deltaTime * m_movementSpeed);
            }
            yield return null;
        }
        PathCompleted();
    }
    IEnumerator FollowPath()
    {
        bool followingPath = true;
        int pathIndex = 0;
        transform.LookAt(m_path.lookPoints[0]);

        float speedPercent = 1;

        while (followingPath)
        {
            if (m_path.turnBoundaries.Length > pathIndex)
                yield return null;

            Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
            while (m_path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
            {
                if (pathIndex == m_path.finishLineIndex)
                {
                    followingPath = false;
                    break;
                }
                else pathIndex++;
            }

            if (followingPath)
            {
                Quaternion targetRotation = Quaternion.LookRotation(m_path.lookPoints[pathIndex] - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * m_rotationSpeed);
                transform.Translate(Vector3.forward * Time.deltaTime * m_movementSpeed * speedPercent, Space.Self);
            }

            yield return null;
        }
        PathCompleted();
    }
    protected virtual void PathCompleted()
    {
        //print("pathfinish");
        m_moving = false;
        if (selected)
            GameManager.inst.destinationArrow.Unsign();

        if (pathFinished != null)
            pathFinished.Invoke(this);
    }
    public void ClearPathFinished()
    {
        StopCoroutine("MoveTo");
        if (selected)
            GameManager.inst.destinationArrow.Unsign();
        m_moving = false;
        pathFinished = null;
    }
    public void ClearTargetVehicul()
    {
        if (mVehiculTarget == null) return;

        mVehiculTarget.UnAssignVehicul(this);
        mVehiculTarget = null;
    }
    #endregion
}
