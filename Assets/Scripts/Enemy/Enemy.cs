using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {


    public event System.Action<Enemy> OnEnemyDeath;

    public ParticleSystem deathVFX;
    public float randomForceDifference = 1f;

    public float force = 5f;
    public float smoothAmount = 1f;
    Transform targetToFollow;
    private Rigidbody rigid;

    Vector3 desiredDir;

    public float setForce { set { force = value; } }
    // Use this for initialization
    void Start ()
    {
        if(!GameManager.instance.gameIsOver)
            targetToFollow = GameObject.FindObjectOfType<Player>().transform;   
        force += Random.Range(-randomForceDifference, randomForceDifference);
        rigid = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(targetToFollow != null && !GameManager.instance.gameIsOver)
            desiredDir = (targetToFollow.position - transform.position).normalized;
	}

    private void FixedUpdate()
    {
        if (!GameManager.instance.gameIsOver)
            rigid.AddForce(desiredDir * force, ForceMode.Force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            CameraShake.isShaking = true;
            collision.gameObject.GetComponent<Player>().UpdateHealth();
            Die();
        }

    }

    public void OnTakeDamage()
    {
        // For now
        Die();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeadZone"))
        {
            //gameObject.SetActive(false);
            Die();
        }
    }

    void Die()
    {
        ParticleSystem VFX = Instantiate(deathVFX, transform.position, Quaternion.identity);
        Destroy(VFX.gameObject, VFX.main.duration);

        if (OnEnemyDeath != null)
            OnEnemyDeath(this);

        Destroy(gameObject);
    }
}
