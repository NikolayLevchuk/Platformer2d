using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMover : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    [SerializeField] private float _speed;
    [SerializeField] private SpriteRenderer spriteRend;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private float _groundCheckerRadius;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private Collider2D _headCollider;
    [SerializeField] private float _headCheckingRadius;
    [SerializeField] private Transform _headChecker;

    private float _direction;
    private bool _jump;
    private bool _crawl;


    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    
    private void Update()
    {
        _direction = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jump = true;
            
        }

        if (_direction > 0 && spriteRend.flipX)
        {
            spriteRend.flipX = false;
        }
        else if (_direction < 0 && !spriteRend.flipX)
        {
            spriteRend.flipX = true;
        }

        _crawl = Input.GetKey(KeyCode.C);       
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_direction * _speed, _rigidbody.velocity.y);

        bool canJump = Physics2D.OverlapCircle(_groundChecker.position, _groundCheckerRadius, _whatIsGround);
        bool canStand = !Physics2D.OverlapCircle(_headChecker.position, _headCheckingRadius, _whatIsGround);


        _headCollider.enabled = !_crawl && canStand;

        if (_jump && canJump)
        {

            _rigidbody.AddForce(Vector2.up * _jumpForce);
            _jump = false;
        }       
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundChecker.position, _groundCheckerRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_headChecker.position, _headCheckingRadius);
    }
}
