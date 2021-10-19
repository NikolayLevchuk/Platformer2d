using UnityEngine;
using UnityEngine.UI;

public class BirdEnemy : MonoBehaviour
{
    [SerializeField] private float _flyRange;
    [SerializeField] private bool _faceRight;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _speed;
    [SerializeField] private int _maxHp;
    [SerializeField] private Slider _slider;
    [SerializeField] private GameObject _enemySystem;
    [SerializeField] private Rigidbody2D _canvasRigidbody;
    [SerializeField] private Transform _point;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _whatIsPlayer;
    [SerializeField] private Transform _shootpoint;
    [SerializeField] private Rigidbody2D _rocket;
    [SerializeField] private float _rocketSpeed;

    private int _currentHp;
    private bool _canShoot;
    private float _verticalDirection;

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
        Gizmos.DrawWireCube(_drawPosition, new Vector3(_flyRange * 2, 1, 0));

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(1, _attackRange, 0));
    }


    private void Update()
    {
        _verticalDirection = Input.GetAxisRaw("Vertical");
        _canvasRigidbody.transform.position = _point.transform.position;

        if (_faceRight && transform.position.x > _startPosition.x + _flyRange)
        {
            Flip();
        }
        else if (!_faceRight && transform.position.x < _startPosition.x - _flyRange)
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

    public void TakeDamage(int damage)
    {
        CurrentHp -= damage;
        if (CurrentHp <= 0)
        {
            Destroy(_enemySystem);
        }
    }

    private void CheckIfCanShoot()
    {
        Collider2D player = Physics2D.OverlapBox(transform.position, new Vector2(1, _attackRange), 0, _whatIsPlayer);

        if (player != null)
        {
            _canShoot = true;
        }
        else
        {
            _canShoot = false;
        }

    }

    public void Shoot()
    {
        Rigidbody2D Rocket = Instantiate(_rocket, _shootpoint.position, Quaternion.identity);
        //Rocket.velocity = _rocketSpeed * transform.up;
        Rocket.velocity = new Vector2(_rocket.velocity.x, _verticalDirection * _rocketSpeed);
        //_rocket.velocity = new Vector2(_rocket.velocity.x, _verticalDirection * _rocketSpeed);
        //Invoke(nameof(CheckIfCanShoot), 1f);
    }
}
