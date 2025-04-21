using UnityEngine;
using MyGame.Utils;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State startingState;
    [SerializeField] private float roamingDistanceMax = 7f;
    [SerializeField] private float roamingDistanceMin = 3f;
    [SerializeField] private float roamingTimerMax = 2f;

    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private State state;
    private float roamingTime;
    private Vector3 roamingPosition;
    private Vector3 startingPosition;

    private enum State
    {
        Roaming
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        state = startingState;
    }
    private void Update()
    {
        switch (state)
        {
            default:
            case State.Roaming:
                roamingTime -= Time.deltaTime;
                if (roamingTime <= 0)
                {
                    Roaming();
                    roamingTime = roamingTimerMax;
                }
                break;
        }
    }

    private void Roaming()
    {
        startingPosition = transform.position;
        roamingPosition = GetRoamingPosition();
        ChangeFacingDirection(startingPosition, roamingPosition);
        navMeshAgent.SetDestination(roamingPosition);
    }

    private Vector3 GetRoamingPosition()
    {
        return startingPosition + Utils.GetRandomDirection() * UnityEngine.Random.Range(roamingDistanceMin, roamingDistanceMax);
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
