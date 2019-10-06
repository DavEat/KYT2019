using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorter : Building
{
    protected override void Start()
    {
        GameManager.inst.mSorters.Add(this);
        base.Start();
    }
}
