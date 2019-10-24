using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VehiculTuto : Vehicul
{
    public int step = 0;

    public UnityEvent selection;
    public Transform targetNode;
    public UnityEvent pathCompleted;
    public UnityEvent diselection;

    public override void Selection()
    {
        base.Selection();
        if (step == 0)
        {
            step++;
            selection.Invoke();
        }
    }
    public override void Diselection()
    {
        base.Diselection();
        if (step == 2)
        {
            step++;
            diselection.Invoke();
        }
    }
    protected override void PathCompleted()
    {
        base.PathCompleted();
        if (step == 1)
        {
            if (Grid.inst.NodeFromWorldPoint(transform.position) == Grid.inst.NodeFromWorldPoint(targetNode.position))
            {
                step++;
                if (selected)
                    pathCompleted.Invoke();
                else
                {
                    step++;
                    diselection.Invoke();
                }
            }
        }
    }
}
