using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasManager : Singleton<CanvasManager>
{
    public RectTransform mMenu;
    public RectTransform mPolitics;
    public BuildingInfo mBuildingInfo;
    public RectTransform mBuild;

    public TextMeshProUGUI mCash = null;
    public TextMeshProUGUI mUpKeep = null;
}
