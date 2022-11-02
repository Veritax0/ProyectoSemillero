using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class BladeEnemyController : MonoBehaviour{
    public List<Transform> points;
    public PlayerController objective;
    public float minDistance;
    private NavMeshAgent _agent;
    private int _currentPosition;
    private float _distance;
    private PatrolEnum _status = PatrolEnum.GUARD;
    private bool _changeDestination;
    public float attackDistance = 1.5f;
    private bool _attacking;
    private GameObject _blade;
    private Animator _animator;
    private static readonly int Attack1 = Animator.StringToHash("attack");

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.SetDestination(points[_currentPosition].position);
        _blade = transform.GetChild(0).gameObject;
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        CheckStatus();
    }

    private void CheckStatus()
    {
        switch (_status)
        {
            case PatrolEnum.GUARD:
                Guard();
                break;
            case PatrolEnum.CHASE:
                Chase();
                break;
            case PatrolEnum.ATTACK:
                StartCoroutine(Attack());
                break;
        }
    }

    private IEnumerator Attack()
    {
        _blade.SetActive(true);
        _agent.SetDestination(transform.position);
        _animator.SetBool(Attack1, true);
        yield return new WaitForSeconds(0.5f);
        _status = PatrolEnum.GUARD;
        _animator.SetBool(Attack1, false);
    }
    
    private void Guard()
    {
        _blade.SetActive(false);
        _distance = Vector3.Distance(transform.position, points[_currentPosition].position);
        if (!_changeDestination && _distance < minDistance)
        {
            StartCoroutine(WaitAndChange());
        }
        else
        {
            _agent.SetDestination(points[_currentPosition].position);
        }
    }

    private void Chase()
    {
        _blade.SetActive(false);
        _agent.SetDestination(objective.transform.position);
        if (transform.position == objective.transform.position)
        {
            _status = PatrolEnum.GUARD;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            _status = PatrolEnum.CHASE;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        float colliderDistance = Vector3.Distance(transform.position, other.transform.position);
        if (colliderDistance < attackDistance)
        {
            _status = PatrolEnum.ATTACK;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _status = PatrolEnum.GUARD;
            _agent.SetDestination(points[_currentPosition].position);
        }
    }

    IEnumerator WaitAndChange()
    {
        _changeDestination = true;
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        _currentPosition = _currentPosition < points.Count - 1 ? _currentPosition + 1 : 0;
        _agent.SetDestination(points[_currentPosition].position);
        _changeDestination = false;
    }
}