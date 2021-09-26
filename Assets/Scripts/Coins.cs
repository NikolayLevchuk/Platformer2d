using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    [SerializeField] private int _cions;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMove player = collision.GetComponent<PlayerMove>();

        if (player != null)
        {
            player.AddCoins(1);
            Destroy(gameObject);
        }
    }
}
