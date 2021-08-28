using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.Events;
using System;

public class CannonController : MonoBehaviour
{
    public Transform pivotPoint;
    public GameObject[] for_mods;
    public float fireRate;
    [SerializeField] private float cooldownTime;
    private List<BulletModifier> bms;
    private float cooldown;

    public void PrintBms()
    {
        Debug.Log("there are " + bms.Count + "modifiers");
        foreach(BulletModifier bulletModifier in bms)
        {
            Debug.Log(bulletModifier.GetType());
        }
    }

    public UnityEvent ShootEvent { get; set; } = new UnityEvent();

    void Start()
    {
        bms = new List<BulletModifier>();
        cooldown = 0.0f;
    }

    private void ManageCooldown()
    {
        cooldown -= Time.deltaTime;
    }

    private void ResetCooldown(float bullet_cooldown)
    {
        cooldown = fireRate * (cooldownTime + bullet_cooldown);
    }

    private void Shoot()
    {
        float cool = 0;
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
                    bulletSc.affiliation = affiliation;
                    bulletSc.Activate();
                    bulletSc.AddModifiers(bms);
                    cool = bulletSc.Shoot();

                    ShootEvent.Invoke();
                }
            }
            ResetCooldown(cool);
        }
        
    }

    private void TraceMouse()
    {
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
    }

    public void AddBulletModifier(BulletModifier mod)
    {
        bool exists = false;
        BulletModifier existingOne = null;
        Type typ = mod.GetType();
        foreach (BulletModifier go in bms)
        {
            if (go.GetType() == typ)
            {
                exists = true;
                existingOne = go;
            }
        }
        if (exists)
            existingOne.strenght++;
        else
            bms.Add(mod);
        
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
            AddBulletModifier(new CanonModifier());
        if (Input.GetKeyDown(KeyCode.E))
            AddBulletModifier(new PierceModifier());
        if (Input.GetKeyDown(KeyCode.R))
            AddBulletModifier(new FourWayShootModifier());
        if (Input.GetKeyDown(KeyCode.T))
            AddBulletModifier(new ScatterModifier());
        if (Input.GetKeyDown(KeyCode.P))
            AddBulletModifier(new HomingBullet());
        if (Input.GetKeyDown(KeyCode.F))
            AddBulletModifier(new DamagingAuraModifier(for_mods));
        if (Input.GetKeyDown(KeyCode.G))
            AddBulletModifier(new ShieldModifier());
        if (Input.GetKeyDown(KeyCode.H))
            AddBulletModifier(new ExplosionModifier(for_mods));
        if (Input.GetKeyDown(KeyCode.L))
            AddBulletModifier(new TargetterModifier());
    }

    private Affiliation affiliation = Affiliation.Player;
}
