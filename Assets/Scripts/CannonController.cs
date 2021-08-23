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
                Debug.Log("adding force");
                bulletRb.velocity = bullet.transform.forward * bulletForce;
            }

            bullet.SetActive(true);
        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }
}
