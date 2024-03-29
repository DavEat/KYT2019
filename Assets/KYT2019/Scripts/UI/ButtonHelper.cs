﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHelper : MonoBehaviour, ISelectHandler, ISubmitHandler
{
    public bool upgrade = false;

    public virtual void OnSelect(BaseEventData eventData)
    {
        PlaySound();
    }

    public virtual void OnSubmit(BaseEventData eventData)
    {
        PlaySound();
    }

    public virtual void PlaySound()
    {
        if (upgrade)
            SoundManager.inst.PlayUpgrade();
        else SoundManager.inst.PlayButton();
    }
}
