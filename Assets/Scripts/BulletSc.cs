using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;


/// <summary>
/// attached to a bullet object
/// allows shooting via public Shoot method
/// </summary>
public class BulletSc : MonoBehaviour
{
    public float damage = 1;
    public float speed = 2000f;
    public float lifetime = 5.0f;
    public int life = 1;
    public float delay = 0;
    private List<BulletModifier> bms;
    private float start_life;
    private Rigidbody rb;
    float base_damage, base_speed, base_lifetime;

    public Affiliation affiliation = Affiliation.Player;

    public Material playerMaterial;
    public Material enemyMaterial;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        base_damage = damage;
        base_speed = speed;
        base_lifetime = lifetime;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        start_life = Time.time;
    }

    public float Shoot()
    {
        //gameObject.SetActive(true);
        //start_life = Time.time;
        rb.AddForce(transform.forward * speed);
        return delay;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.activeInHierarchy)
        {
            foreach (BulletModifier bm in bms)
            {
                bm.update_effect(gameObject);
            }
            //gameObject.transform.Translate(Vector3.forward*speed);
            if (start_life + lifetime < Time.time)
            {
                gameObject.SetActive(false);
            }
        }  
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (BulletModifier bm in bms)
        {
            bm.trigger_effect(gameObject, other);
        }

        switch (affiliation)
        {
            case Affiliation.Player:
                if (other.CompareTag("Enemy"))
                {
                    var enemyController = other.GetComponentInParent<EnemyController>();
                    if (enemyController) { enemyController.ReceiveDamage(damage); }
                    life--;
                }
                break;
            
            case Affiliation.Enemy:
                if (other.CompareTag("Player"))
                {
                    var playerController = other.GetComponentInParent<PlayerController>();
                    if (playerController) { playerController.ReceiveDamage(damage); }
                    life--;
                }
                break;
        }
        
        if (life <= 0)
        {
            gameObject.SetActive(false);
        }
    }


    private void OnDisable()
    {
        if(bms != null)
        {
            foreach (BulletModifier bm in bms)
            {
                bm.destroy_effect(gameObject);
            }
        }
        rb.velocity = Vector3.zero;
        rb.rotation = Quaternion.Euler(Vector3.zero);
        damage = base_damage;
        speed = base_speed;
        lifetime = base_lifetime;
        life = 1;
        delay = 0;
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        affiliation = Affiliation.Player;
    }

    public void AddModifiers(List<BulletModifier> new_bms)
    {
        bms = new List<BulletModifier>(new_bms);

        foreach (BulletModifier bm in bms)
        {
            bm.create_effect(gameObject);
        }
    }

    public List<BulletModifier> GetBulletModifiers()
    {
        return bms;
    }

    public void removeModifier(BulletModifier mod)
    {
        bms.Remove(mod);
    }

    public void SetBulletMaterialToAffiliation()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            switch (affiliation)
            {
                case Affiliation.Player:
                    renderer.material = playerMaterial;
                    break;
                case Affiliation.Enemy:
                    renderer.material = enemyMaterial;
                    break;
            }
        }
    }
}
