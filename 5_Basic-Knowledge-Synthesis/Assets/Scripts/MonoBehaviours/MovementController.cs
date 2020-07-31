using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float movementSpeed = 3.0f;
    private Vector2 _movementDirection;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private string _animationState = "AnimationState";

    private enum CharacterStates
    {
        walkEast = 1,
        walkSouth = 2,
        walkWest = 3,
        walkNorth = 4,
        idleSouth = 5
    }

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
        if (_movementDirection.x > 0)
        {
            _animator.SetInteger(_animationState, (int)CharacterStates.walkEast);
        }
        else if (_movementDirection.x < 0)
        {
            _animator.SetInteger(_animationState, (int)CharacterStates.walkWest);
        }
        else if (_movementDirection.y > 0)
        {
            _animator.SetInteger(_animationState, (int)CharacterStates.walkNorth);
        }
        else if (_movementDirection.y < 0)
        {
            _animator.SetInteger(_animationState, (int)CharacterStates.walkSouth);
        }
        else
        {
            _animator.SetInteger(_animationState, (int)CharacterStates.idleSouth);
        }
    }
}