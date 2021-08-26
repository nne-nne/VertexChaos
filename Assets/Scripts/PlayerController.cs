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
    
    protected Affiliation affiliation = Affiliation.Friend;

    public static string PlayerName { get; } = "Player";

    public void ResetRigidbodyVelocity(bool resetLinear = true, bool resetAngular = true)
    {
        if (resetLinear) { rb.velocity = Vector3.zero; }
        if (resetAngular) { rb.angularVelocity = Vector3.zero; }
    }

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

    protected void UpdateRotationToFace(Vector3 targetDirection)
    {
        Quaternion target;

        if (rb.velocity != Vector3.zero)
        {
            target = Quaternion.LookRotation(targetDirection.normalized);
        }
        else
        {
            target = transform.rotation;
        }

        float rotationSpeed = angularSpeed * rb.velocity.magnitude / maxMovSpeed;
        rb.rotation = Quaternion.RotateTowards(transform.rotation, target, rotationSpeed);
    }

    protected void UpdateRotationNoVelocity(Vector3 targetDirection)
    {
        Quaternion target = Quaternion.LookRotation(targetDirection.normalized);

        float rotationSpeed = angularSpeed / maxMovSpeed;
        rb.angularVelocity = Vector3.zero;
        rb.rotation = Quaternion.RotateTowards(transform.rotation, target, rotationSpeed);
    }

    protected void SetRotation(Vector3 targetDirection)
    {
        Quaternion target = Quaternion.LookRotation(targetDirection.normalized);

        const float fullAngle = 180f;
        rb.angularVelocity = Vector3.zero;
        rb.rotation = Quaternion.RotateTowards(transform.rotation, target, fullAngle);
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
