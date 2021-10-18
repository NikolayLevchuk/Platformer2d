using UnityEngine;

public class Crab : MonoBehaviour
{
    [SerializeField] private float _walkRange;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _speed;
    [SerializeField] private bool _faceRight = true;
    [SerializeField] private int _damage;
    [SerializeField] private float _pushPower;
    private Vector2 _startPosition;
    private int _direction = 1;

    private float _lastAttakTime;
    private Vector2 _drawPosition
    {
        get
        {
            if (_startPosition == Vector2.zero)
                return transform.position;
            else
                return _startPosition;
        }
    }
    private void Start()
    {
        _startPosition = transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(_drawPosition, new Vector3(_walkRange * 2, 1, 0));
    }

    private void Update()
    {
        if (_faceRight && transform.position.x > _startPosition.x + _walkRange)
        {
            Flip();
        }
        else if (!_faceRight && transform.position.x < _startPosition.x - _walkRange)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = Vector2.right * _direction * _speed;
    }

    private void Flip()
    {
        _faceRight = !_faceRight;
        transform.Rotate(0, 180, 0);
        _direction *= -1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMove player = collision.collider.GetComponent<PlayerMove>();
        if (player != null && Time.time - _lastAttakTime > 0.2f)
        {
            _lastAttakTime = Time.time;
            player.TakeDamage(_damage, _pushPower, transform.position.x);
        }
    }
}