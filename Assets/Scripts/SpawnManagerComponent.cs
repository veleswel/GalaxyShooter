using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerComponent : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private float _enemySpawnInterval = 5f;

    [SerializeField]
    private GameObject[] _powerups;

    [SerializeField]
    private float _powerupSpawnIntervalMin = 7f;
    
    [SerializeField]
    private float _powerupSpawnIntervalMax = 15f;

    public bool StopSpawning { get; set; } = false;

    private IEnumerator _enemySpawnRoutine;

    private IEnumerator _powerupSpawnRoutine;

    void Start()
    {
        _enemySpawnRoutine = EnemySpawnRoutine();
        StartCoroutine(_enemySpawnRoutine);

        _powerupSpawnRoutine = PowerupSpawnRoutine();
        StartCoroutine(_powerupSpawnRoutine);
    }

    IEnumerator EnemySpawnRoutine()
    {
        while (!StopSpawning)
        {
            Rect bounds = Camera.main.GetComponent<CameraBounds>().Bounds;

            Vector3 position = new Vector3(Random.Range(bounds.xMin, bounds.xMax), bounds.yMax, 0);

            GameObject enemy = Instantiate(_enemyPrefab, position, Quaternion.identity);
            enemy.transform.SetParent(_enemyContainer.transform);

            yield return new WaitForSeconds(_enemySpawnInterval);
        }
    }

    IEnumerator PowerupSpawnRoutine()
    {
        while (!StopSpawning)
        {
            float powerupSpawnInterval = Random.Range(_powerupSpawnIntervalMin, _powerupSpawnIntervalMax);

            yield return new WaitForSeconds(powerupSpawnInterval);

            Rect bounds = Camera.main.GetComponent<CameraBounds>().Bounds;
            Vector3 position = new Vector3(Random.Range(bounds.xMin, bounds.xMax), bounds.yMax, 0);

            int powerupID = Random.Range(0, _powerups.Length);

            Instantiate(_powerups[powerupID], position, Quaternion.identity);
        }
    }
}
