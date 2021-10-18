using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Lever_lvl_2 : MonoBehaviour
{
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private Collider2D _closedDoor;

    private SpriteRenderer _spriteRenderer;

    private Sprite _inactiveSprite;


    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _inactiveSprite = _spriteRenderer.sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Mini_car car = collision.GetComponent<Mini_car>();
        if (car != null)
        {
            _spriteRenderer.sprite = _activeSprite;
            _closedDoor.enabled = false;

        }
    }
}
