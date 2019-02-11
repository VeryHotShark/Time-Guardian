using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public LayerMask enemyLayer;
    public LayerMask groundLayer;
    public ParticleSystem bulletSplash;
    public float force;
    private Rigidbody rigid;

    Vector3 initialPos;
    float halfProjectileSize;

	// Update is called once per frame
	public void OnProjectileSpawn (Vector3 dir)
    {
        initialPos = transform.position;
        halfProjectileSize = transform.localScale.x / 2f;
        rigid = GetComponent<Rigidbody>();
        rigid.AddForce(dir * force, ForceMode.Impulse);
        Destroy(gameObject, 3f);
    }

    public void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position , transform.forward);

        Ray rightRay = new Ray(transform.position + (transform.right * halfProjectileSize), transform.forward);
        Debug.DrawRay(transform.position + (transform.right * halfProjectileSize), transform.forward);

        Ray leftRay = new Ray(transform.position - (transform.right * halfProjectileSize), transform.forward);
        Debug.DrawRay(transform.position - (transform.right * halfProjectileSize), transform.forward);

        Ray upRay = new Ray(transform.position + (transform.up* halfProjectileSize), transform.forward);
        Debug.DrawRay(transform.position + (transform.up * halfProjectileSize), transform.forward);

        Ray bottomRay = new Ray(transform.position - (transform.up * halfProjectileSize), transform.forward);
        Debug.DrawRay(transform.position - (transform.up * halfProjectileSize), transform.forward);

        Ray rightCornerRay = new Ray(transform.position + (transform.right * halfProjectileSize) + (transform.up * halfProjectileSize), transform.forward);
        Debug.DrawRay(transform.position + (transform.right * halfProjectileSize) + (transform.up * halfProjectileSize), transform.forward);

        Ray leftCornerRay = new Ray(transform.position - (transform.right * halfProjectileSize) + (transform.up * halfProjectileSize), transform.forward);
        Debug.DrawRay(transform.position - (transform.right * halfProjectileSize) + (transform.up * halfProjectileSize), transform.forward);

        Ray rightBottomCornerRay = new Ray(transform.position + (transform.right * halfProjectileSize) - (transform.up * halfProjectileSize), transform.forward);
        Debug.DrawRay(transform.position + (transform.right * halfProjectileSize) - (transform.up * halfProjectileSize), transform.forward);

        Ray leftBottomCornerRay = new Ray(transform.position - (transform.right * halfProjectileSize) - (transform.up * halfProjectileSize), transform.forward);
        Debug.DrawRay(transform.position - (transform.right * halfProjectileSize) - (transform.up * halfProjectileSize), transform.forward);

        RaycastHit hit;
        RaycastHit hitL;
        RaycastHit hitR;
        RaycastHit hitU;
        RaycastHit hitB;
        RaycastHit hitLC;
        RaycastHit hitLBC;
        RaycastHit hitRC;
        RaycastHit hitRBC;

        Ray groundRay = new Ray(transform.position, -(transform.up));
        RaycastHit hitG;

        //bool hitEnemy = Physics.BoxCast(transform.position, transform.localScale + Vector3.one * 0.02f, transform.forward, out hit, Quaternion.identity, 0.12f, enemyLayer);
        bool hitEnemy = Physics.Raycast(ray, out hit, 0.12f, enemyLayer); // center ray;
        bool hitRightRay = Physics.Raycast(rightRay, out hitR, 0.12f, enemyLayer); // right ray;
        bool hitLeftRay = Physics.Raycast(leftRay, out hitL, 0.12f, enemyLayer); // left ray;
        bool hitUpRay = Physics.Raycast(upRay, out hitU, 0.12f, enemyLayer);
        bool hitBottomRay = Physics.Raycast(bottomRay, out hitB, 0.12f, enemyLayer); 
        bool hitRightCornerRay = Physics.Raycast(rightCornerRay, out hitRC, 0.12f, enemyLayer); 
        bool hitLeftCornerRay = Physics.Raycast(leftCornerRay, out hitLC, 0.12f, enemyLayer); 
        bool hitRightBottomCornerRay = Physics.Raycast(rightBottomCornerRay, out hitRBC, 0.12f, enemyLayer); 
        bool hitLeftBottomCornerRay = Physics.Raycast(leftBottomCornerRay, out hitLBC, 0.12f, enemyLayer); 

        bool hitGround = Physics.Raycast(groundRay, out hitG, 0.12f, groundLayer, QueryTriggerInteraction.Ignore);

        if (hitGround)
        {
            InstantiateVFX(hitG);

            Destroy(gameObject);
        }

        if (hitLeftCornerRay)
            HitEnemy(hitLC);
        else if (hitLeftBottomCornerRay)
            HitEnemy(hitLBC);
        else if (hitRightCornerRay)
            HitEnemy(hitRC);
        else if (hitRightBottomCornerRay)
            HitEnemy(hitRBC);
        else if (hitLeftRay)
            HitEnemy(hitL);
        else if (hitRightRay)
            HitEnemy(hitR);
        else if (hitUpRay)
            HitEnemy(hitU);
        else if (hitBottomRay)
            HitEnemy(hitB);
        else if (hitEnemy)
            HitEnemy(hit);

    }

    void HitEnemy(RaycastHit hit)
    {
        if (AudioManager.instance)
            AudioManager.instance.PlayClip("Hit");

        InstantiateVFX(hit);
        Enemy enemy = hit.collider.GetComponent<Enemy>();
        enemy.OnTakeDamage();
        Destroy(gameObject);
    }

    public void InstantiateVFX(RaycastHit hit)
    {

        ParticleSystem vfx = Instantiate(bulletSplash, hit.point, Quaternion.identity) as ParticleSystem;
        Vector3 shootDir = (hit.point - initialPos).normalized;
        vfx.transform.rotation = Quaternion.LookRotation(shootDir);
        Destroy(vfx.gameObject, vfx.main.duration);
    }

}
