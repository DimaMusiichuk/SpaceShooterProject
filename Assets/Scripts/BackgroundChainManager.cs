using UnityEngine;

public class BackgroundChainManager : MonoBehaviour
{
    [Header("Налаштування руху")]
    [SerializeField] private float scrollSpeed = 3f;
    
    [Header("Список фонів (по порядку)")]
    [SerializeField] private Transform[] backgroundNodes; 

    private float backgroundHeight;
    private float totalChainHeight;

    void Start()
    {
        if (backgroundNodes.Length == 0) return;

        backgroundHeight = backgroundNodes[0].GetComponent<SpriteRenderer>().bounds.size.y;
        
        totalChainHeight = backgroundHeight * backgroundNodes.Length;

        for (int i = 0; i < backgroundNodes.Length; i++)
        {
            backgroundNodes[i].position = new Vector3(transform.position.x, transform.position.y + (i * backgroundHeight), transform.position.z);
        }
    }

    void Update()
    {
        foreach (Transform bg in backgroundNodes)
        {
            bg.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

            if (bg.position.y < transform.position.y - backgroundHeight)
            {
                Vector3 newPos = bg.position;
                newPos.y += totalChainHeight;
                bg.position = newPos;
            }
        }
    }
}