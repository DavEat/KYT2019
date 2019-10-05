using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    #region Vars
    public bool selected { get { return SelectionManager.selection == this; } }
    #endregion
    #region MonoFunctions
    protected virtual void Awake()
    {        
    }
    protected virtual void Start()
    {
    }
    protected virtual void OnDrawGizmos()
    {
    }
    #endregion
    #region Functions
    public virtual void Selection()
    {
        //print(name + " select");
    }
    public virtual void Diselection()
    {
        //print(name + " diselect");
    }
    #endregion
}
