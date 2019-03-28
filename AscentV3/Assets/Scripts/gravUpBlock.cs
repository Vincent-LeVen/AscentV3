using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravUpBlock : MonoBehaviour
{

    private Rigidbody myRigidbody;
    public float gravUpForce = 18.0f;
    public float gravUpCD = 3.0f;
    private bool onCD = false;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && onCD == false)
        {
            onCD = true;
            myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, gravUpForce , myRigidbody.velocity.z);
            StartCoroutine(GravityTimeController());
        }
    }

    IEnumerator GravityTimeController()
    {
        yield return new WaitForSecondsRealtime(gravUpCD);
        onCD = false;
        StopCoroutine(GravityTimeController());
    }
}
