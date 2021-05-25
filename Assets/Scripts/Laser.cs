using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    [SerializeField]
    private float _speed = 25.0f;

    CameraBounds _cameraBounds;

    void Start()
    {
        _cameraBounds = Camera.main.GetComponent<CameraBounds>();
    }

    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        Rect bounds = _cameraBounds.Bounds;

        if (transform.position.y >= bounds.yMax || transform.position.y <= bounds.yMin)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }
}
