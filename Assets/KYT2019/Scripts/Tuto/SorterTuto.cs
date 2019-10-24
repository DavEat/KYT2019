using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SorterTuto : Sorter
{
    public int step = 0;
    public UnityEvent selectionEvent;
    public UnityEvent relocatingEvent;
    public UnityEvent diselectionEvent;

    public override void Selection()
    {
        base.Selection();
        if (step == 0)
        {
            step++;
            selectionEvent.Invoke();
        }
    }
    public override bool Locate(Node node, bool afterDrag = false)
    {
        bool b = base.Locate(node, afterDrag);
        if (step == 1 && b)
        {
            step++;
            relocatingEvent.Invoke();
        }
        return b;
    }
    public override void Diselection()
    {
        base.Diselection();
        if (step == 2)
        {
            step++;
            diselectionEvent.Invoke();
        }
    }
}
