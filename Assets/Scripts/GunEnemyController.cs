using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GunEnemyController : MonoBehaviour
{  
    [Header("Guard config")]
    public List<Transform> points;
    public float minDistanceToChangePoint;
    
    [Header("Gun config")]
    public Transform pointer;
    public Transform bulletGen;
    public GameObject bullet;
    
    [Header("Attack config")]
    public PlayerController objective;
    public float aimTime;
    public float aimDistance;
    public float attackDistance = 10f;
    
    private EnemyStateEnum _status = EnemyStateEnum.GUARD;
    private RaycastHit _hit;
    private Animator _animator;
    private NavMeshAgent _agent;
    private Coroutine _aimWalkCor;
    private Coroutine _aimAroundCor;
    private float _distanceToObjective;
    private float _distanceToNextPoint;
    private int _currentPosition;
    private bool _changeDestination;
    private bool _isAimWalk;
    private bool _isHit;
    private bool _isAim;
    private static readonly int Aim1 = Animator.StringToHash("Aim");

    
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.SetDestination(points[_currentPosition].position);
        _animator = GetComponent<Animator>();
        aimDistance = Vector3.Distance(pointer.position, bulletGen.position);
    }

    // Update is called once per frame
    void Update()
    {
        CheckStatus();
        if (_isHit)
        {
            Debug.Log(_hit.transform.tag);
        }
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
            case EnemyStateEnum.AIMANDSHOOT:
                Aim();
                break;
        }
    }

    private void Aim()
    {
        if (!_isAim)
        {
            StartCoroutine(AimAndShoot());
        }
    }

    private void Guard()
    {
        if (_isHit)
        {
            if (_hit.transform.gameObject.CompareTag("Player")){
                StopCoroutine(_aimWalkCor);
                StopCoroutine(_aimAroundCor);
                ChangeStatus(EnemyStateEnum.CHASE);
                return;
            }
        }
        if (!_isAimWalk)
        {
            _aimWalkCor = StartCoroutine(AimWalking());
        }
        _distanceToNextPoint = Vector3.Distance(transform.position, points[_currentPosition].position);
        if (!_changeDestination && _distanceToNextPoint < minDistanceToChangePoint)
        {
            StopCoroutine(_aimWalkCor);
            _aimAroundCor = StartCoroutine(AimAround());
        }
        else
        {
            _agent.SetDestination(points[_currentPosition].position);
        }
    }
    
    private void Chase()
    {
        Vector3 pos = transform.position;
        Vector3 direction = Vector3.Normalize(objective.transform.position - pos);
        _isHit =  Physics.SphereCast(pos, transform.lossyScale.x / 2, 
            direction, out _hit, aimDistance);
       if (_isHit)
        {
            if (!_hit.transform.gameObject.CompareTag("Player")){
                ChangeStatus(EnemyStateEnum.GUARD);
                return;
            }
            Vector3 objPos = objective.transform.position;
            _agent.SetDestination(objPos);
            _distanceToObjective = Vector3.Distance(transform.position,objPos);
            ChangeStatus(_distanceToObjective <= attackDistance ? EnemyStateEnum.AIMANDSHOOT : EnemyStateEnum.CHASE);
        }
        else
        {
            ChangeStatus(EnemyStateEnum.GUARD);
        }
    }
    
    private IEnumerator AimAround()
    {
        _changeDestination = true;
        _animator.SetInteger(Aim1, 0);
        yield return new WaitForFixedUpdate();
        _animator.SetInteger(Aim1, 1);
        yield return new WaitForSeconds(3.7f);
        _animator.SetInteger(Aim1, 0);
        _currentPosition = _currentPosition < points.Count - 1 ? _currentPosition + 1 : 0;
        _agent.SetDestination(points[_currentPosition].position);
        _changeDestination = false;
        _isAimWalk = false;
    }

    private IEnumerator AimWalking()
    {
        _isAimWalk = true;
        _animator.SetInteger(Aim1, 2);
        yield return new WaitForSeconds(Random.Range(2f, 3f));
        _animator.SetInteger(Aim1, 0);
        _isAimWalk = false;
    }
    
    private IEnumerator AimAndShoot()
    {
        _isAim = true;
        _agent.SetDestination(transform.position);
        yield return new WaitForSeconds(aimTime);
        Shoot();
        ChangeStatus(EnemyStateEnum.GUARD);
    }

    private void Shoot()
    {
        GameObject bulletInstantiate = Instantiate(bullet, bulletGen);
        Vector3 dir = Vector3.Normalize(pointer.position - bulletGen.position);
        float force = 20;
        Rigidbody rb = bulletInstantiate.GetComponent<Rigidbody>();
        bulletInstantiate.transform.SetParent(null);
        rb.AddForce(dir * force,ForceMode.Impulse);
        Destroy(bulletInstantiate, 3);
    }
    
    private void OnDrawGizmos()
    {
        Vector3 pointerPos = pointer.position;
        Vector3 bulletGenPos = bulletGen.position;
        Vector3 direction = Vector3.Normalize(pointerPos - bulletGenPos);
        
        _isHit = Physics.SphereCast(bulletGenPos, transform.lossyScale.x / 2, 
            direction, out _hit, aimDistance);
        if (_isHit)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(bulletGenPos, direction * aimDistance);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(bulletGenPos, direction * aimDistance);
        }
    }

    private void ChangeStatus(EnemyStateEnum state)
    {
        _status = state;
        _animator.SetInteger(Aim1, 0);
        _isAim = false;
        _isAimWalk = false;
        _changeDestination = false;
    }
}
