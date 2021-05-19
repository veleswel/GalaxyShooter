using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private float _fireRate = 0.5f;

    private float _nextFire = -1f;

    [SerializeField]
    private int _lives = 3;

    private SpawnManagerComponent _spawnManager;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManagerComponent>();
    }

    void Update()
    {
        Move();
        Fire();
    }

    void Move()
    {
        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.Translate(dir * _speed * Time.deltaTime);

        Rect bounds = Camera.main.GetComponent<CameraBoundsComponent>().Bounds;

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, bounds.yMin, 0.0f), transform.position.z);

        if (transform.position.x >= bounds.xMax)
        {
            transform.position = new Vector3(bounds.xMin, transform.position.y, transform.position.z);
        }
        else if (transform.position.x <= bounds.xMin)
        {
            transform.position = new Vector3(bounds.xMax, transform.position.y, transform.position.z);
        }
    }

    void Fire()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        {
            _nextFire = Time.time + _fireRate;

            BoxCollider box = GetComponent<BoxCollider>();
            
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + box.size.y, transform.position.z);

            Instantiate(_laserPrefab, spawnPosition, Quaternion.identity);
        }
    }

    public void Damage()
    {
        _lives -= 1;

        if (_lives == 0)
        {
            if (_spawnManager != null)
            {
                _spawnManager.StopSpawning = true;
            }

            Destroy(this.gameObject);
        }
    }
}
