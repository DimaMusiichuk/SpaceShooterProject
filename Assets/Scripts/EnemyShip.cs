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

    [Header("Аудіо")]
    public AudioClip shootSound;
    public AudioClip explosionSound;
    private AudioSource audioSource;

    void Start()
    {
        startX = transform.position.x;
        nextFireTime = Time.time + Random.Range(0, fireRate);
        
        audioSource = GetComponent<AudioSource>();
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
                
                // Звук пострілу
                if (shootSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(shootSound);
                }
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
            // Звук вибуху
            if (explosionSound != null)
            {
                AudioSource.PlayClipAtPoint(explosionSound, transform.position);
            }

            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }

            if (GameManager.Instance != null) 
            {
                GameManager.Instance.AddScore(150);
            }
            
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}