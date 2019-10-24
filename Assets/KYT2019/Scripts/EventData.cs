using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventData : MonoBehaviour
{
    public UnityEvent mEvent;

    public void Invoke()
    {
        mEvent.Invoke();
    }
}
