using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : Singleton<SelectionManager>
{
    #region Vars
    public static Selectable selection;

    Vector3 m_inputDown = Vector3.zero;
    bool m_inputIsDown = false;
    bool m_drag;

    [SerializeField] LayerMask m_selectionLayer = 0;
    [SerializeField] LayerMask m_groundLayer = 0;


    #endregion
    #region Events
    public delegate void BuildingPlaced();
    public static BuildingPlaced buildingPlaced;
    #endregion
    #region MonoFunctions
    void Update()
    {
        /*if (CanvasManager.inst.mMenu.gameObject.activeSelf
         && RectTransformUtility.RectangleContainsScreenPoint(CanvasManager.inst.mMenu, Input.mousePosition))
            return;*/

        if (CanvasManager.inst.mBuildingInfo.gameObject.activeSelf
         && RectTransformUtility.RectangleContainsScreenPoint(CanvasManager.inst.mBuildingInfoSellRect.GetComponent<RectTransform>(), Input.mousePosition))
            return;

        if (CanvasManager.inst.mPolitics.gameObject.activeSelf
         && RectTransformUtility.RectangleContainsScreenPoint(CanvasManager.inst.mPolitics, Input.mousePosition))
            return;

        if (CanvasManager.inst.mBuild.gameObject.activeSelf
         && RectTransformUtility.RectangleContainsScreenPoint(CanvasManager.inst.mBuild, Input.mousePosition))
            return;

        if (CanvasManager.inst.mLoseWin.gameObject.activeSelf
         && RectTransformUtility.RectangleContainsScreenPoint(CanvasManager.inst.mLoseWin, Input.mousePosition))
                    return;

        if (m_drag)
        {
            Ray ray = Camera.main.ScreenPointToRay(InputPosition());
            Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
            RaycastHit hit;
            Node node = null;

            if (Physics.Raycast(ray, out hit, 200, m_groundLayer))
            {
                node = Grid.inst.NodeFromWorldPoint(hit.point);
                ((Building)selection).Relocating(node);
                Debug.DrawLine(ray.origin, hit.point, Color.red);
            }

            /*if (Input.GetMouseButtonDown(2))
                selection.transform.Rotate(0, 90, 0);*/

            if (Input.GetMouseButtonUp(0))
            {
                print("drop");

                m_drag = false;
                m_inputIsDown = false;
                ((Building)selection).Locate(node, true);
            }
        }
        else if (ListenInput())
            OnTrySelection();
        else if (m_inputIsDown && selection is Building)
        {
            if (GetSelectableAtPosition(m_inputDown) == selection)
            {
                //StartCoroutine(CheckDrag(.0f));
                if (m_inputIsDown && m_inputDown == InputPosition() && GetSelectableAtPosition(InputPosition()) == selection)
                {
                    print("drag");

                    m_drag = true;
                    ((Building)selection).Relocating(Grid.inst.NodeFromWorldPoint(selection.transform.position));
                    SoundManager.inst.PlayMove();
                }
            }
        }

        if (Input.GetMouseButtonUp(1) && selection != null)
        {
            selection.Diselection();
            selection = null;
        }
    }
    #endregion
    #region Functions
    Vector3 InputPosition()
    {
        #if UNITY_STANDALONE || UNITY_WEBGL
        return Input.mousePosition;
        #elif UNITY_ANDROID
        return Input.touches[0].position;
        #endif
    }
    bool ListenInput()
    {
        #if UNITY_STANDALONE  || UNITY_WEBGL
        return ListenInputStandAlone();
        #elif UNITY_ANDROID
        return ListenInputAndroid();
        #else
        return false;
        #endif
    }
    #if UNITY_STANDALONE  || UNITY_WEBGL
    bool ListenInputStandAlone()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_inputDown = Input.mousePosition;
            if (selection != null)
                m_inputIsDown = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_inputIsDown = false;

            if ((Input.mousePosition - m_inputDown).sqrMagnitude < .3f) // check if the down input position is the same a the up input position
            {
                return true;
            }
            else if (selection != null)
            {
                selection.Diselection();
                selection = null;
            }
        }

        return false;
    }
    #elif UNITY_ANDROID
    bool ListenInputAndroid()
    {
        return false;
    }
    #endif
    void OnTrySelection()
    {
        if (selection != null)
            selection.Diselection();

        Selectable tmp_selection = GetSelectableAtPosition(m_inputDown);
        if (tmp_selection != null && tmp_selection is Building
         && selection != null && selection is TransportTruck && !(tmp_selection is Sorter))
        {
            ((TransportTruck)selection).AssignBuilding((Building)tmp_selection);
            selection = null;
            SoundManager.inst.PlaySend();
            return;
        }
        if (tmp_selection != null && tmp_selection is VehiculTarget
         && selection != null && selection is Vehicul && !(tmp_selection is Sorter) && !(selection is TransportTruck))
        {
            ((VehiculTarget)tmp_selection).AssignVehicul((Vehicul)selection);
            selection = null;
            SoundManager.inst.PlaySend();
            return;
        }

        if (selection is Vehicul && !((Vehicul)selection).mCanBeSelect)
        {
            selection = null;
            return;
        }

        if (tmp_selection == null && selection != null && selection is Vehicul)
        {
            Ray ray = Camera.main.ScreenPointToRay(InputPosition());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 200, m_groundLayer))
            {
                Vehicul v = ((Vehicul)selection);
                v.ClearPathFinished();
                v.ClearTargetVehicul();
                v.AssignPath(hit.point);
            }
        }

        selection = tmp_selection;
        if (selection != null)
            selection.Selection();
    }
    Selectable GetSelectableAtPosition(Vector3 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 200, m_selectionLayer))
            return hit.transform.GetComponent<Selectable>();
        return null;
    }
    IEnumerator CheckDrag(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        if (m_inputIsDown && m_inputDown == InputPosition() && GetSelectableAtPosition(InputPosition()) == selection)
        {
            print("drag");

            m_drag = true;
            ((Building)selection).Relocating(Grid.inst.NodeFromWorldPoint(selection.transform.position));
            SoundManager.inst.PlayMove();
        }
    }
    #endregion
}
