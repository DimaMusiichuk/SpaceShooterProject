using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 7;
    public int hp = 3;

    public TextMeshProUGUI hpDisplay;
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;

    void Start()
    {
        UpdateHeartsUI();
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveX, moveY, 0);
        transform.Translate(movement * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1) Time.timeScale = 0;
            else Time.timeScale = 1;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyBullet"))
        {
            hp = hp - 1; 
            
            UpdateHeartsUI(); 

            Destroy(other.gameObject); 

            if (hp <= 0)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject); 
            }
        }
    }

    void UpdateHeartsUI()
    {
        if (hpDisplay != null)
        {
            string hearts = ""; 
            
            for (int i = 0; i < hp; i++)
            {
                hearts += "♥"; 
            }
            
            hpDisplay.text = hearts; 
        }
    }
}