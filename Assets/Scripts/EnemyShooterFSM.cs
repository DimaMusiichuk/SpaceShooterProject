using System.Collections;
using UnityEngine;

public class EnemyShooterFSM : MonoBehaviour
{
    public enum EnemyState { MovingToPosition, Shooting, Leaving }

    [Header("Налаштування стану: Moving")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float targetStopY = 3f; 

    [Header("Налаштування стану: Shooting")]
    [SerializeField] private float fireRate = 2f; 
    [SerializeField] private float timeToShoot = 3f;
    [SerializeField] private GameObject bulletPrefab; 
    
    [SerializeField] private Transform[] firePoints; 

    [Header("Налаштування стану: Leaving")]
    [SerializeField] private float leaveSpeed = 5f;

    private EnemyState currentState = EnemyState.MovingToPosition;
    private bool isShootingCoroutineRunning = false;

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.MovingToPosition:
                MoveDown();
                break;
            case EnemyState.Shooting:
                if (!isShootingCoroutineRunning)
                {
                    StartCoroutine(ShootingRoutine());
                }
                break;
            case EnemyState.Leaving:
                LeaveScreen();
                break;
        }
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

        if (transform.position.y <= targetStopY)
        {
            currentState = EnemyState.Shooting;
        }
    }

    private IEnumerator ShootingRoutine()
    {
        isShootingCoroutineRunning = true;
        float currentShootTime = 0f;
        
        while (currentState == EnemyState.Shooting && currentShootTime < timeToShoot)
        {
            if (bulletPrefab && firePoints != null)
            {
                foreach (Transform fp in firePoints)
                {
                    if (fp != null)
                    {
                        Instantiate(bulletPrefab, fp.position, Quaternion.identity);
                    }
                }
            }
            
            yield return new WaitForSeconds(1f / fireRate);
            currentShootTime += (1f / fireRate);
        }
        
        currentState = EnemyState.Leaving;
        isShootingCoroutineRunning = false;
    }

    private void LeaveScreen()
    {
        transform.Translate(new Vector3(1, 1, 0) * leaveSpeed * Time.deltaTime);
    }
}