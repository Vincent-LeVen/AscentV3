using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ascender : MonoBehaviour
{
    private GameObject Player;
    private Rigidbody playerRigidbody;
    private PlayerController playerController;
    public float AscenderForce = 12f;
    [SerializeField] private bool Alenvers = false;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerRigidbody = Player.GetComponent<Rigidbody>();
        playerController = Player.GetComponent<PlayerController>();

    }

    private void OnTriggerStay(Collider coll)
    {
        if (Alenvers)
        {
            if (coll.gameObject.tag == "Player")
            {
                playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, -AscenderForce , playerRigidbody.velocity.z);
                playerController.fallingIndicatorAlpha.a = 0f;
                playerController.fallingIndicator.color = playerController.fallingIndicatorAlpha;
                playerController.fallStartingH = (int)playerController.transform.position.y;
                playerController.fallCounter = 0;
            }
        }
        else
        {
            if (coll.gameObject.tag == "Player")
            {
                playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, AscenderForce, playerRigidbody.velocity.z);
                playerController.fallingIndicatorAlpha.a = 0f;
                playerController.fallingIndicator.color = playerController.fallingIndicatorAlpha;
                playerController.fallStartingH = (int)playerController.transform.position.y;
                playerController.fallCounter = 0;
            }
        }

        
        
    }
}
