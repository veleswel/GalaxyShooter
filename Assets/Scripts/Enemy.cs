using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;

    private Player _player;

    private CameraBounds _cameraBounds;

    private Animator _animator;

    private bool _isDestroying = false;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        _cameraBounds = Camera.main.GetComponent<CameraBounds>();

        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Laser")
        {
            Destroy();

            if (_player != null)
            {
                _player.OnEnemyDestroyed();
            }

            Destroy(collider.gameObject);
        }

        if(collider.gameObject.tag == "Player")
        {
            Destroy();
            
            if (_player != null)
            {
                _player.Damage();
            }
        }
    }

    void Move()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        Rect bounds = _cameraBounds.Bounds;

        if (transform.position.y <= bounds.yMin && !_isDestroying)
        {
            Vector3 position = new Vector3(Random.Range(bounds.xMin, bounds.xMax), bounds.yMax, 0);
            transform.position = position;
        }
    }

    void Destroy()
    {
        _isDestroying = true;
        GetComponent<Collider2D>().enabled = false;
        _animator.SetTrigger("OnEnemyDestroyed");
    }

    public void OnDestroyAnimationEnded()
    {
        Destroy(this.gameObject);
    }
}
