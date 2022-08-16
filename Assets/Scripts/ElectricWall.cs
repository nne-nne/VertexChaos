using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricWall : MonoBehaviour
{
    // Start is called before the first frame update
    public static float damage = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerController = other.GetComponentInParent<PlayerController>();
            if (playerController) { playerController.ReceiveDamage(damage);}
        }
    }
}
