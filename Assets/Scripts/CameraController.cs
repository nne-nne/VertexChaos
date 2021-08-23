using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    [SerializeField] private float cameraSpeed;

    private Vector3 targetPos;

    void Awake()
    {
        targetPos = transform.position;
    }

    void Update()
    {
        //targetPos = Vector3.Lerp(transform.position, 
        //    new Vector3(player.position.x, transform.position.y, player.position.z), cameraSpeed * Time.deltaTime);

    }

    void FixedUpdate()
    {
        //transform.position = targetPos;
        transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
    }
}
