using Audio;
using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Cop : MonoBehaviour
{
    [SerializeField] List<Transform> wayPoint;
    int currentIndex;
    Vector2 currentDirection = Vector2.zero;
    public enum Status { Idle, Chasing, Stun };
    [SerializeField] public Status status;
    [SerializeField] private float chasingSpeed = 2f;
    [SerializeField] private float patrolSpeed = .5f;
    [SerializeField] private GameObject copVisionGo;
    float currentSpeed;

    private Transform _target;
    void Start()
    {
        currentIndex = 0;
        if (wayPoint.Count >= 2)
        {
            transform.position = wayPoint[0].position;
            _target = wayPoint[0];
        }

        currentSpeed = patrolSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsStatic() && status != Status.Stun)
        {
            if (Vector2.Distance(transform.position, _target.position) < .2f && !(status==Status.Chasing))
            {
                if (currentIndex < wayPoint.Count-1)
                    currentIndex++;
                else
                    currentIndex = 0;
                _target = wayPoint[currentIndex];
            }
            transform.position = Vector2.MoveTowards(transform.position, _target.position, currentSpeed * Time.timeScale);
        }
    }

    public void BeginChase(Transform target)
    {
        _target = target;
        status = Status.Chasing;
        currentSpeed = chasingSpeed;
    }

    public Transform GetTarget()
    {
        return _target == null ? transform : _target;
    }
    public bool IsStatic()
    {
        return _target == null;
    }

    public void LookLeft(bool lookLeft)
    {
       transform.DOScaleX(lookLeft ? 1 : -1, 0.01f).SetUpdate(true);       
    }

    public void Stun(int force)
    {
        status = Status.Stun;

        copVisionGo.SetActive(false);
        
        AudioController.Instance.PlayAudio(Audio.AudioType.SFX_Wood);

        Invoke(nameof(Unstun), force);
    }

    private void Unstun()
    {
        copVisionGo.SetActive(true);

        status = Status.Chasing;
    }
}
