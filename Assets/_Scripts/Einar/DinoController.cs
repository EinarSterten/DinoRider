using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(Rigidbody))]
public class DinoController : NetworkBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotSpeed = 100f;

    [Header("Camera (optional 3rd-person)")]
    public Transform cameraTransform;
    public Vector3 cameraOffset = new Vector3(0, 5, -8);
    public float cameraFollowSpeed = 10f;

    private Vector2 input;
    private Rigidbody rb;

    public override void OnNetworkSpawn()
    {
        if (cameraTransform)
            cameraTransform.gameObject.SetActive(IsOwner);
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!IsOwner) return;

        ReadInput();
        // No movement here: handle in FixedUpdate for physics
    }

    private void ReadInput()
    {
        input.x = 0;
        if (Keyboard.current.aKey.isPressed) input.x = -1;
        if (Keyboard.current.dKey.isPressed) input.x = 1;

        input.y = 0;
        if (Keyboard.current.wKey.isPressed) input.y = 1;
        if (Keyboard.current.sKey.isPressed) input.y = -1;
    }

    private void FixedUpdate()
    {
        if (!IsOwner || rb == null) return;

        // Rotate based on horizontal input
        float rotationAmount = input.x * rotSpeed * Time.fixedDeltaTime;
        Quaternion deltaRotation = Quaternion.Euler(0, rotationAmount, 0);
        rb.MoveRotation(rb.rotation * deltaRotation);

        // Move forward/backward based on vertical input
        Vector3 moveDirection = transform.forward * input.y * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDirection);
    }
}