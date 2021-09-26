using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    [SerializeField] private float _speed;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private float _groundCheckerRadius;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private LayerMask _whatIsCell;
    [SerializeField] private Collider2D _headCollider;
    [SerializeField] private float _headCheckerRadius;
    [SerializeField] private Transform _headChecker;

    [Header(("Animation"))]
    [SerializeField] private Animator _animator;

    [SerializeField] private string _runAnimatorKey;
    [SerializeField] private string _jumpAnimatorKey;
    [SerializeField] private string _crouchAnimatorKey;

    private float _horizontalDirection;
    private float _verticalDirection;
    private bool _jump;
    private bool _crawl;

    public bool CanClimb { private get; set; }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _horizontalDirection = Input.GetAxisRaw("Horizontal");
        _verticalDirection = Input.GetAxisRaw("Vertical");

        _animator.SetFloat(_runAnimatorKey, Mathf.Abs(_horizontalDirection));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jump = true;
        }

        if (_horizontalDirection > 0 && _spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_horizontalDirection < 0 && !_spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = true;
        }

        _crawl = Input.GetKey(KeyCode.C);

    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_horizontalDirection * _speed, _rigidbody.velocity.y);

        if (CanClimb)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _verticalDirection * _speed);
            _rigidbody.gravityScale = 0;
        }
        else
        {
            _rigidbody.gravityScale = 4;
        }

        bool canJump = Physics2D.OverlapCircle(_groundChecker.position, _groundCheckerRadius, _whatIsGround);
        bool canStand = !Physics2D.OverlapCircle(_headChecker.position, _headCheckerRadius, _whatIsCell);

        _headCollider.enabled = !_crawl && canStand;

        if (_jump && canJump)
        {
            _rigidbody.AddForce(Vector2.up * _jumpForce);
            _jump = false;
        }

        _animator.SetBool(_jumpAnimatorKey, !canJump);
        _animator.SetBool(_crouchAnimatorKey, !_headCollider.enabled);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundChecker.position, _groundCheckerRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_headChecker.position, _headCheckerRadius);
    }

    //*************************
    public void AddHp(int hpPoints)
    {
        Debug.Log("Hp raised " + hpPoints);
    }

    public void AddArmor(int armorPoints)
    {
        Debug.Log("Armor raised " + armorPoints);
    }

    public void AddCoins(int coins)
    {
        Debug.Log($"You got {coins} coin!");
    }
}
