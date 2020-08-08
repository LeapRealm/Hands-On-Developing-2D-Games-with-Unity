using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float movementSpeed = 3.0f;
    private Vector2 _movementDirection;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateState();
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        _movementDirection.x = Input.GetAxisRaw("Horizontal");
        _movementDirection.y = Input.GetAxisRaw("Vertical");
        
        _movementDirection.Normalize();

        _rigidbody2D.velocity = _movementDirection * movementSpeed;
    }

    private void UpdateState()
    {
        if (Mathf.Approximately(_movementDirection.x, 0) && Mathf.Approximately(_movementDirection.y, 0))
        {
            _animator.SetBool("isWalking", false);
        }
        else
        {
            _animator.SetBool("isWalking", true);
        }
        
        _animator.SetFloat("xDir", _movementDirection.x);
        _animator.SetFloat("yDir", _movementDirection.y);
    }
}