using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupID { TripleShot, Speed, Shield };

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;

    [SerializeField]
    private PowerupID _powerupID = PowerupID.TripleShot;

    CameraBounds _cameraBounds;
    void Start()
    {
        _cameraBounds = Camera.main.GetComponent<CameraBounds>();
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        Rect bounds = _cameraBounds.Bounds;

        if (transform.position.y <= bounds.yMin)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            Player player = collider.gameObject.GetComponent<Player>();
            
            if (player != null)
            {
                player.ActivatePowerup(_powerupID);
            }
            
            Destroy(this.gameObject);
        }
    }
}
