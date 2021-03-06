﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyBehaviour : MonoBehaviour
{
    public enum EnemyState { Idle, Patrol, Chase, Attack, Stun, Dead }
    public EnemyState state;

    private NavMeshAgent agent;
    public Transform targetTransform;
    [Header("Patch")]
    public Transform[] points;
    private int pathIndex = 0;
    [Header("Distance")]
    public float chaseRange;
    public float attackRange;
    private float distanceFromTarget = Mathf.Infinity;
    [Header("Timers")]
    public float idleTime = 1; //deberia ser privada
    private float timeCounter = 0;
    public float stunTime = 1;
    public float coolDownAttack = 0.5f;
    [Header("Stats")]
    [SerializeField] private bool canAttack = false;
    [Header("Properties")]
    private int hitDamage = 10;
    public int life = 30;
	[Header("Animations")]
	public Animator anim;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;

        SetIdle();
    }

    void Update()
    {
        //agent.SetDestination(targetTransform.position);
        distanceFromTarget = GetDistanceFromTarget();

        switch (state)
        {
            case EnemyState.Idle:
                IdleUpdate();
                break;
            case EnemyState.Patrol:
                PatrolUpdate();
                break;
            case EnemyState.Chase:
                ChaseUpdate();
                break;
            case EnemyState.Attack:
                AttackUpdate();
                break;
            case EnemyState.Stun:
                StunUpdate();
                break;
            case EnemyState.Dead:
                //DeadUpdate();
                break;
            default:
                break;
        }
    }

    #region Updates
    void IdleUpdate()
    {
        if (timeCounter >= idleTime)
        {
            SetPatrol();
        }
        else timeCounter += Time.deltaTime;
    }
    void PatrolUpdate()
    {
        if (distanceFromTarget < chaseRange)
        {
            SetChase();
            return;
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            pathIndex++;
            if (pathIndex >= points.Length) pathIndex = 0;

            SetPatrol();
        }
    }
    void ChaseUpdate()
    {
        agent.SetDestination(targetTransform.position);

        if (distanceFromTarget > chaseRange)
        {
            SetPatrol();
            return;
        }
        if (distanceFromTarget < attackRange)
        {
            SetAttack();
            return;
        }
    }
    void AttackUpdate()
    {
        agent.SetDestination(targetTransform.position);
        
        if(canAttack)
        {
            agent.Stop();

			anim.SetTrigger("Attack");
            //version 5.6 agent.isStopped = true;
            targetTransform.GetComponent<PlayerBehaviour>().SetDamage(hitDamage);
            idleTime = coolDownAttack;
            SetIdle();
            return;
        }

        if (distanceFromTarget > attackRange)
        {
            SetChase();
            return;
        }
    }
    void StunUpdate()
    {
        if (timeCounter >= stunTime)
        {
            idleTime = 0;
            SetIdle();
        }
        else timeCounter += Time.deltaTime;
    }
    void DeadUpdate() { }
    #endregion
    #region Sets
    void SetIdle()
    {
        timeCounter = 0;
        state = EnemyState.Idle;
    }
    void SetPatrol()
    {
        agent.Resume();
        //agent.isStopped = false; 5.6

        anim.SetBool("Patrol", true);

        agent.SetDestination(points[pathIndex].position);
        state = EnemyState.Patrol;
    }
    void SetChase()
    {
        //Feedback
        anim.SetBool("Patrol", true);
        state = EnemyState.Chase;
    }
    void SetAttack()
    {
        state = EnemyState.Attack;
    }
    void SetStun()
    {
        agent.Stop();
        //FEEDBACK (animaciones, sonido, etc...)
		anim.SetTrigger("Damage");

        state = EnemyState.Stun;
    }
    void SetDead()
    {
        agent.Stop();
        //FEEDBACK (animación, sonido, etc...)
		anim.SetTrigger("Dead");

        state = EnemyState.Dead;

        this.gameObject.SetActive(false);
        //Destroy(this.gameObject);
    }
    #endregion
    #region Public Functions
    public void SetDamage(int hit)
    {
        life -= hit;

        anim.SetTrigger("Damage");

        if(life <= 0)
        {
            SetDead();
            return;
        }
        SetStun();
    }
    #endregion

    float GetDistanceFromTarget()
    {
        return Vector3.Distance(targetTransform.position, transform.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            canAttack = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            canAttack = false;
        }
    }

    void OnDrawGizmos()
    {
        Color newColor = Color.yellow;
        newColor.a = 0.15f;
        Gizmos.color = newColor;
        Gizmos.DrawSphere(transform.position, chaseRange);
        newColor = Color.red;
        newColor.a = 0.15f;
        Gizmos.color = newColor;
        Gizmos.DrawSphere(transform.position, attackRange);
    }
}