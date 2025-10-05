using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NetworkObject))]
public class DinoController : NetworkBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotSpeed = 100f;

    [Header("Camera (optional 3rd-person)")]
    public Transform cameraTransform;
    public Vector3 cameraOffset = new Vector3(0, 5, -8);
    public float cameraFollowSpeed = 10f;

    private Vector2 input;      // WASD only

    public override void OnNetworkSpawn()
    {
        if (cameraTransform)
            cameraTransform.gameObject.SetActive(IsOwner);
    }

    void Update()
    {
        if (!IsOwner) return;

        ReadInput();
        Move();
        UpdateCamera();
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

    private void Move()
    {
        transform.Rotate(0, input.x * rotSpeed * Time.deltaTime, 0);

        float forward = Mathf.Clamp01(input.y);               // no reverse
        transform.Translate(0, 0, forward * moveSpeed * Time.deltaTime, Space.Self);
    }

    private void UpdateCamera()
    {
        if (!cameraTransform) return;

        Vector3 targetPos = transform.position + transform.TransformDirection(cameraOffset);
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPos, cameraFollowSpeed * Time.deltaTime);
        cameraTransform.LookAt(transform);
    }
}