﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateY : MonoBehaviour
{

    private Vector3 startRotation;
    public float actualRotation = 0.0f;
    public float rotateSpeed = 0.5f;
    // Use this for initialization
    void Start()
    {
        startRotation = new Vector3(gameObject.transform.rotation.eulerAngles.x, gameObject.transform.rotation.eulerAngles.y, gameObject.transform.rotation.eulerAngles.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log("start : " + startRotation);
        actualRotation += rotateSpeed;
        transform.rotation = Quaternion.Euler(startRotation.x, actualRotation, startRotation.z);
    }
}
