using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Weapon : MonoBehaviour
{
    public GameObject ammoPrefab;
    private static List<GameObject> ammoPool;

    public int poolSize;

    public float weaponVelocity;

    [HideInInspector] public Animator animator;
    private Camera _localCamera;

    private bool isFiring;
    private float positiveSlope;
    private float negativeSlope;

    private enum Quadrant
    {
        East,
        South,
        West,
        North
    }

    private void Awake()
    {
        if (ammoPool == null)
        {
            ammoPool = new List<GameObject>();
        }

        for (var i = 0; i < poolSize; i++)
        {
            var ammoObject = Instantiate(ammoPrefab);
            ammoObject.SetActive(false);
            ammoPool.Add(ammoObject);
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        isFiring = false;
        _localCamera = Camera.main;

        var lowerLeft = _localCamera.ScreenToWorldPoint(new Vector2(0, 0));
        var upperRight = _localCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        var upperLeft = _localCamera.ScreenToWorldPoint(new Vector2(0, Screen.height));
        var lowerRight = _localCamera.ScreenToWorldPoint(new Vector2(Screen.width, 0));

        positiveSlope = GetSlope(lowerLeft, upperRight);
        negativeSlope = GetSlope(upperLeft, lowerRight);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isFiring = true;
            FireAmmo();
        }

        UpdateState();
    }

    private void UpdateState()
    {
        if (isFiring)
        {
            Vector2 quadrantVector;
            var quadEnum = GetQuadrant();

            switch (quadEnum)
            {
                case Quadrant.East:
                    quadrantVector = new Vector2(1.0f, 0.0f);
                    break;
                
                case Quadrant.South:
                    quadrantVector = new Vector2(0.0f, -1.0f);
                    break;
                
                case Quadrant.West:
                    quadrantVector = new Vector2(-1.0f, 0.0f);
                    break;
                
                case Quadrant.North:
                    quadrantVector = new Vector2(0.0f, 1.0f);
                    break;
                
                default:
                    quadrantVector = new Vector2(0.0f, 0.0f);
                    break;
            }
            
            animator.SetBool("isFiring", true);
            
            animator.SetFloat("fireXDir", quadrantVector.x);
            animator.SetFloat("fireYDir", quadrantVector.y);

            isFiring = false;
        }
        else
        {
            animator.SetBool("isFiring", false);
        }
    }

    private GameObject SpawnAmmo(Vector3 location)
    {
        foreach (var ammo in ammoPool)
        {
            if (ammo.activeSelf == false)
            {
                ammo.SetActive(true);
                ammo.transform.position = location;

                return ammo;
            }
        }

        return null;
    }

    private void FireAmmo()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var ammo = SpawnAmmo(transform.position);

        if (ammo != null)
        {
            var arcScript = ammo.GetComponent<Arc>();
            var travelDuration = 1.0f / weaponVelocity;

            StartCoroutine(arcScript.TravelArc(mousePosition, travelDuration));
        }
    }

    private void OnDestroy()
    {
        ammoPool = null;
    }

    private float GetSlope(Vector2 pointOne, Vector2 pointTwo)
    {
        return (pointTwo.y - pointOne.y) / (pointTwo.x - pointOne.x);
    }

    private bool HigherThanPositiveSlopeLine(Vector2 inputPosition)
    {
        var playerPosition = transform.position;
        var mousePosition = _localCamera.ScreenToWorldPoint(inputPosition);

        var yIntercept = playerPosition.y - (positiveSlope * playerPosition.x);
        var inputIntercept = mousePosition.y - (positiveSlope * mousePosition.x);

        return inputIntercept > yIntercept;
    }

    private bool HigherThanNegativeSlopeLine(Vector2 inputPosition)
    {
        var playerPosition = transform.position;
        var mousePosition = _localCamera.ScreenToWorldPoint(inputPosition);

        var yIntercept = playerPosition.y - (negativeSlope * playerPosition.x);
        var inputIntercept = mousePosition.y - (negativeSlope * mousePosition.x);

        return inputIntercept > yIntercept;
    }

    private Quadrant GetQuadrant()
    {
        var higherThanPositiveSlopeLine = HigherThanPositiveSlopeLine(Input.mousePosition);
        var higherThanNegativeSlopeLine = HigherThanNegativeSlopeLine(Input.mousePosition);

        if (!higherThanPositiveSlopeLine && higherThanNegativeSlopeLine)
        {
            return Quadrant.East;
        }
        else if (!higherThanPositiveSlopeLine && !higherThanNegativeSlopeLine)
        {
            return Quadrant.South;
        }
        else if (higherThanPositiveSlopeLine && !higherThanNegativeSlopeLine)
        {
            return Quadrant.West;
        }
        else
        {
            return Quadrant.North;
        }
    }
}