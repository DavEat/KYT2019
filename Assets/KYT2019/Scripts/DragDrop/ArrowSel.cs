using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSel : MonoBehaviour
{
    [SerializeField] Transform[] squares = null;
    [SerializeField] Transform vehicul = null;

    public void Assign(Building b)
    {
        if (b.mSize.Length == 1)
        {
            squares[0].gameObject.SetActive(true);
            squares[1].gameObject.SetActive(false);
            squares[2].gameObject.SetActive(false);
        }
        else
        {
            squares[0].gameObject.SetActive(false);
            if (b.mSize.Length == 2)
            {
                //squares[1].localPosition = new Vector3 (b.centerOffet.x, 0, b.centerOffet.y);

                squares[1].gameObject.SetActive(true);
                squares[2].gameObject.SetActive(false);
            }
            else if(b.mSize.Length == 4)
            {
                squares[1].gameObject.SetActive(false);
                squares[2].gameObject.SetActive(true);
            }
        }

        transform.parent = b.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        gameObject.SetActive(true);
    }
    public void Assign(Vehicul v)
    {
        squares[0].gameObject.SetActive(false);
        squares[1].gameObject.SetActive(false);
        squares[2].gameObject.SetActive(false);

        vehicul.gameObject.SetActive(true);

        transform.parent = v.transform;
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(true);
    }
    public void Unsign(bool isVehicul = false)
    {
        gameObject.SetActive(false);
        transform.parent = null;
        if (isVehicul)
        {
            vehicul.gameObject.SetActive(false);
            //transform.rotation = Quaternion.identity;
        }
    }
}
