using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicul : Selectable
{
    #region Vars
    [SerializeField] protected float m_movementSpeed = 1;
    [SerializeField] protected float m_rotationSpeed = 1;
    [SerializeField] protected float m_stoppingDst = 1;

    protected Path m_path;
    protected Vector3 m_destination;
    protected bool m_moving = false;

    [HideInInspector] public VehiculTarget mVehiculTarget;

    [HideInInspector] public Building mBuilding;
    #endregion
    #region Events
    public delegate void PathFinished(Vehicul v);
    public PathFinished pathFinished;
    public object locker = new object();
    #endregion
    #region MonoFunctions

    #endregion
    #region Functions
    public override void Selection()
    {
        base.Selection();

        if (m_moving)
            ArrowDst.inst.Assign(m_destination);
    }
    public override void Diselection()
    {
        base.Diselection();

        ArrowDst.inst.Unassign();
    }
    public virtual void AssignPath(Vector3 position, Building b = null)
    {
        PathRequestManager.ResquestPath(new PathRequest(transform.position, position, OnPathFound));
        mBuilding = b;
    }
    public void OnPathFound(Vector3[] wayPoints, bool pathSuccessfull)
    {
        if (pathSuccessfull)
        {
            int turnDst = 1;
            m_destination = wayPoints[wayPoints.Length - 1];
            m_path = new Path(wayPoints, transform.position, turnDst, m_stoppingDst);
            m_moving = true;

            StopCoroutine("MoveTo");
            StartCoroutine(MoveTo(wayPoints));

            /*StopCoroutine("FollowPath");
            StartCoroutine(FollowPath());*/
        }
    }
    IEnumerator MoveTo(Vector3[] path)
    {
        int pathCrtIndex = 0;
        Transform t = transform;
        Vector3 crtDirection = (path[0] - t.position).normalized;
        transform.LookAt(path[0]);

        while (pathCrtIndex != -1)
        {
            if ((path[pathCrtIndex] - t.position).sqrMagnitude < .2f)
            {
                pathCrtIndex++;
                if (pathCrtIndex >= path.Length)
                    pathCrtIndex = -1;
                else
                {
                    Quaternion targetRotation = Quaternion.LookRotation(path[pathCrtIndex] - t.position);
                    t.rotation = targetRotation;
                    crtDirection = (path[pathCrtIndex] - t.position).normalized;
                }
            }
            else
            {
                t.position = Vector3.MoveTowards(t.position, path[pathCrtIndex], Time.deltaTime * m_movementSpeed);
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
                if (pathIndex >= m_path.slowDownIndex && m_stoppingDst > 0)
                {
                    speedPercent = Mathf.Clamp01(m_path.turnBoundaries[m_path.finishLineIndex].DistanceFromPoint(pos2D) / m_stoppingDst);
                    if (speedPercent < 0.01f)
                        followingPath = false;
                }

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
        print("pathfinish");
        m_moving = false;
        ArrowDst.inst.Unassign();

        try
        {
            pathFinished.Invoke(this);
        }
        catch (System.Exception e) { print(e.ToString()); }
    }
    public void ClearPathFinished()
    {
        StopCoroutine("MoveTo");

        pathFinished = null;
    }
    #endregion
}
