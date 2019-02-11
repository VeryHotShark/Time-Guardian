using System.Collections;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    [Header("Post Processing")]
    public PostProcessProfile postProcessing;

    [Header("SlowMo")]
    public float slowMoAmount = 0.5f;
    public float slowMoLerpSpeed = 2f;
    public bool doSlowMo = true;
    bool slowMo = false;
    float normalTimeScale = 1f;
    float slowMoCurrentVelocity;

    [Space]
    public ParticleSystem DeathVFX;

    public int lives = 3;
    int currentLives;
    public Image health;

    public Transform spawnTransform;
    public Projectile projectile;
    public float respawnTime = 2f;

    [Header("Movement Variables")]
    public Transform vfxRotation;
    public ParticleSystem dashVFX;
    public float stopThreshold = 0.8f;
    public float moveThreshold = 2f;
    public float moveSpeed = 5f;
    public float smoothTime = 0.5f;
    public float SlerpSpeed = 2f;
    public float dashForce = 5f;

    public LayerMask layersToCollideWith;
    private Rigidbody rigid;

    bool moving;
    Vector3 moveVelocity;

    float smoothVelocity;
    float smoothFactor;

    //Vector3 rotationInEuler;
    Quaternion finalRotationInQuaternion;

    Vector3 startPos;
    Quaternion startRot;

    public static event System.Action OnPlayerDeath;
    public static event System.Action OnPlayerDash;

    MainMenu mainMenuSetting;
    bool gameIsPaused;

    // Use this for initialization
    void Start ()
    {
        mainMenuSetting = MainMenu.instance;

        if (!mainMenuSetting)
            gameIsPaused = false;

        if (mainMenuSetting)
            mainMenuSetting.OnPausePress += TogglePause;

        rigid = GetComponent<Rigidbody>();
        startPos = transform.position;
        startRot = transform.rotation;
        currentLives = lives;
	}
	
    void TogglePause()
    {
        gameIsPaused = !gameIsPaused;
    }

	// Update is called once per frame
	void Update ()
    {

        if (!gameIsPaused)
        {
            if (!GameManager.instance.gameIsOver || GameManager.instance.playerWon)
            {
                Movement();
                Dash();
                Shoot();
                if (doSlowMo)
                    DoSlowMo();
            }
            else
            {
                Time.timeScale = Mathf.SmoothDamp(Time.timeScale, normalTimeScale, ref slowMoCurrentVelocity, slowMoLerpSpeed);
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                moving = false;
                moveVelocity = Vector3.zero;
                rigid.velocity = Vector3.zero;
            }
        }
    }

    void DoSlowMo()
    {
        // We Get The Movement from our mouse 
        float horizontal = Input.GetAxis("Mouse X");
        float vertical = Input.GetAxis("Mouse Y");

        if (!moving && !(Mathf.Abs(horizontal) > 0f) && !(Mathf.Abs(vertical) > 0f) && !(Mathf.Abs(rigid.velocity.x) > 0.1f) && !(Mathf.Abs(rigid.velocity.z) > 0.1f) && !Input.GetMouseButton(0) ) // if we are not moving and not rotation or moving our mouse or velocity of our rigidbody is less than 0.1f or we are not shooting then we do Slow motion
            slowMo = true;
        else if(moving || (Mathf.Abs(rigid.velocity.x) > 0f) || (Mathf.Abs(rigid.velocity.z) > 0f) || Mathf.Abs(horizontal) > 0f || Mathf.Abs(vertical) > 0f || Input.GetMouseButton(0)) // otherwise if we do at least one of those i mention above we go back to normal time
            slowMo = false;

        //Debug.Log(slowMo);

        if(slowMo) // here we just lerp to slow mottion time scale and after that we have to change Time.fixed delta Time too so it will stay smooth
        {
            Time.timeScale = Mathf.SmoothDamp(Time.timeScale, slowMoAmount, ref slowMoCurrentVelocity, slowMoLerpSpeed /2f);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
        else
        {
            Time.timeScale = Mathf.SmoothDamp(Time.timeScale, normalTimeScale, ref slowMoCurrentVelocity, slowMoLerpSpeed);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }

    }

    private void Movement()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition); // cast a ray through screen pointing at our mouse direction
        RaycastHit hit;

        bool hitGround = Physics.Raycast(mouseRay, out hit, 100f, layersToCollideWith,QueryTriggerInteraction.Collide); // check if we collide with a ground;

        if (hitGround) // if we did
        {
            Vector3 dir = (hit.point - transform.position); // we calculate the direction vector from our player to hit point, so we can specify in which dir our player has to rotate towards
            Vector3 projectOnSurfaceDir = Vector3.ProjectOnPlane(dir, transform.up).normalized; // then be project the vector on the ground so basically we flatten it and we normalize it;

            moving = dir.magnitude > moveThreshold ? true : false; // if our cursor is enough far away from our player we make it so it can 

            Vector3 moveDir = projectOnSurfaceDir; // assign our flat vector to new one, so we can operate more easily;
            float moveDirMagnitude = moveDir.magnitude; // we get the magnitude of our direction vector

            moveDirMagnitude = moving ? moveDirMagnitude : 0f; // if we are moving we make it stay the same length which is 1f otherwise we set it to 0f

            smoothFactor = Mathf.SmoothDamp(smoothFactor, moveDirMagnitude, ref smoothVelocity, smoothTime); // here we make our smooth variable which will interpolate between 0 and 1 depending on whether we are moving or not
            moveVelocity = moveDir * moveSpeed * smoothFactor; // here we  multiply everything together to get our final Vector 

            //Debug.Log(dir.magnitude);

            if (dir.magnitude < stopThreshold) // if the cursor is to closed to our player we set the moving bool to false and final moving Vector to zero so it will stop moving
            {
                moving = false;
                moveVelocity = Vector3.zero;
            }

            // SMOOTH ROTATION ON Y Axis
            //Debug.Log(projectOnSurfaceDir);
            projectOnSurfaceDir.y = 0; // we flatten our surface dir so it woint point upwards or dowards
            Quaternion desiredRotation = Quaternion.LookRotation(projectOnSurfaceDir); // we specify lookRotation for our player by passing VEctor that we wants to point towards
            Quaternion finalRotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * SlerpSpeed); // here we apply a slight smoothing to our rotation so it won't rotate towards our cursor instantly
            transform.rotation = finalRotation;
           
            //finalRotationInQuaternion = Quaternion.Euler(finalRotation.eulerAngles * Time.deltaTime);
            //AlignRotationWithGround(finalRotation);
        }
        else// if we are not hitting the ground just stop our player or game is over
        {
            moving = false;
            moveVelocity = Vector3.zero;
            rigid.velocity = Vector3.zero;
        }

    }

    void AlignRotationWithGround(Quaternion yRotation)
    {
        // CONVERT QUATERNION ROTATION TO EULER ANGLES
        Vector3 rotationInEuler = yRotation.eulerAngles; //we convert our Quaternion rotation into eulerAngles rotation (the rotation that is normal and easy to operate on) // 

        // CALCULATE CUBE ROTATION ON SLOPES
        Ray groundRay = new Ray(transform.position, -(transform.up)); // w cast a ray downwards to chech if we hit the ground
        RaycastHit hitNormal;

        bool onGround = Physics.Raycast(groundRay, out hitNormal, 1f, layersToCollideWith);

        if (onGround) // if we did
        {
            Vector3 cross = Vector3.Cross(transform.right, hitNormal.normal); // we get the cross product of our right vector and ground normal
            Quaternion desiredRot = Quaternion.LookRotation(cross); // we create a a look rotation by passing our cross product
            Vector3 tiltEulerRot = desiredRot.eulerAngles; // we convert it to euler Angles
            transform.rotation = Quaternion.Euler(tiltEulerRot.x, rotationInEuler.y, tiltEulerRot.z); // and finally we mix those two rotations, so on X and Z axis we want it to be affected by this rotation because we want it to adjust to our ground slopes and on Y axis we want the rotation that we calculated earlier
        }
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + moveVelocity * Time.fixedDeltaTime); // actually move our player based on our final move Vector 
        //rigid.velocity = moveVelocity
        //rigid.MoveRotation(rigid.rotation * finalRotationInQuaternion); // NIe działa ale zrób by działało
    }

    void Dash()
    {
        if(Input.GetMouseButtonDown(1)) // if we press right mouse button
        {
            if (AudioManager.instance)
                AudioManager.instance.PlayClip("Thrust");

            if (GameManager.instance.tutorial)
            {
                if (OnPlayerDash != null)
                    OnPlayerDash();
            }

            rigid.AddForce(transform.forward * dashForce , ForceMode.Impulse); // we add force to  our player in forward dir
            if(dashVFX!=null)
            {
                ParticleSystem vfx = Instantiate(dashVFX, vfxRotation.position, vfxRotation.rotation) as ParticleSystem;
                Destroy(vfx.gameObject, vfx.main.duration);
            }
        }
    }

    void Shoot()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (AudioManager.instance)
                AudioManager.instance.PlayClip("Shoot");

            Projectile spawnedProjectile = Instantiate(projectile, spawnTransform.position, spawnTransform.rotation) as Projectile;
            spawnedProjectile.OnProjectileSpawn(spawnTransform.forward);
        }
    }

    public void UpdateHealth()
    {
            if (AudioManager.instance)
                AudioManager.instance.PlayClip("Death");

        currentLives--;

        if (currentLives <= 0)
        {
            Die();
        }

        float healthAsPercent = (float)currentLives / (float)lives;
        Debug.Log(healthAsPercent);
        healthAsPercent = Mathf.Clamp(healthAsPercent, 0f,1f);

        health.fillAmount = healthAsPercent;
    }


    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, moveThreshold);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("DeadZone"))
        {
            //gameObject.SetActive(false);
            Die();
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);

        rigid.velocity = Vector3.zero;
        transform.position = startPos;
        transform.rotation = startRot;

        UpdateHealth();
    }

    void RespawnPlayer()
    {
        rigid.velocity = Vector3.zero;
        transform.position = startPos;
        transform.rotation = startRot;

        UpdateHealth();
    }

    void Die()
    {

        if(!GameManager.instance.tutorial)
        {
            if(mainMenuSetting != null)
                mainMenuSetting.OnPausePress -= TogglePause;
        }

        if (OnPlayerDeath!= null)
        {
            OnPlayerDeath();
        }
        ParticleSystem vfx = Instantiate(DeathVFX, transform.position, DeathVFX.transform.rotation) as ParticleSystem;
        Destroy(vfx.gameObject, vfx.main.duration);

        if (GameManager.instance.tutorial)
        {
            currentLives = lives + 1;
            RespawnPlayer();
        }
        else
        {
            GameManager.instance.gameIsOver = true;
            Destroy(gameObject);
        }
    }
}
