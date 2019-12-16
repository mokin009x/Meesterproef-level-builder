using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceTower : MonoBehaviour
{
    public Transform target;
    //public bool hasTarget = false;
    

    [Header("Attributes")]
    public float range = 4f;
    public float fireRate = 1f;
    private float _fireRateTimer = 0f;
    
    [Header("Unity Setup Fields")] 
    public float turnSpeed = 10;
    public Transform partToRotate;
    
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform fireRotation;
    

    // Start is called before the first frame update
    public  void Start()
    {
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
    }

    public void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (var enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
            
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    
    // Update is called once per frame
    public  void Update()
    {
        if (target == null)
        {
            return;
        }

        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation =  Quaternion.Euler(0f, rotation.y, 0f);

        if (_fireRateTimer <= 0)
        {
            Shoot();
            _fireRateTimer = 1f / fireRate;
        }

        _fireRateTimer -= Time.deltaTime;
    }

    public void Shoot()
    {
        GameObject bulletInstance = Instantiate(bulletPrefab,firePoint.position, firePoint.rotation);
        Bullets bullet = bulletInstance.GetComponent<Bullets>();

        if (bullet!= null)
        {
            bullet.seek(target);
            Debug.Log("Shoot");

        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
