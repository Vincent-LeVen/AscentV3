using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{

    public GameObject LightExt;
    public GameObject LightInt;
    public GameObject LightTuto;
    public GameObject Wall;

    // Start is called before the first frame update
    void Start()
    {
        LightTuto.SetActive(true);
        LightExt.SetActive(false);
        LightInt.SetActive(false);
        Wall.SetActive(false);
    }

    private void OnTriggerEnter(Collider coll)
    {

        if (coll.gameObject.name == "LightTrigger")
        {
            LightTuto.SetActive(false);
            LightExt.SetActive(false);
            LightInt.SetActive(true);
            Wall.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.name == "LightTrigger")
        {
            LightTuto.SetActive(false);
            LightExt.SetActive(true);
            LightInt.SetActive(false);
            Wall.SetActive(false);
        }
    }
}
