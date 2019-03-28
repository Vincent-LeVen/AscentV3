using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Floater : MonoBehaviour
{
    private Vector3 startingPos;
    private Vector3 targetPos;
    private float timer = 0;
    [SerializeField] private float floatingRythme;
    private float tweenerID;


    void Start()
    {
        tweenerID = Random.Range(0, 90000000);
        startingPos = transform.position;
        floatingRythme = Random.Range(10, 20);
        floating();
    }

    void Update()
    {
        if (timer == floatingRythme)
        {
            DOTween.Kill(tweenerID);
            timer = 0;
            floating();
        }
    }

    void floating()
    {
        
        //targetPos = new Vector3(Random.Range(-1.0f, 1.0f) + startingPos.x, Random.Range(-1.0f, 1.0f) + startingPos.y, Random.Range(-1.0f, 1.0f) + startingPos.z);
        //DOTween.To(() => transform.position, x => transform.position = x, targetPos, floatingRythme);
        transform.DOShakePosition(floatingRythme, 20.3f, 0, 90, false, false).SetId(tweenerID);
        DOTween.To(() => timer, x => timer = x, floatingRythme, floatingRythme).SetId(tweenerID);
    }
}
