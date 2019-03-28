using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMouseLook : MonoBehaviour {

	public Vector2 mouseLook;
	public float sensitivity = 5.0f;
    public float slideInitialX;
    public float slideSecondX;
    private float slideXMod;

    public GameObject character;
    [HideInInspector] PlayerController playerController;
    [HideInInspector] GameObject playerHolder;

	// Use this for initialization
	void Start () {
		character = this.transform.parent.parent.gameObject;
        playerController = character.GetComponent<PlayerController>();
        playerHolder = GameObject.FindGameObjectWithTag("PlayerHolder");
    }

	// Update is called once per frame
	void Update () {
       
		Vector2 mouseDelta = new Vector2 (Input.GetAxisRaw ("Mouse X"), Input.GetAxisRaw ("Mouse Y"));

        if (Input.GetAxis("Horizontal2") !=0 || Input.GetAxis("Vertical2") != 0)
        {
        mouseDelta = new Vector2(Input.GetAxisRaw("Horizontal2") *3, Input.GetAxisRaw("Vertical2") *3);
        }
        //Debug.Log (mouseDelta);

        mouseDelta = Vector2.Scale (mouseDelta, new Vector2 (sensitivity , sensitivity ));
		mouseLook += mouseDelta;
		transform.localRotation = Quaternion.AngleAxis (-mouseLook.y, Vector3.right);
        character.transform.localRotation = Quaternion.identity;

        if (playerController.slideFirstFrame)
        {
            slideInitialX = mouseLook.x;
        }

        if (playerController.isSliding && !playerController.slideFirstFrame)
        {
            slideXMod = (slideInitialX - mouseLook.x);
      
            if (slideXMod > 60.0f)
            {
                slideXMod = 60.0f;
                mouseLook.x = slideInitialX - 60;
            }
            else if (slideXMod < -60.0f)
            {
                slideXMod = -60.0f;
                mouseLook.x = slideInitialX + 60;
            }

            slideSecondX = -(slideInitialX - slideXMod);
        }
            

        if (playerController.isAlenvers == true)
            {
                if (playerController.isSliding && !playerController.slideFirstFrame)
                {
                    character.transform.localRotation = Quaternion.AngleAxis(slideSecondX , character.transform.up);
                }
                else
                {
                    character.transform.localRotation = Quaternion.AngleAxis(-mouseLook.x, character.transform.up);
                }
            }
            else
            {
                if (playerController.isSliding && !playerController.slideFirstFrame)
                {
                    character.transform.localRotation = Quaternion.AngleAxis(-slideSecondX, character.transform.up);
                }
                else
                {                        
                    character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
                }
            }

        
        BlockView ();
	}



	void BlockView()
	{
		if (mouseLook.y > 90.0f) 
		{
			mouseLook.y = 90.0f;
			transform.localRotation = Quaternion.AngleAxis (-mouseLook.y, Vector3.right);
		}

		if (mouseLook.y < -70.0f) 
		{
			mouseLook.y = -70.0f;
			transform.localRotation = Quaternion.AngleAxis (-mouseLook.y, Vector3.right);
		}

        if (mouseLook.x > 360)
        {
            mouseLook.x = mouseLook.x - 360;
        }

        if (mouseLook.x < -360)
        {
            mouseLook.x = mouseLook.x + 360;
        }

    }
}
