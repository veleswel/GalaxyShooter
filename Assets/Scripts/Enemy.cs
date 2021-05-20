using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    void Start()
    {
       
    }

    void Update()
    {
        Move();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Laser")
        {
            Destroy(collider.gameObject);
            Destroy(this.gameObject);
        }

        if(collider.gameObject.tag == "Player")
        {
            Player player = collider.gameObject.GetComponent<Player>();
            
            if (player != null)
            {
                player.Damage();
            }
            
            Destroy(this.gameObject);
        }
    }

    void Move()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        Rect bounds = Camera.main.GetComponent<CameraBoundsComponent>().Bounds;

        if (transform.position.y <= bounds.yMin)
        {
            Vector3 position = new Vector3(Random.Range(bounds.xMin, bounds.xMax), bounds.yMax, 0);
            transform.position = position;
        }
    }
}
