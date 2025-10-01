using UnityEngine;
using UnityEngine.InputSystem;

public class ShootingScript : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject projectilePrefab;   // Bullet Prefab
    public Transform firePoint;           // BulletDispenser
    public float Velocity = 20f;   // Velocity

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Spawn
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Force
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * Velocity, ForceMode.Impulse);

        //Kill your self
        Destroy(projectile, 5f);
    }
}
