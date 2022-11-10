using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class BladeEnemyController : MonoBehaviour{
    
    [Header("Guard config")]
    public List<Transform> points;
    public PlayerController objective;
    public float minDistanceToChangePoint;
    
    [Header("Atack config")]
    public float attackDistance = 1.5f;
    public float chaseDistance = 10f;
    
    private EnemyStateEnum _status = EnemyStateEnum.GUARD;
    private NavMeshAgent _agent;
    private GameObject _blade;
    private Animator _animator;
    private RaycastHit _hit;
    private int _currentPosition;
    private float _distance;
    private bool _changeDestination;
    private bool _attacking;
    private bool _isHit;
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
            case EnemyStateEnum.GUARD:
                Guard();
                break;
            case EnemyStateEnum.CHASE:
                Chase();
                break;
            case EnemyStateEnum.ATTACK:
                StartCoroutine(Attack());
                break;
        }
    }

    private void Guard()
    {
        if (_isHit)
        {
            if (_hit.transform.gameObject.CompareTag("Player")){
                        _status = EnemyStateEnum.CHASE;
                        return;
            }
        }
        _blade.SetActive(false);
        _distance = Vector3.Distance(transform.position, points[_currentPosition].position);
        if (!_changeDestination && _distance < minDistanceToChangePoint)
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
        switch (_isHit)
        {
            case false:
                _status = EnemyStateEnum.GUARD;
                _agent.SetDestination(points[_currentPosition].position);
                return;
            case true when !_hit.transform.gameObject.CompareTag("Player"):
                _status = EnemyStateEnum.GUARD;
                _agent.SetDestination(points[_currentPosition].position);
                return;
            case true:
                _blade.SetActive(false);
                Vector3 objPos = objective.transform.position;
                _agent.SetDestination(objPos);
                float distance = Vector3.Distance(transform.position,objPos);
                if (distance <= minDistanceToChangePoint)
                {
                    _status = EnemyStateEnum.GUARD;
                }
                float objDistance = Vector3.Distance(transform.position, objPos);
                if (objDistance < attackDistance)
                {
                    _status = EnemyStateEnum.ATTACK;
                }
                break;
        }
    }

    private IEnumerator WaitAndChange()
    {
        _changeDestination = true;
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        _currentPosition = _currentPosition < points.Count - 1 ? _currentPosition + 1 : 0;
        _agent.SetDestination(points[_currentPosition].position);
        _changeDestination = false;
    }
    private IEnumerator Attack()
    {
        _blade.SetActive(true);
        _agent.SetDestination(transform.position);
        _animator.SetBool(Attack1, true);
        yield return new WaitForSeconds(0.7f);
        _status = EnemyStateEnum.GUARD;
        _animator.SetBool(Attack1, false);
    }

    private void OnDrawGizmos()
    {
        Vector3 objPos = objective.transform.position;
        Vector3 position = transform.position;
        Vector3 direction = Vector3.Normalize(objPos - position);

        _isHit = Physics.SphereCast(position, transform.lossyScale.x / 2, 
            direction, out _hit, chaseDistance);
        if (_isHit)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(position, direction * _hit.distance);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(position, direction * chaseDistance);
        }
    }
}
