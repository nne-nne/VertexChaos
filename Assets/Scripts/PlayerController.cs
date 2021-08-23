using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// WASD/arrows controller for player movement
/// 
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float maxRotSpeed, maxMovSpeed, movAcc, rotAcc, brake, drag, angDrag, inertia;

    private Rigidbody rb;
    private float speed, rotSpeed;

    private Vector3 prevVelocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        prevVelocity = Vector3.zero;
    }

    void UpdateMovementSpeed()
    {
        float verticalInput = Input.GetAxis("Vertical");

        if (verticalInput > 0.0f)
        {
            speed = Mathf.Lerp(speed, maxMovSpeed, movAcc * Time.deltaTime);
        }
        else if (verticalInput == 0.0f)
        {
            speed = Mathf.Lerp(speed, 0.0f, drag * Time.deltaTime);
        }
        else
        {
            speed = Mathf.Lerp(speed, 0.0f, brake * Time.deltaTime);
        }
    }

    void UpdateRotationSpeed()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput > 0.0f)
        {
            rotSpeed = Mathf.Lerp(rotSpeed, maxRotSpeed, rotAcc * Time.deltaTime);
        }
        else if (horizontalInput == 0.0f)
        {
            rotSpeed = Mathf.Lerp(rotSpeed, 0.0f, angDrag * Time.deltaTime);
        }
        else
        {
            rotSpeed = Mathf.Lerp(rotSpeed, -maxRotSpeed, rotAcc * Time.deltaTime);
        }
    }

    void Update()
    {
        UpdateMovementSpeed();
        UpdateRotationSpeed();
    }

    void FixedUpdate()
    {
        rb.angularVelocity = new Vector3(0f, rotSpeed, 0f);
        
        if(prevVelocity != Vector3.zero)
        {
            rb.velocity = Vector3.LerpUnclamped(transform.forward, prevVelocity.normalized, inertia) * speed;
        }
        else
        {
            rb.velocity = transform.forward * speed;
        }
        prevVelocity = rb.velocity;
    }
}
