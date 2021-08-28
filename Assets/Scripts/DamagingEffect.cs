using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class DamagingEffect : MonoBehaviour
{
    public float damage;
    public Affiliation affiliation = Affiliation.Player;
    private void OnTriggerEnter(Collider other)
    {

        switch (affiliation)
        {
            case Affiliation.Player:
                if (other.CompareTag("Enemy"))
                {
                    var enemyController = other.GetComponentInParent<EnemyController>();
                    if (enemyController) { enemyController.ReceiveDamage(damage); }
                }
                break;

            case Affiliation.Enemy:
                if (other.CompareTag("Player"))
                {
                    var playerController = other.GetComponentInParent<PlayerController>();
                    if (playerController) { playerController.ReceiveDamage(damage); }
                }
                break;
        }

    }

}
