﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerController : MonoBehaviour {
    private new Camera camera;
    private GameObject camHolder;
    public CamMouseLook cam;
    public CameraShake cameraShakeCam;
    [HideInInspector] float timeLastDeath = 0f;

    [HideInInspector] public bool isAlive = true;
    [HideInInspector] public bool canMove = true;
    private float justDied = 0f;

    [SerializeField] GameObject cameraBase;

    [HideInInspector] public Vector3 spawnPoint;

    [SerializeField] float airStraffe = 2.0f;
    [SerializeField] float walkSpeed = 10.0f;
    [SerializeField] float runSpeed = 30.0f;
    [HideInInspector] public float speed;
    private float fadeSpeed = 0.5f;

    [SerializeField] float wallJumpForce = 10.0f;
    [SerializeField] float jumpForce = 10.0f;
    [SerializeField] float jumpForceFade = 10.0f;
    [SerializeField] float maxTimeJump = 0.2f;
    private float actualTimeJump = 1.5f;
    private bool jumped = false;
    private bool doubleWalljumpCounter = false;

    private Vector3 previousPosition;
    [SerializeField] float airBufferDivider = 180f;

    [SerializeField] float groundSpeed = 30.0f;
    public float wallJumpReduction = 10.0f;
    [SerializeField] float speedDivisionStraffe = 2.0f;
    Rigidbody rbPlayer;

    [Range(0.0f, 1.0f)] public float angleAttaque = 0.5f;

    [HideInInspector] public bool onGround = false;

    private AudioSource source;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip walkSound;
    [SerializeField] AudioClip walkSound2;
    [SerializeField] AudioClip walkSound3;
    [SerializeField] AudioClip walkSound4;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip landSound;
    [SerializeField] AudioClip spawnSound;
    [SerializeField] AudioClip power1;
    [SerializeField] AudioClip power2;

    private float walksoundPlayed;
    private float timeBetweenSteps;

    [SerializeField] float worldGravity = -30f;

    private Transform myTransform;
    [HideInInspector] public bool isAlenvers = false;
    [HideInInspector] public GameObject playerHolder;
    [HideInInspector] public bool inverseLook = false;

    [HideInInspector] public bool isAttachedToOne = false;
    [HideInInspector] public bool isAttachedToCrusher = false;
    [HideInInspector] public bool callDeath = false;

    [HideInInspector] public bool slideFirstFrame = true;
    [HideInInspector] public bool isSliding;
    private Vector3 slideVector;
    private bool wasOnGround;
    [HideInInspector] public bool powerIsOnCd;
    [HideInInspector] public int fallStartingH;
    [HideInInspector] public int fallLandingH;
    [SerializeField] private int maxFallHeight = 50;
    [HideInInspector] public int fallCounter = 0;
    private Vector3 jumpStartPosition;
    [SerializeField] private bool alternateAirBuffer = false;
    [SerializeField] private CapsuleCollider myCapsuleCollider5H;
    [HideInInspector] public bool respawnUpsideDown = false;
    private bool fallingCheck;
    [HideInInspector] public float checkpointDirection;
    private bool dieOnLanding = false;

    [HideInInspector] public bool rotatePlayer;
    float aRotation = 0;
    float bRotation = 180;

    [SerializeField] private float spawnDirection;
    private float tiltTweenAngle = 0;
    private float slideTweenHeight = 0;

    [HideInInspector] public int momentum = 0;
    [HideInInspector] public float momentumForce = 0.50f;
    private Vector3 atpPosition;
    private bool staticRotation = false;

    public Image fallingIndicator;
    [HideInInspector] public Color fallingIndicatorAlpha;
    private bool escaping;
    public GameObject exitButton;

    [SerializeField] public int maxFallTime;

    void Start () 
	{
        Physics.gravity = new Vector3(0, worldGravity, 0);
        source = GetComponent<AudioSource>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		rbPlayer = GetComponent<Rigidbody> ();
		speed = walkSpeed;
		spawnPoint = transform.position;
		previousPosition = transform.position;
		timeLastDeath = Time.time;
		canMove = true;
		cameraBase.SetActive (true);
        camera = cameraBase.GetComponent<Camera>();
		Time.timeScale = 1;
		source.PlayOneShot (spawnSound, 1.0f);
        myTransform = GetComponent<Transform>();
        playerHolder = GameObject.FindGameObjectWithTag("PlayerHolder");
        camHolder = GameObject.FindGameObjectWithTag("CamHolder");
        fallingIndicator = GameObject.FindGameObjectWithTag("FallingIndicator").GetComponent<Image>();
        fallingIndicatorAlpha.a = 0f;
        fallingIndicator.color = fallingIndicatorAlpha;
        //cam.mouseLook.x = spawnDirection;
    }

	void Update ()
	{      
        if (canMove) 
		{
			Jumping ();

			WallJumping ();

            HeadTilt();

            if ((Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.LeftControl) || Input.GetAxisRaw("Fire4")!=0 || Input.GetButton("Fire5")) && onGround)
            {
                isSliding = true;
            } else if (!slideFirstFrame && isSliding == true && ( !Input.GetKey(KeyCode.C) || Input.GetAxisRaw("Fire4") == 0))
            {
                isSliding = false;
                DOTween.To(() => cameraShakeCam.transform.localPosition, x => cameraShakeCam.transform.localPosition = x, cameraShakeCam.resetPos, 0.1f);
                myCapsuleCollider5H.center = new Vector3(0,2.5f,0);
                myCapsuleCollider5H.height = 5;
                slideFirstFrame = true;
            }

            if (Input.GetKeyDown(KeyCode.R) || callDeath || Input.GetButtonDown("restart"))
            {
                Debug.Log("dieded Reset");
                Death();
            }
		}

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!escaping)
            {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            exitButton.SetActive(true);
                escaping = true;
            } else
            {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            exitButton.SetActive(false);
                escaping = false;
            }
        }

        if (Time.time - justDied > 0.1f && !isAlive) 
		{
			isAlive = true;
            Debug.Log("Respawn");
            dieOnLanding = false;
            source.PlayOneShot (spawnSound, 1.0f);
			transform.position = spawnPoint;
            fallStartingH = (int)transform.position.y;
            cameraBase.SetActive (true);
			timeLastDeath = Time.time;
			cam.mouseLook = new Vector2 (checkpointDirection, 0f);
			justDied = 0f;           
            canMove = true;
		}

        if ((Input.GetKeyDown(KeyCode.A)|| Input.GetMouseButtonDown(0) || Input.GetButton("Fire3") ) && !powerIsOnCd && !isSliding)
        {   
            InitiateRotation();
            if (isAlenvers)
            {
                source.PlayOneShot(power1, 1.0f);
            }
            else
            {
                source.PlayOneShot(power2, 1.0f);
            }
        }        
        fallingIndicator.color = fallingIndicatorAlpha;
    }

    void MomentumParse()
    {
        if (((Input.GetKey(KeyCode.Z) || Input.GetAxisRaw("Vertical") > 0 )&& onGround) && previousPosition != atpPosition && !isSliding)
        {
            if (momentum < 50)
            {
                momentum++;
                momentumForce = 0.75f + (((float)momentum / 200));
            }
        }
        else if ((Input.GetKey(KeyCode.S) || Input.GetAxisRaw("Vertical") < -0.5 || onGround && !Input.GetKey(KeyCode.Z) || previousPosition == atpPosition) && !isSliding) 
        {
            if (momentum > 0)
            {
            momentum -= 5;
            momentumForce = 0.75f + (((float)momentum / 200));
                if (momentum < 0)
                {
                    momentum = 0;
                }
            }
        }
        camera.fieldOfView = 90 + (((float)momentum / 2)*0.5f);
    }

    public void InitiateRotation ()
    {
        if (isAlenvers)
        {
            isAlenvers = false;
            Physics.gravity = new Vector3(0, worldGravity, 0);
        }
        else
        {
            Physics.gravity = new Vector3(0, -worldGravity, 0);
            isAlenvers = true;
        }
        powerIsOnCd = true;

        

        aRotation = 0;
        bRotation = 180;

        float pRotation = transform.rotation.eulerAngles.y;       
        playerHolder.transform.position = transform.position;      
        transform.position = playerHolder.transform.position;
        if (isAlenvers)
        {
            playerHolder.transform.eulerAngles = new Vector3(playerHolder.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -aRotation);

        }
        else
        {
            playerHolder.transform.eulerAngles = new Vector3(playerHolder.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -bRotation);
        }
        transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, pRotation, transform.rotation.eulerAngles.z);
        cam.mouseLook.x = 0;
        
        fallCounter = 0;
        fallStartingH = (int)transform.position.y;

        DOTween.To(() => fallingIndicatorAlpha.a, x => fallingIndicatorAlpha.a = x, 0f, 0.25f); ;
        
        if (isAlenvers)
        {
            rbPlayer.velocity = new Vector3(rbPlayer.velocity.x,6f, rbPlayer.velocity.z);
        }
        else
        {
            rbPlayer.velocity = new Vector3(rbPlayer.velocity.x, -6f, rbPlayer.velocity.z);
        }
        if (atpPosition == previousPosition)
        {
            staticRotation = true;
        }
        else
        {
            staticRotation = false;
        }
        rotatePlayer = true;     
    }
    
	void FixedUpdate ()
	{
        if (powerIsOnCd || fallingCheck ) {
        fallCounter = fallCounter + 1;

            if (fallCounter > maxFallTime -50)
            {
                fallingIndicatorAlpha.a = 1f - ((180f - fallCounter)*2f/100f);
            }

            if (fallCounter >= maxFallTime )
            {
                fallCounter = 0;
                Debug.Log("dieded longFall");
                Death();
            }
        } else { fallCounter = 0;            
            fallingIndicatorAlpha.a = 0f;
            fallingCheck = false;
        }

		GroundChecking ();
        IsRunning ();
        MomentumParse();

		if (canMove)
        {
			Moving ();
		}

        if (rotatePlayer)
        {         
            if (isAlenvers)
            {
                if (aRotation < 185)
                {
                    playerHolder.transform.position = this.transform.position;
                    transform.position = playerHolder.transform.position;
                    if (aRotation < 165)
                    {
                       
                        playerHolder.transform.eulerAngles = new Vector3(playerHolder.transform.rotation.x, playerHolder.transform.rotation.eulerAngles.y, -aRotation);
                        aRotation = aRotation + 9;
                    }
                    else
                    {
                        aRotation = aRotation - 2;
                        playerHolder.transform.eulerAngles = new Vector3(playerHolder.transform.rotation.x, playerHolder.transform.rotation.eulerAngles.y, -aRotation);
                        aRotation = aRotation + 9;
                    }
                    
                } else
                {
                    playerHolder.transform.eulerAngles = new Vector3(0, playerHolder.transform.rotation.eulerAngles.y, -180);
                    rotatePlayer = false;
                    tiltTweenAngle = -180;
                }
            }
            else
            {
                if (bRotation > -5)
                {
                    playerHolder.transform.position = this.transform.position;
                    transform.position = playerHolder.transform.position;
                    if (bRotation > 15)
                    {
                        playerHolder.transform.eulerAngles = new Vector3(playerHolder.transform.rotation.x, playerHolder.transform.rotation.eulerAngles.y, -bRotation);
                        bRotation = bRotation - 9;
                    }
                    else
                    {
                        bRotation = bRotation + 2;
                        playerHolder.transform.eulerAngles = new Vector3(playerHolder.transform.rotation.x, playerHolder.transform.rotation.eulerAngles.y, -bRotation);
                        bRotation = bRotation - 9;
                    }
                }
                else
                {
                    playerHolder.transform.eulerAngles = new Vector3(0, playerHolder.transform.rotation.eulerAngles.y, -0);
                    rotatePlayer = false;
                    tiltTweenAngle = 0;
                }
            }          
        }
	}

	private void OnCollisionEnter (Collision coll)
	{
		if (coll.gameObject.tag == "KillZone" && isAlive) 
		{
            Debug.Log("dieded Killzone");
            Death();
		}
	}

    private void Death() {
        
        callDeath = false;
        powerIsOnCd = false;
        fallCounter = 0;
        fallingIndicatorAlpha.a = 0f;
        fallingIndicator.color = fallingIndicatorAlpha;
        Debug.Log("resetDeath");

        if (!slideFirstFrame)
        {
            cameraShakeCam.transform.localPosition = cameraShakeCam.resetPos;
            if (isAlenvers)
                {
                    cameraBase.transform.position = new Vector3(cameraBase.transform.position.x, cameraBase.transform.position.y - 1.5f, cameraBase.transform.position.z);
                }
                else
                {
                    cameraBase.transform.position = new Vector3(cameraBase.transform.position.x, cameraBase.transform.position.y + 1.5f, cameraBase.transform.position.z);
                }
            myCapsuleCollider5H.center = new Vector3(0, 2.5f, 0);
            myCapsuleCollider5H.height = 5;
            slideFirstFrame = true;
        }

        if (respawnUpsideDown)
        {
            playerHolder.transform.eulerAngles = new Vector3(playerHolder.transform.rotation.x, playerHolder.transform.rotation.eulerAngles.y, -180);
            Physics.gravity = new Vector3(0, -worldGravity, 0);
            isAlenvers = true;
        } else
        {
            playerHolder.transform.eulerAngles = new Vector3(playerHolder.transform.rotation.x, playerHolder.transform.rotation.eulerAngles.y, 0);
            Physics.gravity = new Vector3(0, worldGravity, 0);
            isAlenvers = false;
        }

        momentum = 0;
        fallingCheck = false;
        isSliding = false;
        source.PlayOneShot(deathSound, 1.0f);
        rbPlayer.velocity = Vector3.zero;
        canMove = false;
        cameraBase.SetActive(false);
        justDied = Time.time;
        isAlive = false;
    }

    private void IsRunning()
	{
		if (Input.GetKey (KeyCode.LeftShift)) 
		{
			//speed = runSpeed;
			//timeBetweenSteps = 0.32f;

            speed = walkSpeed;
            timeBetweenSteps = 0.35f;
        } 
		else 
		{
			speed = walkSpeed;
			timeBetweenSteps = 0.35f;
		}
	}

	private void GroundChecking()
	{
		RaycastHit hit;
		Vector3 physicsCentre = this.transform.position + this.GetComponent<CapsuleCollider> ().center;		

		if (onGround) 
		{
			wasOnGround = true;  
		} 
		else 
		{
			wasOnGround = false;
            fallLandingH = (int)transform.position.y;

            if ((fallStartingH - fallLandingH) >= 1.5 * maxFallHeight)
            {
                dieOnLanding = true;
            }
        }

		float j = 0.0f;
        for (int i = 0; i < 16; i++)
        {

            Vector3 vectorDirector;

            if (isAlenvers)
            {
                vectorDirector = Vector3.up;
            }
            else
            {
                vectorDirector = Vector3.down;
            }       
            Vector3 rayStartPoint = physicsCentre + new Vector3((Mathf.Cos(j * (Mathf.PI / 8)) * 0.42f), 0f, (Mathf.Sin(j * (Mathf.PI / 8)) * 0.42f));

            if (Physics.Raycast(rayStartPoint, vectorDirector, out hit, 0.6f))
            {
                if (hit.transform.gameObject.tag != "Player")
                {
                    onGround = true;
                    break;
                }
            }
            else
            {
                onGround = false;
            }

            j += 1.0f;
            
        }
		if (!wasOnGround && onGround) 
		{
			source.PlayOneShot (landSound, 0.6f);
            fallLandingH = (int)transform.position.y;
            powerIsOnCd = false;

            if (((fallStartingH - fallLandingH) >= maxFallHeight && !isAlenvers) || dieOnLanding || (-(fallStartingH - fallLandingH) >= maxFallHeight && isAlenvers))
            {
                dieOnLanding = false;
                Death();
            }
            else if (!Input.GetKey(KeyCode.C))
            {
                cameraShakeCam.camShaker();
            }
		}
        else if (wasOnGround && !onGround)
        {
            fallStartingH = (int)transform.position.y;
            fallingCheck = true;
        } 
	}

	private void Moving ()
	{
		float translation = 0f;
		float straffe = 0f;

		if (onGround) 
		{
            fallingCheck = false;
            translation = Input.GetAxisRaw ("Vertical") * ((speed * fadeSpeed)*groundSpeed);
			straffe = Input.GetAxisRaw ("Horizontal") * (((speed * fadeSpeed)*groundSpeed) / speedDivisionStraffe);
			translation *= Time.deltaTime;
			straffe *= Time.deltaTime;
            if (isSliding)
            {
                if (slideFirstFrame)
                {
                    Vector3 force = new Vector3(0.0f, 0.0f, (((speed * fadeSpeed) * groundSpeed) * 0.037f)) * momentumForce;
                    force = transform.localToWorldMatrix.MultiplyVector(force);
                    slideVector = force;
                    rbPlayer.AddForce(force, ForceMode.VelocityChange);
                    slideFirstFrame = false;

                    cameraShakeCam.transform.localPosition = cameraShakeCam.resetPos;
                    if (isAlenvers)
                    {
                        slideTweenHeight = cameraBase.transform.position.y;
                        DOTween.To(() => slideTweenHeight, x => slideTweenHeight = x, slideTweenHeight + 1.2f, 0.1f);
                    }
                    else
                    {
                        slideTweenHeight = cameraBase.transform.position.y;
                        DOTween.To(() => slideTweenHeight, x => slideTweenHeight = x, slideTweenHeight - 1.2f, 0.1f);
                    }
                    myCapsuleCollider5H.center = new Vector3(0, 1f, 0);
                    myCapsuleCollider5H.height = 2;
                } else
                {
                    if (cameraBase.transform.position.y != slideTweenHeight)
                    {
                    cameraBase.transform.position = new Vector3(cameraBase.transform.position.x, slideTweenHeight, cameraBase.transform.position.z);
                    }
                    slideVector = slideVector * 0.987f;
                    rbPlayer.AddForce(slideVector, ForceMode.VelocityChange);
                }
            }
            else
            {               
                Vector3 force = new Vector3(straffe, 0.0f, translation * momentumForce);
			    force = transform.localToWorldMatrix.MultiplyVector (force);
			    rbPlayer.AddForce (force, ForceMode.VelocityChange);
            }

			Vector3 v = rbPlayer.velocity;
			v.x = 0f;
			v.z = 0f;
			rbPlayer.velocity = v;

			if (fadeSpeed < 1.1f)
			{
				fadeSpeed += 0.1f;
			}

			if (translation == 0 && straffe == 0 )
			{
				fadeSpeed = 0.5f;
			}

            if ((translation != 0f || straffe != 0f) && Time.time - walksoundPlayed > timeBetweenSteps)
            {
                int soundChoice = Random.Range(1, 5);
                if (soundChoice == 1)
                {
                    source.PlayOneShot(walkSound, Random.Range(0.10f, 0.20f));
                }
                else if (soundChoice == 2)
                {
                    source.PlayOneShot(walkSound2, Random.Range(0.10f, 0.20f));
                }
                else if (soundChoice == 3)
                {
                    source.PlayOneShot(walkSound3, Random.Range(0.10f, 0.20f));
                }
                else
                {
                    source.PlayOneShot(walkSound4, Random.Range(0.10f, 0.20f));
                }
                walksoundPlayed = Time.time;
            }
            alternateAirBuffer = false;
            jumpStartPosition = transform.position;
		}
        else if (((jumpStartPosition.x == transform.position.x) && (jumpStartPosition.z == transform.position.z)) || alternateAirBuffer)
        {
            alternateAirBuffer = true;
            translation = Input.GetAxis("Vertical") * speed;
            straffe = Input.GetAxis("Horizontal") * speed * airStraffe;
            translation *= Time.deltaTime;
            straffe *= Time.deltaTime;
            float airControlBuffer = CalculateAirControlBuffer(translation, straffe);
            Vector3 force = new Vector3(straffe * airControlBuffer *6, 0.0f, translation * airControlBuffer *6);
            force = transform.localToWorldMatrix.MultiplyVector(force);
            rbPlayer.AddForce(force, ForceMode.VelocityChange);
        } 
        else
        {
			translation = Input.GetAxis ("Vertical") * speed;
			straffe = Input.GetAxis ("Horizontal") * speed * airStraffe;
			translation *= Time.deltaTime;
			straffe *= Time.deltaTime;
			float airControlBuffer = CalculateAirControlBuffer (translation, straffe);
			Vector3 force = new Vector3 (straffe * airControlBuffer, 0.0f, translation * airControlBuffer);
			force = transform.localToWorldMatrix.MultiplyVector (force);
			rbPlayer.AddForce (force, ForceMode.VelocityChange);
		}
        atpPosition = previousPosition;
		previousPosition = transform.position;
	}

	private float CalculateAirControlBuffer (float translation, float straffe)
	{
		Vector3 currentDirection = transform.position - previousPosition;
		currentDirection = new Vector3 (currentDirection.x, 0f, currentDirection.z);
		Vector3 direction = new Vector3 (straffe, 0f, translation);
		direction = transform.TransformDirection (direction);
		float angle = Vector3.Angle (currentDirection, direction);
		if (angle > 30) {
			float airControlBuffer = 1 + (angle / airBufferDivider);
			return airControlBuffer;
		}
		return 1.0f;
	}
		
	private void Jumping()
	{
        if (isAlenvers)
        {
            if ((Input.GetKeyDown("space") || Input.GetButtonDown("Fire2")) && onGround)
            {                
                rbPlayer.velocity = new Vector3(rbPlayer.velocity.x, -jumpForce, rbPlayer.velocity.z);
                actualTimeJump = Time.time;
                jumped = true;
                source.PlayOneShot(jumpSound, 0.6f);
            }

            if ((Input.GetKey("space") || Input.GetButton("Fire2")) && Time.time - actualTimeJump < maxTimeJump)
            {                
                rbPlayer.velocity = new Vector3(rbPlayer.velocity.x, (-jumpForce - (-jumpForceFade * (Time.time - actualTimeJump))), rbPlayer.velocity.z);
            }
        }
        else if (!isAlenvers)
        {
            if ((Input.GetKeyDown("space") || Input.GetButtonDown("Fire2")) && onGround)
            {                
                rbPlayer.velocity = new Vector3(rbPlayer.velocity.x, jumpForce, rbPlayer.velocity.z);
                actualTimeJump = Time.time;
                jumped = true;
                source.PlayOneShot(jumpSound, 0.6f);
            }

            if ((Input.GetKey("space") || Input.GetButton("Fire2")) && Time.time - actualTimeJump < maxTimeJump)
            {
                rbPlayer.velocity = new Vector3(rbPlayer.velocity.x, (jumpForce - (jumpForceFade * (Time.time - actualTimeJump))), rbPlayer.velocity.z);
            }

        }

        if ((Input.GetKeyUp("space") || Input.GetButtonUp("Fire2")) && Time.time - actualTimeJump < maxTimeJump && jumped)
        {
            rbPlayer.velocity = new Vector3(rbPlayer.velocity.x, 0f, rbPlayer.velocity.z);
            jumped = false;
        }
	}

    private void HeadTilt()
    {
        Vector3 vectorDirector;
        Vector3 vectorDirector2;
        bool hitRight = false;
        bool hitLeft = false;    

        vectorDirector = transform.TransformDirection(new Vector3(1.5f, 0, 1.5f));
        vectorDirector2 = transform.TransformDirection(new Vector3(-1.5f, 0, 1.5f));
        RaycastHit hit;

        //Debug.DrawRay(transform.position, vectorDirector * 1, Color.red);
        //Debug.DrawRay(transform.position, vectorDirector2 * 1, Color.red);

        if (Physics.Raycast(transform.position, vectorDirector, out hit, 1.2f))
        {
            if (hit.transform.tag == "WallJumpAble")
            {
                hitRight = true;
            }
        }
        if (Physics.Raycast(transform.position, vectorDirector2, out hit, 1.2f))
        {
            if (hit.transform.tag == "WallJumpAble")
            {
                hitLeft = true;
            }
        }

        if (isAlenvers && !rotatePlayer && !isSliding)
        {
            if (hitLeft && !hitRight)
            {
                if (tiltTweenAngle == -180 || tiltTweenAngle == -184)
                {
                    DOTween.To(() => tiltTweenAngle, x => tiltTweenAngle = x, -176, 0.1f);
                }
                camHolder.transform.eulerAngles = new Vector3(camHolder.transform.eulerAngles.x, camHolder.transform.eulerAngles.y, tiltTweenAngle);
            }
            else if (!hitLeft && hitRight)
            {
                if (tiltTweenAngle == -180 || tiltTweenAngle == -176)
                {
                    DOTween.To(() => tiltTweenAngle, x => tiltTweenAngle = x, -184, 0.1f);
                }
                camHolder.transform.eulerAngles = new Vector3(camHolder.transform.eulerAngles.x, camHolder.transform.eulerAngles.y, tiltTweenAngle);
            }
            else
            {
                if (tiltTweenAngle != -180)
                {
                    DOTween.To(() => tiltTweenAngle, x => tiltTweenAngle = x, -180, 0.1f);
                }

                camHolder.transform.eulerAngles = new Vector3(camHolder.transform.eulerAngles.x, camHolder.transform.eulerAngles.y, tiltTweenAngle);
            }
        }
        else if (!isAlenvers && !rotatePlayer && !isSliding)
        {
            if (hitLeft && !hitRight)
            {
                if (tiltTweenAngle == 0 || tiltTweenAngle == -4)
                {
                    DOTween.To(() => tiltTweenAngle, x => tiltTweenAngle = x, 4, 0.1f);
                }
                camHolder.transform.eulerAngles = new Vector3(camHolder.transform.eulerAngles.x, camHolder.transform.eulerAngles.y, tiltTweenAngle);
            }
            else if (!hitLeft && hitRight)
            {
                if (tiltTweenAngle == 0 || tiltTweenAngle == 4)
                {
                    DOTween.To(() => tiltTweenAngle, x => tiltTweenAngle = x, -4, 0.1f);
                }
                camHolder.transform.eulerAngles = new Vector3(camHolder.transform.eulerAngles.x, camHolder.transform.eulerAngles.y, tiltTweenAngle);
            }
            else
            {
                if (tiltTweenAngle != 0)
                {
                    DOTween.To(() => tiltTweenAngle, x => tiltTweenAngle = x, 0, 0.1f);
                }
                camHolder.transform.eulerAngles = new Vector3(camHolder.transform.eulerAngles.x, camHolder.transform.eulerAngles.y, tiltTweenAngle);
            }
        }
    }
        
	private void WallJumping()
	{
		float j = 0f;
		for (int i = 0; i < 16; i++) 
		{
            Vector3 vectorDirector;
            if (isAlenvers)
            {
                vectorDirector = Vector3.down;
            }
            else
            {
                vectorDirector = Vector3.up;
            }
            Vector3 vectorDirection = Quaternion.AngleAxis(j * 22.5f, vectorDirector) * Vector3.forward;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, vectorDirection, out hit, 1.35f) && hit.transform.tag == "WallJumpAble")
            {
                fallStartingH = (int)transform.position.y;
                if ((Input.GetKeyDown("space") || Input.GetButtonDown("Fire2")) && hit.normal.y < angleAttaque && !onGround && !doubleWalljumpCounter )
                {
                    fallCounter = 0;
                    Vector3 v = rbPlayer.velocity;
                    v.y = 0f;
                    rbPlayer.velocity = v;
                    if (isAlenvers)
                    {
                    rbPlayer.velocity = new Vector3(rbPlayer.velocity.x, -wallJumpForce, rbPlayer.velocity.z);
                    } else
                    {
                    rbPlayer.velocity = new Vector3(rbPlayer.velocity.x, wallJumpForce, rbPlayer.velocity.z);
                    }
                    rbPlayer.AddForce(hit.normal * (speed / wallJumpReduction), ForceMode.VelocityChange);
                    doubleWalljumpCounter = true;
                    source.PlayOneShot(jumpSound, 0.6f);
                    break;
                }
            }
            else
            {
                doubleWalljumpCounter = false;
            }
            j += 1f;
		}
	}
}
