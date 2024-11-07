using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event Action<Enemy> HasDestroy;

    private Vector3 _direction;

    private void Update()
    {
        transform.position += _direction * Time.deltaTime;
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<DestroyZone>(out _))
        {
            HasDestroy?.Invoke(this);
        }
    }
}
