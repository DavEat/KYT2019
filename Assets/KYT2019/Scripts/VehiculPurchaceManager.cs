using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiculPurchaceManager : MonoBehaviour
{
    public void CreateVehicul(Vehicul prefab)
    {
        if (GameManager.inst.money > 0)
        {
            Vehicul v = Instantiate(prefab, transform.position, transform.rotation);
            SoundManager.inst.PlayButton();
        }
        else SoundManager.inst.PlayError();
    }
}
