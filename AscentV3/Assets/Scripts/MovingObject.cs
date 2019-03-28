using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour {

	public List<Transform> checkpoints = new List<Transform>();
	public float speed = 10.0f;

	private Transform target;
	private int indexTarget = 0;

  /*  public Rigidbody myRigidbody;
    Vector3 lastPosition = Vector3.zero;
    public float rbVel;*/

	// Use this for initialization
	void Start () 
	{
       /* myRigidbody = GetComponent<Rigidbody>();*/
	}

	// Update is called once per frame
	void Update () 
	{
		if (indexTarget == checkpoints.Count) 
		{
			indexTarget = 0;
		}

		target = checkpoints [indexTarget];

		if (transform.position == target.position) 
		{
			indexTarget++;
		}

		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, target.position, step);
	}

    /*private void FixedUpdate()
    {
        rbVel = (transform.position - lastPosition).magnitude;
        lastPosition = transform.position;
        rbVel = rbVel * 10;
        rbVel = (int)rbVel;
    }*/
}