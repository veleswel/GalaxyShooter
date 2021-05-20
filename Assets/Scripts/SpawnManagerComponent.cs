using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerComponent : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private float _spawnInterval = 5f;

    public bool StopSpawning { get; set; } = false;

    private IEnumerator _spawnRoutine;

    void Start()
    {
        _spawnRoutine = SpawnRoutine();
        StartCoroutine(_spawnRoutine);
    }

    IEnumerator SpawnRoutine()
    {
        while (!StopSpawning)
        {
            Rect bounds = Camera.main.GetComponent<CameraBoundsComponent>().Bounds;

            Vector3 position = new Vector3(Random.Range(bounds.xMin, bounds.xMax), bounds.yMax, 0);

            GameObject enemy = Instantiate(_enemyPrefab, position, Quaternion.identity);
            enemy.transform.SetParent(_enemyContainer.transform);

            yield return new WaitForSeconds(_spawnInterval);
        }
    }
}
