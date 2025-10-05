using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public string targetTag = "Enemy"; // Tag

    void OnCollisionEnter(Collision collision)
    {
        // DestroyEnemy
        if (collision.gameObject.CompareTag(targetTag))
        {
            Destroy(collision.gameObject);
        }

    }
}