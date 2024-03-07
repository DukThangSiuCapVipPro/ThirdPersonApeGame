using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Tooltip("The distance at which the enemy considers that it has reached its current path destination point")]
    public float PathReachingRadius = 2f;
    [Tooltip("The speed at which the enemy rotates")]
    public float OrientationSpeed = 10f;

    public List<GameObject> dropItems;

    public PatrolPath PatrolPath { get; set; }
    public NavMeshAgent NavMeshAgent { get; private set; }


    public DetectionModule DetectionModule { get; private set; }
    public GameObject KnownDetectedTarget => DetectionModule.KnownDetectedTarget;
    public bool IsTargetInAttackRange => DetectionModule.IsTargetInAttackRange;
    public bool IsSeeingTarget => DetectionModule.IsSeeingTarget;

    int m_PathDestinationNodeIndex;

    private void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        var navigationModules = GetComponentsInChildren<DetectionModule>();
        if (navigationModules.Length > 0)
        {
            DetectionModule = navigationModules[0];
            NavMeshAgent.speed = DetectionModule.MoveSpeed;
            NavMeshAgent.angularSpeed = DetectionModule.AngularSpeed;
            NavMeshAgent.acceleration = DetectionModule.Acceleration;
        }
    }

    public bool TryAtack(Vector3 enemyPosition)
    {
        // Shoot the weapon
        bool didFire = true;

        return didFire;
    }
    public void OrientTowards(Vector3 lookPosition)
    {
        Vector3 lookDirection = Vector3.ProjectOnPlane(lookPosition - transform.position, Vector3.up).normalized;
        if (lookDirection.sqrMagnitude != 0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation =
                Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * OrientationSpeed);
        }
    }
    bool IsPathValid()
    {
        return PatrolPath && PatrolPath.PathNodes.Count > 0;
    }

    public void ResetPathDestination()
    {
        m_PathDestinationNodeIndex = 0;
    }

    public void SetPathDestinationToClosestNode()
    {
        if (IsPathValid())
        {
            int closestPathNodeIndex = 0;
            for (int i = 0; i < PatrolPath.PathNodes.Count; i++)
            {
                float distanceToPathNode = PatrolPath.GetDistanceToNode(transform.position, i);
                if (distanceToPathNode < PatrolPath.GetDistanceToNode(transform.position, closestPathNodeIndex))
                {
                    closestPathNodeIndex = i;
                }
            }

            m_PathDestinationNodeIndex = closestPathNodeIndex;
        }
        else
        {
            m_PathDestinationNodeIndex = 0;
        }
    }
    public Vector3 GetDestinationOnPath()
    {
        if (IsPathValid())
        {
            return PatrolPath.GetPositionOfPathNode(m_PathDestinationNodeIndex);
        }
        else
        {
            return transform.position;
        }
    }
    public void SetNavDestination(Vector3 destination)
    {
        if (NavMeshAgent)
        {
            NavMeshAgent.SetDestination(destination);
        }
    }
    public void UpdatePathDestination(bool inverseOrder = false)
    {
        if (IsPathValid())
        {
            // Check if reached the path destination
            if ((transform.position - GetDestinationOnPath()).magnitude <= PathReachingRadius)
            {
                // increment path destination index
                m_PathDestinationNodeIndex =
                    inverseOrder ? (m_PathDestinationNodeIndex - 1) : (m_PathDestinationNodeIndex + 1);
                if (m_PathDestinationNodeIndex < 0)
                {
                    m_PathDestinationNodeIndex += PatrolPath.PathNodes.Count;
                }

                if (m_PathDestinationNodeIndex >= PatrolPath.PathNodes.Count)
                {
                    m_PathDestinationNodeIndex -= PatrolPath.PathNodes.Count;
                }
            }
        }
    }

    public void SpawnDropItems()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        foreach (GameObject obj in dropItems)
        {
            SimplePool.Spawn(obj, pos, Quaternion.identity);
        }
    }
}
