using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    [SerializeField] private float mouseFollowRadiusFrac;
    [SerializeField] private float cameraSwingMultiplier;

    private float mouseFollowRadius;

    private Vector3 targetPos;

    void Awake()
    {
        targetPos = transform.position;
        mouseFollowRadius = Screen.height * mouseFollowRadiusFrac;
    }

    private Vector3 FindCameraTarget()
    {
        Vector2 offset = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);
        if (Mathf.Abs(offset.x) > mouseFollowRadius) offset.x = mouseFollowRadius * Mathf.Sign(offset.x);
        if (Mathf.Abs(offset.y) > mouseFollowRadius) offset.y = mouseFollowRadius * Mathf.Sign(offset.y);
        offset *= cameraSwingMultiplier;
        Debug.Log(offset);
        return new Vector3(offset.x, 0f, offset.y);
    }

    void Update()
    {
        //targetPos = Vector3.Lerp(transform.position, 
        //    new Vector3(player.position.x, transform.position.y, player.position.z), cameraSpeed * Time.deltaTime);

    }

    void FixedUpdate()
    {
        //targetPos = Vector3.Lerp(transform.position,
        //    FindCameraTarget(), cameraSpeed);
        transform.position = new Vector3(player.position.x, transform.position.y, player.position.z) + FindCameraTarget();
        //transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
    }
}
