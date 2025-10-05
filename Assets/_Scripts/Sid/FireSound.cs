using UnityEngine;

public class FireSound : MonoBehaviour
{
    private AudioSource audioSource;
    private float lastPlayTime;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            audioSource.Play();
        }
    }
}
