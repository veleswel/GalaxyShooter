using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _speedBoostMultiplier = 2f;

    [SerializeField]
    private int _enemyKillScoreMin;
    [SerializeField]
    private int _enemyKillScoreMax;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shieldPrefab;

    private GameObject _shield;
    private IEnumerator _shieldDownRoutine;

    [SerializeField]
    private float _fireRate = 0.5f;
    private float _nextFire = -1f;

    [SerializeField]
    private int _lives = 3;
    private int _score = 0;

    [SerializeField]
    private float _laserSpawnVerticalOffset = 1.0f;

    private SpawnManagerComponent _spawnManager;

    private UIManager _UIManager;

    private GameManager _gameManager;

    private Vector2 _playerSize;

    private CameraBounds _cameraBounds;

    private bool _isTripleShotEnabled = false;
    private bool _isSpeedBoostEnabled = false;
    private bool _isShieldEnabled = false;

    [SerializeField]
    private float _powerDownTime = 5f;

    void Start()
    {
        _score = 0;
        _lives = 3;

        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManagerComponent>();

        _UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _UIManager.UpdateScoreUI(_score);
        _UIManager.UpdateLivesUI(_lives);

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        _playerSize = GetComponent<BoxCollider2D>().size * transform.localScale;

        _cameraBounds = Camera.main.GetComponent<CameraBounds>();
    }

    void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        {
           Fire();
        }
    }

    void Move()
    {
        float speed = _isSpeedBoostEnabled ? _speed * _speedBoostMultiplier : _speed;
        
        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.Translate(dir * speed * Time.deltaTime);

        Rect bounds = _cameraBounds.Bounds;

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
        _nextFire = Time.time + _fireRate;

        if (_isTripleShotEnabled)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y + _laserSpawnVerticalOffset, transform.position.z), Quaternion.identity);
        }
    }

    public void Damage()
    {
        if (_isShieldEnabled)
        {
            ResetShield();
            return;
        }

        _lives -= 1;

        _UIManager.UpdateLivesUI(_lives);

        if (_lives == 0)
        {
            if (_spawnManager != null)
            {
                _spawnManager.StopSpawning = true;
            }

            _gameManager.GameOver();

            Destroy(this.gameObject);
        }
    }

    public void ActivatePowerup(PowerupID powerupID)
    {
        switch (powerupID)
        {
            case PowerupID.TripleShot:
                _isTripleShotEnabled = true;
                StartCoroutine(TripleShotPowerDownRoutine());
                break;
            case PowerupID.Speed:
                _isSpeedBoostEnabled = true;
                StartCoroutine(SpeedPowerDownRoutine());
                break;
            case PowerupID.Shield:
                if (_isShieldEnabled)
                {
                    ResetShield();
                }

                StartShield();
                break;
            default:
                Debug.LogError("Wrong powerupID: " + powerupID.ToString());
                break;
        }
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(_powerDownTime);
        _isTripleShotEnabled = false;
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(_powerDownTime);
        _isSpeedBoostEnabled = false;
    }

    IEnumerator ShieldPowerDownRoutine()
    {
        yield return new WaitForSeconds(_powerDownTime);
        ResetShield();
    }

    void ResetShield()
    {
        _isShieldEnabled = false;
        
        Destroy(_shield);

        if (_shieldDownRoutine != null)
        {
            StopCoroutine(_shieldDownRoutine);
            _shieldDownRoutine = null;
        }
    }

    void StartShield()
    {
        _isShieldEnabled = true;

        _shield = Instantiate(_shieldPrefab, transform.position, Quaternion.identity);
        _shield.transform.SetParent(transform);

        _shieldDownRoutine = ShieldPowerDownRoutine();
        StartCoroutine(_shieldDownRoutine);
    }

    public void OnEnemyDestroyed()
    {
        AddScore(UnityEngine.Random.Range(_enemyKillScoreMin, _enemyKillScoreMax));
    }

    void AddScore(int score)
    {
        _score += score;

        _UIManager.UpdateScoreUI(_score);
    }
}
