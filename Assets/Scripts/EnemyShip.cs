using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    public float speed = 2f;
    public float tiltSpeed = 2f;
    public float tiltAmount = 3f;

    public GameObject bulletPrefab; 
    public Transform shootPoint;   
    public float fireRate = 2f;     
    private float nextFireTime;

    public GameObject explosionPrefab;
    private float startX;

    void Start()
    {
        startX = transform.position.x;
        nextFireTime = Time.time + Random.Range(0, fireRate);
    }

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;

        float currentX = startX + Mathf.Sin(Time.time * tiltSpeed) * tiltAmount;
        transform.position = new Vector3(currentX, transform.position.y, transform.position.z);

        if (Time.time >= nextFireTime)
        {
            if (bulletPrefab != null && shootPoint != null)
            {
                Instantiate(bulletPrefab, shootPoint.position, Quaternion.Euler(0, 0, 180));
            }
            nextFireTime = Time.time + fireRate;
        }

        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}