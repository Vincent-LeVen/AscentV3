using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunStart : MonoBehaviour
{
    public bool a;
    private Animator Anim;

    void Start()
    {
        Anim = GetComponent<Animator>();
        Anim.SetBool("Looking", true);
        StartCoroutine(Wait());
    }

    public IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(11.5f);
        Anim.SetBool("Looking", false);
        Debug.Log("isok");
        StopCoroutine(Wait());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Anim.SetBool("Walk", true);
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            Anim.SetBool("Walk", false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Anim.SetBool("Saut", true);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Anim.SetBool("Saut", false);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Anim.SetBool("Glis", true);
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            Anim.SetBool("Glis", false);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Anim.SetBool("Power", true);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            Anim.SetBool("Power", false);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Anim.SetBool("Walk2", true);
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            Anim.SetBool("Walk2", false);
        }
    }
}
