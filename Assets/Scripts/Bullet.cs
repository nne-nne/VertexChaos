using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    List<BulletModifier> bms;
    public float damage = 1;
    public float speed = 0.1f;
    public float lifetime = 5.0f;
    float start_life;
    
    void Start()
    {
        start_life = Time.time;
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.AddRelativeForce(Vector3.forward * speed);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (BulletModifier bm in bms)
        {
            bm.update_effect(gameObject);
        }
        //gameObject.transform.Translate(Vector3.forward*speed);
        if(start_life + lifetime < Time.time)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyScript>();
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        foreach (BulletModifier bm in bms)
        {
            bm.destroy_effect(gameObject);
        }
    }

    public void AddModifiers(List<BulletModifier> new_bms)
    {
        bms = new List<BulletModifier> (new_bms);

        foreach (BulletModifier bm in bms)
        {
            bm.create_effect(gameObject);
        }
    }
}
