using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public Transform pivotPoint;
    [SerializeField] private float bulletForce;

    void Start()
    {
        
    }

    private void Shoot()
    {
        GameObject bullet = ObjectPool.SharedInstance.GetPooledObject();
        if(bullet != null)
        {
            bullet.transform.position = pivotPoint.position;
            bullet.transform.rotation = transform.rotation;

            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if(bulletRb != null)
            {
                bulletRb.velocity = bullet.transform.forward * bulletForce;
            }

            bullet.SetActive(true);
        }
    }

    private void TraceMouse()
    {
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
    }

    void Update()
    {
        TraceMouse();

        if(Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }
}
