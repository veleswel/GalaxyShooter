using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;

    private Player _player;

    private CameraBounds _cameraBounds;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        _cameraBounds = Camera.main.GetComponent<CameraBounds>();
    }

    void Update()
    {
        Move();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Laser")
        {
            if (_player != null)
            {
                _player.OnEnemyDestroyed();
            }

            Destroy(collider.gameObject);
            Destroy(this.gameObject);
        }

        if(collider.gameObject.tag == "Player")
        {
            if (_player != null)
            {
                _player.Damage();
            }
            
            Destroy(this.gameObject);
        }
    }

    void Move()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        Rect bounds = _cameraBounds.Bounds;

        if (transform.position.y <= bounds.yMin)
        {
            Vector3 position = new Vector3(Random.Range(bounds.xMin, bounds.xMax), bounds.yMax, 0);
            transform.position = position;
        }
    }
}
