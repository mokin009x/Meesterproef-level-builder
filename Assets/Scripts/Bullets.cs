using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    private Transform _target;
    private Vector3 _aimPoint;
    public float speed = 70f;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void seek(Transform target)
    {
        _target = target;
        _aimPoint = target.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_target == null)
        {
            Destroy(this.gameObject);
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

    public void HitTarget()
    {
        Destroy(_target.gameObject);
        Destroy(this.gameObject);
    }




}
