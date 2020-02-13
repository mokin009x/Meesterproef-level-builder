using UnityEngine;

public class Bullets : MonoBehaviour
{
    private Vector3 _aimPoint;
    private Transform _target;

    public float speed = 70f;

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

    public void HitTarget()
    {
        Destroy(_target.gameObject);
        Destroy(gameObject);
    }
}