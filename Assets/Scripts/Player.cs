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

    private Vector2 _playerSize;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManagerComponent>();

        _playerSize = GetComponent<BoxCollider2D>().size * transform.localScale;
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

        float y = Mathf.Clamp(transform.position.y, bounds.yMin + _playerSize.y / 2, 0.0f - _playerSize.y / 2);

        transform.position = new Vector3(transform.position.x, y, transform.position.z);

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

            float playerVerticalOffset = _playerSize.y / 2;
            float laserVerticalOffset = (_laserPrefab.GetComponent<BoxCollider2D>().size * _laserPrefab.transform.localScale).y / 2;

            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + playerVerticalOffset - laserVerticalOffset, transform.position.z);

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
