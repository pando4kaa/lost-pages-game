using UnityEngine;
using MyGame.Utils;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State _startingState;
    [SerializeField] private float _roamingDistanceMax = 7f;
    [SerializeField] private float _roamingDistanceMin = 3f;
    [SerializeField] private float _roamingTimerMax = 2f;

    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private State _state;
    private float _roamingTime;
    private Vector3 _roamingPosition;
    private Vector3 _startingPosition;

    private enum State
    {
        Roaming
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        _state = _startingState;
    }
    private void Update()
    {
        switch (_state)
        {
            default:
            case State.Roaming:
                _roamingTime -= Time.deltaTime;
                if (_roamingTime <= 0)
                {
                    Roaming();
                    _roamingTime = _roamingTimerMax;
                }
                break;
        }
    }

    private void Roaming()
    {
        _startingPosition = transform.position;
        _roamingPosition = GetRoamingPosition();
        ChangeFacingDirection(_startingPosition, _roamingPosition);
        navMeshAgent.SetDestination(_roamingPosition);
    }

    private Vector3 GetRoamingPosition()
    {
        return _startingPosition + Utils.GetRandomDirection() * UnityEngine.Random.Range(_roamingDistanceMin, _roamingDistanceMax);
    }

    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        if(sourcePosition.x > targetPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
