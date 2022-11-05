using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class GunEnemyController : MonoBehaviour
{  
    public float minDistance;
    public List<Transform> points;
    public PlayerController objective;
    public float attackDistance = 10f;
    public Transform pointer;
    public Transform bulletGen;
    public GameObject bullet;
    
    private EnemyStateEnum _status = EnemyStateEnum.GUARD;
    private float _distance;
    private int _currentPosition;
    private NavMeshAgent _agent;
    private bool _changeDestination;
    private bool _isHit;
    private RaycastHit _hit;
    private Animator _animator;
    private bool _isAimWalk;
    private Coroutine _aimWalkCor;
    private bool _isAim;
    private float aimDistance;

    //Estados: Guardia, apuntar (por unos segundos) y disparar
    // Start is called before the first frame update
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
            case EnemyStateEnum.AIMFIRE:
                Aim();
                break;
        }
    }

    private void Aim()
    {
        if (!_isAim)
        {
            StartCoroutine(AimAndFire());
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
        if (!_isAimWalk)
        {
            _aimWalkCor = StartCoroutine(AimWalking());
        }
        _distance = Vector3.Distance(transform.position, points[_currentPosition].position);
        if (!_changeDestination && _distance < minDistance)
        {
            StopCoroutine(_aimWalkCor);
            StartCoroutine(WaitAndChange());
        }
        else
        {
            _agent.SetDestination(points[_currentPosition].position);
        }
    }
    
    private void Chase()
    {
        _isHit = Physics.SphereCast(bulletGen.position, transform.lossyScale.x / 2, 
            objective.transform.position, out _hit, aimDistance);
        if (_isHit)
        {
            if (!_hit.transform.gameObject.CompareTag("Player")){
                _status = EnemyStateEnum.GUARD;
                return;
            }
            Vector3 objPos = objective.transform.position;
            _agent.SetDestination(objPos);
            float distance = Vector3.Distance(transform.position,objPos);
            if (distance <= attackDistance)
            {
                _status = EnemyStateEnum.AIMFIRE;
            }
            else
            {
                _status = EnemyStateEnum.CHASE;
            }
        }
        else
        {
            _status = EnemyStateEnum.GUARD;
        }
    }
    
    private IEnumerator WaitAndChange()
    {
        _changeDestination = true;
        _animator.SetInteger("Aim", 0);
        yield return new WaitForFixedUpdate();
        _animator.SetInteger("Aim", 1);
        yield return new WaitForSeconds(3.7f);
        _animator.SetInteger("Aim", 0);
        _currentPosition = _currentPosition < points.Count - 1 ? _currentPosition + 1 : 0;
        _agent.SetDestination(points[_currentPosition].position);
        _changeDestination = false;
        _isAimWalk = false;
    }

    private IEnumerator AimWalking()
    {
        _isAimWalk = true;
        _animator.SetInteger("Aim", 2);
        yield return new WaitForSeconds(Random.Range(2f, 3f));
        _animator.SetInteger("Aim", 0);
        _isAimWalk = false;
    }
    
    private IEnumerator AimAndFire()
    {
        _isAim = true;
        _agent.SetDestination(transform.position);
        Vector3 aim = objective.transform.position;
        yield return new WaitForSeconds(1);
        Fire(aim);
        _isAim = false;
    }

    private void Fire(Vector3 aim)
    {
        GameObject bulletInstantiate = Instantiate(bullet, bulletGen);
        Vector3 dir = Vector3.Normalize(pointer.position - bulletInstantiate.transform.position);
        float force = 20;
        bulletInstantiate.GetComponent<Rigidbody>().AddForce(dir * force,ForceMode.Impulse);
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
            if(_hit.transform.CompareTag("Pointer"))Gizmos.color = Color.yellow;
            else Gizmos.color = Color.red;
            Debug.Log("hit: " + _hit.transform.position);
            
            Gizmos.DrawRay(bulletGenPos, direction * aimDistance);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(bulletGenPos, direction * aimDistance);
        }

        if (_isHit)
        {
            if (_hit.transform.CompareTag("Player"))
            {
                Debug.Log("hit jugador  ");
            }
        }
    }
}
