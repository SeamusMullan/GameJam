using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float baseSpeed = 1f;
    [SerializeField] private float turbulenceAmount = 0.5f;
    [SerializeField] private float turbulenceFrequency = 1f;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationAmount = 2f;
    [SerializeField] private float rotationSpeed = 0.5f;

    [Header("Physics Effects")]
    [SerializeField] private bool affectPhysicsObjects = true;
    [SerializeField] private float gravityModifier = 0.1f;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private float timeOffset;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        timeOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        ApplyShipMovement();
    }

    private void ApplyShipMovement()
    {
        float time = Time.time + timeOffset;

        Vector3 position = startPosition;
        position.y += Mathf.Sin(time * turbulenceFrequency) * turbulenceAmount;
        position.x += Mathf.Cos(time * turbulenceFrequency * 0.7f) * turbulenceAmount * 0.5f;
        transform.position = position;

        Vector3 rotation = startRotation.eulerAngles;
        rotation.z = Mathf.Sin(time * rotationSpeed) * rotationAmount;
        rotation.x = Mathf.Cos(time * rotationSpeed * 0.8f) * rotationAmount * 0.5f;
        transform.rotation = Quaternion.Euler(rotation);
    }

    public Vector3 GetShipVelocity()
    {
        float time = Time.time + timeOffset;
        Vector3 velocity = Vector3.zero;
        velocity.y = Mathf.Cos(time * turbulenceFrequency) * turbulenceAmount * turbulenceFrequency;
        return velocity;
    }
}
