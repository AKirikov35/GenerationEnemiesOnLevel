using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    private ObjectPool<Enemy> pool;
    private Vector3 direction;

    private void Update()
    {
        transform.position += direction * Time.deltaTime;
    }

    public void Initialize(ObjectPool<Enemy> pool)
    {
        this.pool = pool;
    }

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    public void ReturnToPool()
    {
        pool?.Release(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<DestroyZone>(out _))
        {
            ReturnToPool();
        }
    }
}