using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraShake : MonoBehaviour
{
    public Vector3 resetPos;
    public Vector3 shakePos;
    [HideInInspector] GameObject character;
    [HideInInspector] PlayerController playerController;
       private int shakeForce = 0;



    private void Start()
    {
        resetPos = transform.localPosition;
        character = this.transform.parent.parent.gameObject;
        playerController = character.GetComponent<PlayerController>();
    }

    public IEnumerator CamShake (float duration, float magnitude)
    {
       
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            //float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(/*x +*/resetPos.x, y + resetPos.y, resetPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        if (playerController.isSliding)
        {           
                transform.localPosition = new Vector3(resetPos.x, resetPos.y -3.22f, resetPos.z);       
        } else
        {
        transform.localPosition = resetPos;
        }

    }

    public void Update()
    {
        if (transform.localPosition == shakePos)
        {
            if (shakeForce == 1)
            {
                DOTween.To(() => transform.localPosition, x => transform.localPosition = x, resetPos, 0.1f);
            }
            else if (shakeForce == 2)
            {
                DOTween.To(() => transform.localPosition, x => transform.localPosition = x, resetPos, 0.2f);
            }
            else if (shakeForce == 3)
            {
                DOTween.To(() => transform.localPosition, x => transform.localPosition = x, resetPos, 0.4f);
            }
            else if (shakeForce == 4)
            {
                DOTween.To(() => transform.localPosition, x => transform.localPosition = x, resetPos, 0.8f);
            }



        }
    }
    
    public void camShaker ()
    {
        int fallFactor = playerController.fallStartingH - playerController.fallLandingH;

        Debug.Log(fallFactor);


        if (fallFactor < 0)
        {
            fallFactor = -fallFactor;
        }

        if (fallFactor < 4)
        {
            shakeForce = 1;
            shakePos = new Vector3(resetPos.x, resetPos.y - 0.15f, resetPos.z);
        }
        else if (fallFactor < 11)
        {
            shakeForce = 2;
            shakePos = new Vector3(resetPos.x, resetPos.y - 0.6f, resetPos.z);
        }
        else if (fallFactor < 25)
        {
            shakeForce = 3;
            shakePos = new Vector3(resetPos.x, resetPos.y - 1.1f, resetPos.z);
        }
        else
        {
            shakeForce = 4;
            shakePos = new Vector3(resetPos.x, resetPos.y - 1.7f, resetPos.z);
        }

        DOTween.To(() => transform.localPosition, x => transform.localPosition = x, shakePos, 0.05f);

    }


}
