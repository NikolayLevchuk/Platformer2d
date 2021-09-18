using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject _player;
    void Update()
    {
        transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y, -1f);
    }
}
