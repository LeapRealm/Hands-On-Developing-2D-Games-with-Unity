using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Wander : MonoBehaviour
{
    public float pursuitSpeed;
    public float wanderSpeed;
    private float currentSpeed;
    
    public float directionChangeInterval;
    public bool followPlayer;
    
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private CircleCollider2D _circleCollider;
    
    private Coroutine moveCoroutine;
    private Transform targetTransform;
    private Vector3 endPosition;
    private float currentAngle;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _circleCollider = GetComponent<CircleCollider2D>();
        
        currentSpeed = wanderSpeed;
        endPosition = transform.position;

        StartCoroutine(WanderRoutine());
    }

    private void Update()
    {
        Debug.DrawLine(_rigidbody.position, endPosition, Color.red);
    }

    private IEnumerator WanderRoutine()
    {
        while (true)
        {
            ChooseNewEndpoint();

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            moveCoroutine = StartCoroutine(Move(_rigidbody, currentSpeed));
            
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    private void ChooseNewEndpoint()
    {
        currentAngle += Random.Range(0, 360);
        currentAngle = Mathf.Repeat(currentAngle, 360);

        endPosition = transform.position + Vector3FromAngle(currentAngle);
    }

    private Vector3 Vector3FromAngle(float inputAngleDegrees)
    {
        var inputAngleRadians = inputAngleDegrees * Mathf.Deg2Rad;

        return new Vector3(Mathf.Cos(inputAngleRadians), Mathf.Sin(inputAngleRadians), 0);
    }

    private IEnumerator Move(Rigidbody2D rigidbodyToMove, float speed)
    {
        var remainingDistance = (transform.position - endPosition).sqrMagnitude;

        while (remainingDistance > float.Epsilon)
        {
            if (targetTransform != null)
            {
                endPosition = targetTransform.position;
            }

            if (rigidbodyToMove != null)
            {
                _animator.SetBool("isWalking", true);

                var newPosition = Vector3.MoveTowards(rigidbodyToMove.position, endPosition, speed * Time.deltaTime);
                rigidbodyToMove.MovePosition(newPosition);

                remainingDistance = (transform.position - endPosition).sqrMagnitude;
            }

            yield return new WaitForFixedUpdate();
        }
        
        _animator.SetBool("isWalking", false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && followPlayer)
        {
            currentSpeed = pursuitSpeed;
            targetTransform = other.transform;

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            moveCoroutine = StartCoroutine(Move(_rigidbody, currentSpeed));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.SetBool("isWalking", false);
            currentSpeed = wanderSpeed;

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            targetTransform = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (_circleCollider != null)
        {
            Gizmos.DrawWireSphere(transform.position, _circleCollider.radius);
        }
    }
}