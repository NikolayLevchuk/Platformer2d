using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _damageDelay;
    private float _lastDamageTime;
    private PlayerMove _player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMove player = collision.GetComponent<PlayerMove>();
        if (player != null)
            _player = player;        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerMove player = collision.GetComponent<PlayerMove>();
        if (player == _player)
            _player = null;
    }

    private void Update()
    {
        if (_player != null && Time.time - _lastDamageTime > _damageDelay)
        {
            _lastDamageTime = Time.time;
            _player.TakeDamage(_damage);
        }
    }
}
