using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    [SerializeField] private int _armorPoints;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMove player = collision.GetComponent<PlayerMove>();
        if (player != null)
        {
            player.AddArmor(_armorPoints);
            Destroy(gameObject);          
        }
    }
}
