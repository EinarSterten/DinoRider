using UnityEngine;
using UnityEngine.InputSystem;

public class Player2MovementScript : MonoBehaviour
{
    public float mouseSensitivity = 10f;
    public float moveSpeed = 2.5f;

    private float xRotation = 0f;
    private float yRotation = 0f;

    void Update()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * mouseSensitivity * Time.deltaTime;

        yRotation += mouseDelta.x;
        xRotation -= mouseDelta.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Stops the backflips
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
