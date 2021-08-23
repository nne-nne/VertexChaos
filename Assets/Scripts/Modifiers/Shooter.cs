using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    // Start is called before the first frame update
    List<BulletModifier> bms = new List<BulletModifier>();
    public GameObject projectile;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject pr;
        if (Input.GetButton("Fire1"))
        {
            pr = Instantiate(projectile, transform.position, transform.rotation);
            pr.GetComponent<Bullet>().AddModifiers(bms);
        }
        if (Input.GetKeyDown(KeyCode.Q))
            bms.Add(new AddSpeed());
    }
}
