using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallScr : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int _damage;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = -transform.right * speed;
        Destroy(gameObject, 5f);
    }

    
    void Update()
    {
        
    }
    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMove player = collision.GetComponent<PlayerMove>();

        if (player != null)
        {
            player.TakeDamage(_damage, 3000, transform.position.x);
        }
        Destroy();
    }
}
