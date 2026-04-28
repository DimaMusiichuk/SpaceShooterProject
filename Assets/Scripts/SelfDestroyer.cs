using UnityEngine;

public class SelfDestroyer : MonoBehaviour
{
    public float lifetime = 1.0f; 
    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}