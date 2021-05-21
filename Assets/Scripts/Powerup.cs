using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        Rect bounds = Camera.main.GetComponent<CameraBoundsComponent>().Bounds;

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
                player.ActivateTripleShot();
            }
            
            Destroy(this.gameObject);
        }
    }
}
