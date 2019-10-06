using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorter : Building
{
    public override void Init()
    {
        GameManager.inst.mSorters.Add(this);
        base.Init();
    }
}
