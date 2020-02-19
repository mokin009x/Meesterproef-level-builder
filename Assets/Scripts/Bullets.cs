using System;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    [Header("Bullet Stats")]
    
    public float dmg = 1;
    public float speed = 70f;

    private Vector3 _aimPoint;
    private Transform _target;


    // Start is called before the first frame update
    private void Start()
    {
    }

    public void seek(Transform target)
    {
        _target = target;
        _aimPoint = target.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = _aimPoint - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    /*private void OnCollisionEnter(Collision other)
    {
        if (_target.gameObject == other.gameObject) // note: targeting for rockets better with first enemy hit but no time for it now
        {
            HitTarget();
        }
    }*/

    public void HitTarget()
    {
        // officially missed target but for now hit the enemy anyway
        _target.gameObject.GetComponent<Enemy>().GetHit(dmg);
        Destroy(gameObject);
    }
    
    
}