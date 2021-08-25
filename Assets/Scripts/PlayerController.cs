using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// WASD/arrows controller for player movement
/// 
/// </summary>
public class PlayerController : MonoBehaviour, ITarget
{
    [SerializeField] private float maxMovSpeed, acceleration, angularSpeed;

    private Rigidbody rb;
    private Vector2 speed, direction;
    private Quaternion rotation;

    protected Vector2 Direction
    {
        get => direction;
        set => direction = value;
    }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected void UpdateMovementSpeed()
    {
        if (direction != Vector2.zero) direction.Normalize();

        speed = Vector2.Lerp(speed, direction * maxMovSpeed, acceleration * Time.deltaTime);
    }

    protected void UpdateRotation()
    {
        Quaternion target;

        if (rb.velocity != Vector3.zero)
        {
            target = Quaternion.LookRotation(rb.velocity.normalized);
        }
        else
        {
            target = transform.rotation;
        }

        float rotationSpeed = angularSpeed * rb.velocity.magnitude / maxMovSpeed;
        rb.rotation = Quaternion.RotateTowards(transform.rotation, target, rotationSpeed);
    }

    protected virtual void Update()
    {
        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        UpdateMovementSpeed();
        UpdateRotation();
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector3(speed.x, 0f, speed.y);
    }

    Vector3 ITarget.GetPosition()
    {
        return transform.position;
    }
}
