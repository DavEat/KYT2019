using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbTruck : Vehicul
{
    public Transform mOutsideDestination;
    public GameObject dumdPrebab;

    public static float commingTime = 5;

    Node m_targetNode;

    protected override void Start()
    {
        mCanBeSelect = false;
        mMaintemanceCost = 0;
        mConstrucitonCost = 0;

        //base.Start();

        StartCoroutine(WaitBeforeGo(0));
    }

    public void ArrivedToNode(Vehicul v)
    {
        if (!m_targetNode.buildable || !m_targetNode.walkable)
            FindDestination();
        else
        {
            Instantiate(dumdPrebab, m_targetNode.worldPosition, Quaternion.identity);
            AssignPath(mOutsideDestination.position);
            pathFinished = ArriveToStart;
        }
    }
    public void ArriveToStart(Vehicul v)
    {
        StartCoroutine(WaitBeforeGo(commingTime));
    }

    IEnumerator WaitBeforeGo(float time)
    {
        bool foundDst = false;
        while (!foundDst)
        {
            yield return new WaitForSeconds(time);
            foundDst = FindDestination();
        }
    }

    public bool FindDestination()
    {
        bool newDest = false; ;
        SkyTrash s = GameManager.inst.FindNearestSkyTrash(transform.position);
        if (s != null)
        {
            Node n = GameManager.inst.FindNearestWalkableBuildableNodePos(s.transform.position);
            if (n != null)
            {
                newDest = true;
                m_targetNode = n;
                AssignPath(m_targetNode.worldPosition);
                pathFinished = ArrivedToNode;
            }
        }
        return newDest;
    }
}
