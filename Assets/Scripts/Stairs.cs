using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMove player = collision.GetComponent<PlayerMove>();
        if (collision != null)
        {
            player.CanClimb = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerMove player = collision.GetComponent<PlayerMove>();
        if (collision != null)
        {
            player.CanClimb = false;
        }
    }
}
