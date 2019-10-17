using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasManager : Singleton<CanvasManager>
{
    public RectTransform mMenu = null;
    public RectTransform mPolitics = null;
    public BuildingInfo mBuildingInfo = null;
    public RectTransform mBuildingInfoSellRect = null;
    public RectTransform mBuild = null;

    public TextMeshProUGUI mCash = null;
    public TextMeshProUGUI mUpKeep = null;

    public RectTransform mLoseWin = null;
    public TextMeshProUGUI mLoseWinText = null;
    public TextMeshProUGUI mNumberOfSolarPanel = null;

    public GameObject mVehiculeDepot = null;
}
