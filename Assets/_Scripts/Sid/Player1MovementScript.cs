using UnityEngine;
using UnityEngine.InputSystem;

public class Player1MovementScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public float movementSpeed = 5f;
    public float rotationSpeed = 100f;

// The camera dose not work, so I attach it dirrectly. Keeping this in case I can fix and reuse later.
    [Header("Camera Settings")]
    public Transform cameraTransform;
    public Vector3 cameraOffset = new Vector3(0, 5, -8);
    public float cameraFollowSpeed = 5f;

    void Update()
    {
        // Movement (new Input System)
        if (Keyboard.current.wKey.isPressed)
            transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);

        if (Keyboard.current.sKey.isPressed)
            transform.Translate(-Vector3.forward * movementSpeed * Time.deltaTime);

        if (Keyboard.current.aKey.isPressed)
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);

        if (Keyboard.current.dKey.isPressed)
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
