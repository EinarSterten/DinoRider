using Unity.Netcode;
using UnityEngine;

public class Player1Controller : NetworkBehaviour
{
    public Camera playerCamera;

    void Start()
    {
        if (IsLocalPlayer)
        {
            // Enable camera only for local player
            playerCamera.gameObject.SetActive(true);
        }
        else
        {
            // Disable for remote players
            playerCamera.gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if (!IsOwner) return;

        Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;

        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}
