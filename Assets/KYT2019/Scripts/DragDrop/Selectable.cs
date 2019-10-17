using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    #region Vars
    public bool selected { get { return SelectionManager.selection == this; } }

    [SerializeField] protected string m_name = "", m_description = "";
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
        CanvasManager.inst.mBuildingInfo.DisplayItemInfos(m_name, m_description, null, null, true, false);

        SoundManager.inst.PlaySelection();
    }
    public virtual void Diselection()
    {
        //print(name + " diselect");
        CanvasManager.inst.mBuildingInfo.HideItemInfos();

        SoundManager.inst.PlayUnselection();
    }
    #endregion
}
