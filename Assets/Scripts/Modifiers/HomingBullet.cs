using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet : BulletModifier
{
    public override void update_effect(GameObject bullet)
    {
        Rigidbody bullet_rb = bullet.GetComponent<Rigidbody>();
        Vector3 direction = ClosestEnemyDirection(bullet);
        bullet.transform.Translate(direction.normalized*30*Time.deltaTime);
        
        //direction = new Vector3(ReversedSigmoid(direction.x), ReversedSigmoid(direction.y), ReversedSigmoid(direction.z)) * 100;
        //bullet_rb.AddForce(direction);
    }

    public override string show_message()
    {
        return "Homing bullets";
    }

    public Vector3 ClosestEnemyDirection(GameObject bullet)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = bullet.transform.position;
        foreach (GameObject go in gos)
        {
            
            float curDistance = Vector3.Distance(go.transform.position, bullet.transform.position);
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        Vector3 direction = closest.transform.position - position;
        return direction;
    }

    public static float ReversedSigmoid(float value)
    {
        float val = Mathf.Sign(value) / (1.0f + (float)Mathf.Exp(Mathf.Abs(value) - 7));
        //if (Mathf.Abs(val) < 0.9f) val = 0;
        return (val);
    }
}
