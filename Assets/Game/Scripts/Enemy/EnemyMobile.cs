using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMobile : MonoBehaviour
{
    public enum AIState
    {
        Patrol,
        Follow,
        Attack,
    }
    public AIState AiState { get; private set; }

    [Tooltip("Fraction of the enemy's attack range at which it will stop moving towards target while attacking")]
    [Range(0f, 1f)]
    public float AttackStopDistanceRatio = 0.5f;

    EnemyController m_EnemyController;
    public Animator Animator;
    public Transform handTrans;
    public LayerMask enemyLayer;

    private void Start()
    {
        m_EnemyController = GetComponent<EnemyController>();
        m_EnemyController.SetPathDestinationToClosestNode();
        m_EnemyController.onAttack += OnAttack;
        m_EnemyController.onDetectedTarget += OnDetectedTarget;
        m_EnemyController.onLostTarget += OnLostTarget;
        m_EnemyController.onDamaged += OnDamaged;
        AiState = AIState.Patrol;
    }

    private void Update()
    {
        UpdateAiStateTransitions();
        UpdateCurrentAiState();

        //float moveSpeed = m_EnemyController.NavMeshAgent.velocity.magnitude;
    }
    void UpdateAiStateTransitions()
    {
        // Handle transitions 
        switch (AiState)
        {
            case AIState.Follow:
                // Transition to attack when there is a line of sight to the target
                if (m_EnemyController.IsSeeingTarget && m_EnemyController.IsTargetInAttackRange)
                {
                    AiState = AIState.Attack;
                    m_EnemyController.SetNavDestination(transform.position);
                }

                break;
            case AIState.Attack:
                // Transition to follow when no longer a target in attack range
                if (!m_EnemyController.IsTargetInAttackRange)
                {
                    AiState = AIState.Follow;
                }

                break;
        }
    }

    void UpdateCurrentAiState()
    {
        // Handle logic 
        switch (AiState)
        {
            case AIState.Patrol:
                m_EnemyController.UpdatePathDestination();
                m_EnemyController.SetNavDestination(m_EnemyController.GetDestinationOnPath());
                //Animator.SetFloat("Speed", 2);
                //Animator.SetFloat("MotionSpeed", 1);
                transform.LookAt(m_EnemyController.NavMeshAgent.destination);
                break;
            case AIState.Follow:
                m_EnemyController.SetNavDestination(m_EnemyController.KnownDetectedTarget.transform.position);
                m_EnemyController.OrientTowards(m_EnemyController.KnownDetectedTarget.transform.position);
                break;
            case AIState.Attack:
                if (Vector3.Distance(m_EnemyController.KnownDetectedTarget.transform.position,
                        m_EnemyController.DetectionModule.DetectionSourcePoint.position)
                    >= (AttackStopDistanceRatio * m_EnemyController.DetectionModule.AttackRange))
                {
                    m_EnemyController.SetNavDestination(m_EnemyController.KnownDetectedTarget.transform.position);
                }
                else
                {
                    m_EnemyController.SetNavDestination(transform.position);
                }

                m_EnemyController.OrientTowards(m_EnemyController.KnownDetectedTarget.transform.position);
                m_EnemyController.TryAtack();
                OnAttack();
                break;
        }
    }

    bool attacking = false;
    void OnAttack()
    {
        if (attacking)
            return;
        attacking = true;
        Animator.SetTrigger("attack");
    }

    void OnDetectedTarget()
    {
        if (AiState == AIState.Patrol)
        {
            AiState = AIState.Follow;
        }
        //Animator.SetBool(k_AnimAlertedParameter, true);
    }

    void OnLostTarget()
    {
        if (AiState == AIState.Follow || AiState == AIState.Attack)
        {
            AiState = AIState.Patrol;
        }
        //Animator.SetBool(k_AnimAlertedParameter, false);
    }

    void OnDamaged()
    {
        m_EnemyController.SpawnDropItems();
        gameObject.SetActive(false);
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {

    }
    public void OnAttackDone(AnimationEvent animationEvent)
    {
        Collider[] cols = Physics.OverlapSphere(handTrans.position, 1f, enemyLayer);
        foreach (var col in cols)
        {
            col.SendMessage("OnDamaged", m_EnemyController.score);
        }
        attacking = false;
    }
}
