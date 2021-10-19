using UnityEngine;
using UnityEngine.UI;

public class Crab : MonoBehaviour
{
    [SerializeField] private float _walkRange;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _speed;
    [SerializeField] private bool _faceRight = true;
    [SerializeField] private int _damage;
    [SerializeField] private float _pushPower;
    [SerializeField] private int _maxHp;
    [SerializeField] private Slider _slider;
    [SerializeField] private GameObject _enemySystem;
    [SerializeField] private Rigidbody2D _canvasRigidbody;
    [SerializeField] private Transform _point;

    private int _currentHp;
    private int CurrentHp
    {
        get => _currentHp;
        set 
        {
            _currentHp = value;
            _slider.value = value;
        }
    }
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
        _slider.maxValue = _maxHp;
        CurrentHp = _maxHp;        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_drawPosition, new Vector3(_walkRange * 2, 1, 0));
    }
    
    private void Update()
    {
        _canvasRigidbody.transform.position = _point.transform.position;

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

    public void TakeDamage(int damage)
    {
        CurrentHp -= damage;
        if (CurrentHp <= 0)
        {
            Destroy(_enemySystem);
        }
    }
}