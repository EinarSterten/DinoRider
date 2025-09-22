using UnityEngine;
using UnityEngine.InputSystem; // new system

public class MouseMoveObject : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float moveSpeed = 5f;

    private float xRotation = 0f;
    private float yRotation = 0f;

    void Update()
    {
        // Get mouse delta
        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * mouseSensitivity * Time.deltaTime;

        yRotation += mouseDelta.x;
        xRotation -= mouseDelta.y;
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
