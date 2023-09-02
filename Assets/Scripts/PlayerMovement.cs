using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private float _extraJumpValue;
    [SerializeField] private float _checkRadius;
    [SerializeField] private float _jumpForce;
   

    private Vector2 _moveInput;
    private bool _facingRight = true;
    private Rigidbody2D _rigidbody2D;
    private float _extraJump;
    private float _speed = 5f;
    private bool _isGround;
    private const int MAX_HEALTH = 3;


    public GameObject[] hearts;


    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        Jump();

        MoveKeybord();
        Flip();
    }

    private void MoveKeybord()
    {
        _moveInput.x = Input.GetAxis("Horizontal");
        _rigidbody2D.velocity = new Vector2(_moveInput.x * _speed, _rigidbody2D.velocity.y);
    }


    private void Jump()
    {
        if (_isGround == true)
        {
            _extraJump = _extraJumpValue;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _extraJump > 0)

        {
            _rigidbody2D.velocity = Vector2.up * _extraJump;
            _extraJump--;
        }


        else if (Input.GetKeyDown(KeyCode.Space) && _extraJump == 0 && _isGround == true)

        {
            _rigidbody2D.velocity = Vector2.up * _jumpForce;
        }

        _isGround = Physics2D.OverlapCircle(_groundCheck.position, _checkRadius, _whatIsGround);
    }


    private void Flip()
    {
        if (_moveInput.x > 0 && _facingRight == false || _moveInput.x < 0 && _facingRight == true)
        {
            var transform1 = transform;
            Vector3 theScale = transform1.localScale;
            theScale.x *= -1f;
            transform1.localScale = theScale;

            _facingRight = !_facingRight;
        }
    }
}