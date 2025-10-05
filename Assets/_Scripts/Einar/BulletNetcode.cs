using Unity.Netcode;
using UnityEngine;

public class BulletNetcode : NetworkBehaviour
{
    public string targetTag = "Enemy";

    public void DestroyAfter(float seconds) => Invoke(nameof(SelfDespawn), seconds);

    //public override void OnNetworkSpawn()
    //{
    //    // ---- DEBUG: prove velocity arrived ----
    //    if (IsClient)
    //        Debug.Log($"[BULLET] client velocity = {GetComponent<Rigidbody>().linearVelocity}");
    //}

    void OnCollisionEnter(Collision c)
    {
        if (!IsServer) return;

        if (c.collider.CompareTag(targetTag))
            if (c.collider.TryGetComponent<NetworkObject>(out var enemy))
                enemy.Despawn(true);

        SelfDespawn();
    }

    void SelfDespawn() => GetComponent<NetworkObject>().Despawn(true);
}