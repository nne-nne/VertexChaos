using Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticShock : PlayerModifier
{
    float cooldown = 0;
    float time_between = 2;

    public override void update_effect()
    {
        base.update_effect();
        
        if(Time.time > cooldown + time_between)
        {
            cooldown = Time.time;

            GameObject Player = GameObject.FindGameObjectsWithTag("Player")[0];
            List<GameObject> Enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
            List<GameObject> nonEnemies = new List<GameObject>();
            foreach (GameObject e in Enemies)
            {
                if(e.GetComponent<Rigidbody>() == null)
                {
                    nonEnemies.Add(e);
                }
            }
            foreach (GameObject e in nonEnemies)
            {
                Enemies.Remove(e);
            }
            Vector3 position = Player.transform.position;
            List<GameObject> excluded = new List<GameObject>();
            List<Vector3> positions = new List<Vector3>();
            positions.Add(position);

            for(int i = 0; i < 3 + (strenght-1)*2; i++)
            {
                
                GameObject closest = null;
                float distance = Mathf.Infinity;
                foreach (GameObject go in Enemies)
                {
                    if (!(excluded.Contains(go)))
                    {
                        Vector3 diff = go.transform.position - position;
                        float curDistance = diff.sqrMagnitude;
                        
                        if (curDistance < distance && curDistance < 1500f)
                        {
                            closest = go;
                            distance = curDistance;
                            
                            
                        }
                    }

                }
                if(!(closest is null))
                {
                    excluded.Add(closest);
                    positions.Add(closest.transform.position);
                    position = closest.transform.position;
                    EnemyController ec = closest.GetComponent<EnemyController>();
                    ec.ReceiveDamage(0.5f + 0.25f * strenght);
                }

            }

            for(int i = 0; i < positions.Count-1; i++)
            {
                GameObject shock =  GameObject.Instantiate(EffectsHolder.instance.Mods["Static"], positions[i], Quaternion.identity);
                shock.transform.LookAt(positions[i + 1]);
                shock.GetComponent<ParticleSystem>().startDelay = i * 0.075f;
            }


        }
    }
}
