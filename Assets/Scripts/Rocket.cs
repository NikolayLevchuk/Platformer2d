using UnityEngine;

public class Rocket : MonoBehaviour
{

    [SerializeField] private int _damage;
    [SerializeField] private float _lifeTime;

    private void Start()
    {
        Invoke(nameof(Destroy), _lifeTime);
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
            player.TakeDamage(_damage);
        }
        Destroy();
    }
}
