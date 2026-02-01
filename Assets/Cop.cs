using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Cop : MonoBehaviour
{
    [SerializeField] List<Transform> wayPoint;
    Vector2 currentDirection = Vector2.zero;
    public enum Status { Idle, Chasing };
    [SerializeField] public Status status;
    [SerializeField] private float chasingSpeed = 2f;
    [SerializeField] private float patrolSpeed = .5f;

    private Transform _target;
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (status == Status.Chasing)
        {
            transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, chasingSpeed * Time.timeScale);
        }

        if (wayPoint.Count >= 2)
        {

        }
    }

    public void BeginChase(Transform target)
    {
        _target = target;
        status = Status.Chasing;
    }
}
