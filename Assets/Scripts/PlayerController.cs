using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 7;
    public int hp = 3;

    public TextMeshProUGUI hpDisplay;
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;

    public Sprite premiumSkinSprite;

    private bool hasDoubleShot = false;

    void Start()
    {
        if (DatabaseService.Instance != null)
        {
            PlayerData data = DatabaseService.Instance.LoadData();

            hp += data.extraHealth;

            hasDoubleShot = data.hasDoubleShot;

            if (data.hasPremiumSkin)
            {
                if (premiumSkinSprite != null)
                {
                    GetComponent<SpriteRenderer>().sprite = premiumSkinSprite;
                }
                else 
                {
                    GetComponent<SpriteRenderer>().color = Color.yellow; 
                }
            }
        }

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
            if (hasDoubleShot)
            {
                Instantiate(bulletPrefab, transform.position + new Vector3(-0.3f, 0, 0), Quaternion.identity);
                Instantiate(bulletPrefab, transform.position + new Vector3(0.3f, 0, 0), Quaternion.identity);
            }
            else
            {
                Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            }
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
                
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.ProcessGameOverLogic();
                }

                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;

                Invoke("GoToMenu", 0.5f); 
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

    private void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}