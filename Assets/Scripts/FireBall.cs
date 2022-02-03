using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _lifeTime;
    [SerializeField] private GameObject explotionEffect;

    private void Start()
    {
        Invoke(nameof(Destroy), _lifeTime);
    }

    private void Destroy()
    {
        Instantiate(explotionEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Crab enemy = collision.GetComponent<Crab>();
        EnemyStriker enemyStriker = collision.GetComponent<EnemyStriker>(); 
        if(enemy != null)
        {
            enemy.TakeDamage(_damage);
        }
        if (enemyStriker != null)
        {
            enemyStriker.TakeDamage(_damage);
        }
        Destroy();
    }
}
