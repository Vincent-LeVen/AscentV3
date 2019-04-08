using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class tutorial : MonoBehaviour
{
    private bool T1a;
    private bool T2a;
    private bool T3a;
    private bool T4a;
    private bool T5a;

    private bool T1o;
    private bool T2o;
    private bool T3o;
    private bool T4o;
    private bool T5o;

    private bool T3e;
    private bool T4e;
    private bool T5e;

    public Image T1;
    public Image T2;
    public Image T3;
    public Image T4;
    public Image T5;

    public Color TAlpha1 = Color.white;
    public Color TAlpha2 = Color.white;
    public Color TAlpha3 = Color.white;
    public Color TAlpha4 = Color.white;
    public Color TAlpha5 = Color.white;


    private void Start()
    {
        T1 = GameObject.Find("T1").GetComponent<Image>();
        T2 = GameObject.Find("T2").GetComponent<Image>();
        T3 = GameObject.Find("T3").GetComponent<Image>();
        T4 = GameObject.Find("T4").GetComponent<Image>();
        T5 = GameObject.Find("T5").GetComponent<Image>();

        TAlpha1.a = 0f;
        TAlpha2.a = 0f; 
        TAlpha3.a = 0f;
        TAlpha4.a = 0f;
        TAlpha5.a = 0f;
    }

    private void Update()
    {
            T1.color = TAlpha1;       
            T2.color = TAlpha2;
            T3.color = TAlpha3;
            T4.color = TAlpha4;
            T5.color = TAlpha5;

        
        if ((Input.GetKey(KeyCode.Z) || Input.GetAxisRaw("Vertical") > 0) && T1a && TAlpha1.a == 1f && !T1o)
        {
            T1o = true;
            TAlpha1.a = 1f;
            DOTween.To(() => TAlpha1.a, x => TAlpha1.a = x, 0f, 2f).SetId("t11");
        }

        if ((Input.GetKey(KeyCode.Space) || Input.GetButtonDown("Fire2")) && !T2o)
        {
            T2o = true;
            TAlpha2.a = 1f;
            DOTween.To(() => TAlpha2.a, x => TAlpha2.a = x, 0f, 2f).SetId("t22");
        }

        if (T3a && !T3o && T3e)
        {
            T3o = true;
            TAlpha3.a = 1f;
            DOTween.To(() => TAlpha3.a, x => TAlpha3.a = x, 0f, 2f).SetId("t33");
        }

        if (T4a && !T4o && T4e)
        {
            T4o = true;
            TAlpha4.a = 1f;
            DOTween.To(() => TAlpha4.a, x => TAlpha4.a = x, 0f, 2f).SetId("t44");
        }

        if (T5a && !T5o && T5e)
        {
            T5o = true;
            TAlpha5.a = 1f;
            DOTween.To(() => TAlpha5.a, x => TAlpha5.a = x, 0f, 2f).SetId("t55");
        }

        if (T5o == true && T5.color.a == 0f)
        {
            this.enabled = !this.enabled;
        }






        if (!T1a)
        {
            TAlpha1.a = 0.00001f;
            DOTween.To(() => TAlpha1.a, x => TAlpha1.a = x, 1f, 0.5f).SetId("t1");
            T1a = true;
        }

        if (T1.color.a <= 0.2f && T1o && !T2a && !T2o)
        {
            TAlpha1.a = 0f;
            T1o = true;
            T1.color = TAlpha1;
            TAlpha2.a = 0.00001f;
            DOTween.To(() => TAlpha2.a, x => TAlpha2.a = x, 1f, 0.5f).SetId("t2");
            T2.color = TAlpha2;
            T2a = true;
        }

        if (T1o && T2o && !T3a && !T3o)
        {
            TAlpha1.a = 0f;
            TAlpha2.a = 0f;
            T1.color = TAlpha1;
            T2.color = TAlpha1;
            TAlpha3.a = 0.00001f;
            DOTween.To(() => TAlpha3.a, x => TAlpha3.a = x, 1f, 0.5f).SetId("t3");
            T3.color = TAlpha3;
            T3a = true;
        }
    }

    private void OnTriggerEnter(Collider coll)
    {

        if (coll.gameObject.name == "T4" && !T4a)
        {
            TAlpha1.a = 0f;
            TAlpha2.a = 0f;
            TAlpha3.a = 0f;
            T1.color = TAlpha1;
            T2.color = TAlpha2;
            T3.color = TAlpha3;
            TAlpha4.a = 0.00001f;
            DOTween.To(() => TAlpha4.a, x => TAlpha4.a = x, 1f, 0.5f).SetId("t4");
            T4.color = TAlpha4;
            T4a = true;
        }

        if (coll.gameObject.name == "T5" && !T5a)
        {
            TAlpha5.a = 0.00001f;
            TAlpha4.a = 0f;
            T4.color = TAlpha4;
            DOTween.To(() => TAlpha5.a, x => TAlpha5.a = x, 1f, 0.5f).SetId("t5");
            T5.color = TAlpha5;
            T5a = true;
        }

        if (coll.gameObject.name == "T3e")
        {
            DOTween.Kill("t1");
            DOTween.Kill("t2");
            DOTween.Kill("t3");
            DOTween.Kill("t11");
            DOTween.Kill("t22");
            T3a = true;
            T3e = true;
            T3o = false;
            T1o = true;
            T2o = true;

            TAlpha1.a = 0f;
            TAlpha2.a = 0f;
        }

        if (coll.gameObject.name == "T4e")
        {
            T4e = true;
        }

        if (coll.gameObject.name == "T5e")
        {
            T5e = true;
        }

    }
}
