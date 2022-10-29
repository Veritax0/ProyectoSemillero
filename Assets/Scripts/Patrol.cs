using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Patrol : MonoBehaviour{
    public List<Transform> points;
    public GameObject objective;
    public float minDistance;
    private NavMeshAgent _agent;
    private int _currentPosition = 0;
    private float _distance;
    private PatrolEnum _status = PatrolEnum.GUARD;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.SetDestination(points[_currentPosition].position);
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
        }
    }

    private void Guard()
    {
        _distance = Vector3.Distance(transform.position, points[_currentPosition].position);
        if (_distance < minDistance){
            _currentPosition = _currentPosition < points.Count - 1 ? _currentPosition + 1 : 0;
            _agent.SetDestination(points[_currentPosition].position);
        }
    }

    private void Chase()
    {
        _agent.SetDestination(objective.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            _status = PatrolEnum.CHASE;
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
}