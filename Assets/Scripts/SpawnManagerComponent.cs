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
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private float _tripleShotSpawnIntervalMin = 3f;
    
    [SerializeField]
    private float _tripleShotSpawnIntervalMax = 7f;


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
            Rect bounds = Camera.main.GetComponent<CameraBoundsComponent>().Bounds;

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
            float tripleShotSpawnInterval = Random.Range(_tripleShotSpawnIntervalMin, _tripleShotSpawnIntervalMax);

            yield return new WaitForSeconds(tripleShotSpawnInterval);

            Rect bounds = Camera.main.GetComponent<CameraBoundsComponent>().Bounds;

            Vector3 position = new Vector3(Random.Range(bounds.xMin, bounds.xMax), bounds.yMax, 0);

            Instantiate(_tripleShotPrefab, position, Quaternion.identity);
        }
    }
}
