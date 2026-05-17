using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float fallSpeed = 3;
    public GameObject explosionPrefab;

    [Header("Аудіо")]
    public AudioClip explosionSound;

    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
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
                GameManager.Instance.AddScore(50);
            }

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}