using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 12;
    public float lifetime = 3;

    void Start()
{
    transform.rotation = Quaternion.Euler(0, 0, 90); 
    
    Destroy(gameObject, lifetime);
}

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);  
        }
    }
}