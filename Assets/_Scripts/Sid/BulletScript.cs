using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public string targetTag = "Enemy"; // Tag

    void OnCollisionEnter(Collision collision)
    {
        // If hit enemy → destroy enemy + bullet
        if (collision.gameObject.CompareTag(targetTag))
        {
            Destroy(collision.gameObject);
        }

    }
}