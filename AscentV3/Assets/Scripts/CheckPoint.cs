using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class CheckPoint : MonoBehaviour
{

    Vector3 myPos;
    public bool isActivated;
    public GameObject Player;
    public PlayerController playerController;
    public bool checkpointIsUpsideDown;
    public float playerDirection;
    private VisualEffect visualEffect1;
    
    public GameObject vfx2;
    public float vfx2Duration;
    private GameObject vfxInst;

    void Start()
    {
        myPos = transform.position;
        Player = GameObject.FindGameObjectWithTag("Player");
        playerController = Player.GetComponent<PlayerController>();
        visualEffect1 = GetComponentInChildren<VisualEffect>();
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player" && !isActivated)
        {
            //visualEffect1.enabled = !visualEffect1.enabled;
            isActivated = true;
            playerController.spawnPoint = myPos;
            playerController.respawnUpsideDown = checkpointIsUpsideDown;
            playerController.checkpointDirection = playerDirection;
            //vfxInst = (GameObject)Instantiate(vfx2, visualEffect1.transform);
            Destroy(vfxInst, vfx2Duration);
        }
    }
}
