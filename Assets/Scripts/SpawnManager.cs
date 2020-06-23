using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _EnemyObj;
    [SerializeField]
    private GameObject _EnemyContainer;

    [SerializeField]
    private GameObject[] PowerUps;

    // Start is called before the first frame update

    private bool _stopSpawning = false;

    private bool _stopPowerUp = false;



    public void StartSpawnManager()
    {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerUpSpawningRoutine());
    }


    IEnumerator EnemySpawnRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (!_stopSpawning)
        {
            GameObject newEnemy = Instantiate(_EnemyObj, new Vector3(Random.Range(-9.0f, 9.0f), 6.0f,0), Quaternion.identity);
            newEnemy.transform.parent = _EnemyContainer.transform;

            yield return new WaitForSeconds(3.0f);

        }
    }


    IEnumerator PowerUpSpawningRoutine()
    {       
        while (!_stopPowerUp)
        {
            yield return new WaitForSeconds(Random.Range(10, 20));
            GameObject PowerUpSpawning = Instantiate(PowerUps[Random.Range(0, 6)], new Vector3(Random.Range(-9.0f, 9.0f), 6, 0), Quaternion.identity);
        }
    }


    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    public void PowerUpStop()
    {
        _stopPowerUp = true;
    }
}
