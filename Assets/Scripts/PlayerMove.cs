using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEditor.Experimental.GraphView;
using System.Collections;
using UnityEngine.UI;

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
    [SerializeField] private int _maxHp;

    [Header(("Animation"))]
    [SerializeField] private Animator _animator;

    [SerializeField] private string _runAnimatorKey;
    [SerializeField] private string _jumpAnimatorKey;
    [SerializeField] private string _crouchAnimatorKey;
    [SerializeField] private string _hurtAnimationKey;

    [Header("Casting")]
    [SerializeField] private GameObject _fireBall;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _fireBallSpeed;

    [Header("UI")]
    [SerializeField] private TMP_Text _coinsAmountText;
    [SerializeField] private Slider _hpBar;

    private int _currentHp;
    private float _horizontalDirection;
    private float _verticalDirection;
    private bool _jump;
    private bool _crawl;
    private int _coinsAmount;
    private float _lastPushTime;
    private bool _isCasting;

    public int Coins 
    {
        get => _coinsAmount;
        set
        {
            _coinsAmount = value;
            _coinsAmountText.text = value.ToString();
        }
    } 

    private int CurrentHp
    {
        get => _currentHp;
        set
        {
            if (value > _maxHp)
                value = _maxHp;
            _currentHp = value;
            _hpBar.value = value;
        }
    }

    public bool CanClimb { private get; set; }

    private void Start()
    {
        _hpBar.maxValue = _maxHp;
        CurrentHp = _maxHp;
        Coins = 0;
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

        if (Input.GetKey(KeyCode.E))
        {
            StartCast();
        }

    }

    private void FixedUpdate()
    {
        bool canJump = Physics2D.OverlapCircle(_groundChecker.position, _groundCheckerRadius, _whatIsGround);

        if (_animator.GetBool(_hurtAnimationKey))
        {
            if (Time.time - _lastPushTime > 0.2f && canJump)
                _animator.SetBool(_hurtAnimationKey, false);

            return;
        }
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
        int missingHp = _maxHp - CurrentHp;
        int pointsToAdd = missingHp > hpPoints ? hpPoints : missingHp;
        StartCoroutine(RestoreHp(pointsToAdd));
    }

    private IEnumerator RestoreHp(int pointsToAdd)
    {
        
        while (pointsToAdd != 0)
        {
            pointsToAdd--;
            CurrentHp++;
            yield return new WaitForSeconds(0.09f);
        }
    }

    public void AddArmor(int armorPoints)
    {
        Debug.Log("Armor raised " + armorPoints);
    }

    public void AddCoins(int coins)
    {
        Debug.Log($"You got {coins} coin!");
    }

    public void TakeDamage(int damage, float pushPower = 0, float enemyPosX = 0)
    {
        if (_animator.GetBool(_hurtAnimationKey))
        {
            return;
        }

        CurrentHp -= damage;

        if (_currentHp <= 0)
        {
            Debug.Log("Died");
            gameObject.SetActive(false);
            Invoke(nameof(ReloadScene), 1f);
        }

        if (pushPower != 0)
        {
            _lastPushTime = Time.time;
            int direction = transform.position.x > enemyPosX ? 1 : -1;
            _rigidbody.AddForce(new Vector2(direction * pushPower / 2, pushPower));
            _animator.SetBool(_hurtAnimationKey, true);
        }
    }

    public void StartCast()
    {
        if (_isCasting)
            return;
        _isCasting = true;
        _animator.SetBool("Casting", true);
    }

    private void CastFire()
    {
        GameObject fireball = Instantiate(_fireBall, _firePoint.position, Quaternion.identity);
        fireball.GetComponent<Rigidbody2D>().velocity = transform.right * _fireBallSpeed;
        if(_spriteRenderer.flipX)
        {
            fireball.GetComponent<Rigidbody2D>().velocity = -transform.right * _fireBallSpeed;
            fireball.GetComponent<SpriteRenderer>().flipX = true;
        }
        Destroy(fireball, 5f);
    }

    private void EndCast()
    {
        _isCasting = false;
        _animator.SetBool("Casting", false);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
