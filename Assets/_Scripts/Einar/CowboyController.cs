using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NetworkObject))]
public class CowboyController : NetworkBehaviour
{
    [Header("Look")]
    public float mouseSensitivity = 10f;
    public float minPitch = -90f;
    public float maxPitch = 90f;

    [Header("Shoot")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 20f;

    private float pitch;
    private float yaw;

    public override void OnNetworkSpawn()
    {
        // enable camera only for the rider
        GetComponentInChildren<Camera>(true).gameObject.SetActive(IsOwner);
    }

    private void Update()
    {
        if (!IsOwner) return;          // P2 only

        HandleLook();
        HandleShoot();
    }

    private void HandleLook()
    {
        Vector2 delta = Mouse.current.delta.ReadValue() * mouseSensitivity * Time.deltaTime;

        yaw += delta.x;
        pitch -= delta.y;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    private void HandleShoot()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
            ShootServerRpc();
    }

    [ServerRpc]
    private void ShootServerRpc()
    {
        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        NetworkObject netBullet = bullet.GetComponent<NetworkObject>();
        netBullet.Spawn();                       // replicate to everyone

        // ➜ wait 1 frame so every client has the object, then set velocity
        StartCoroutine(SetVelocityNextFrame(bullet));
    }

    private IEnumerator SetVelocityNextFrame(GameObject bullet)
    {
        yield return null;                                          // 1 frame
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = firePoint.forward * projectileSpeed;
        bullet.GetComponent<BulletNetcode>().DestroyAfter(5f);     // start despawn timer
    }
}
