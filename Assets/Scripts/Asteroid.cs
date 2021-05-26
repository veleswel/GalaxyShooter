using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 15f;

    private GameObject _explosion;

    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * (Time.deltaTime * _rotationSpeed));
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Laser")
        {
            _animator.SetTrigger("OnAsteroidExplosion");
            Destroy(collider.gameObject);
        }
    }

    void OnExplosionAnimationEnded()
    {
        Destroy(this.gameObject);
    }
}
