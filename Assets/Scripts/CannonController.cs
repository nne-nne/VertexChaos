using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public Transform pivotPoint;
    [SerializeField] private float cooldownTime;
    private List<BulletModifier> bms;
    private float cooldown;

    void Start()
    {
        bms = new List<BulletModifier>();
        cooldown = 0.0f;
    }

    private void ManageCooldown()
    {
        cooldown -= Time.deltaTime;
    }

    private void ResetCooldown()
    {
        cooldown = cooldownTime;
    }

    private void Shoot()
    {
        if (cooldown <= 0.0f)
        {
            GameObject bullet = ObjectPool.SharedInstance.GetPooledObject();
            if (bullet != null)
            {
                bullet.transform.position = pivotPoint.position;
                bullet.transform.rotation = transform.rotation;

                BulletSc bulletSc = bullet.GetComponent<BulletSc>();

                if (bulletSc != null)
                {
                    //following method sets bullet active in hierarchy
                    bulletSc.Shoot();
                    bulletSc.AddModifiers(bms);
                }
            }
            ResetCooldown();
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
        ManageCooldown();

        if(Input.GetButton("Fire1"))
        {
            Shoot();
        }

        ///DO DEBUGOWANIA
        ///~PATRYK
        if (Input.GetKeyDown(KeyCode.Q))
            bms.Add(new AddSpeed());
    }
}
