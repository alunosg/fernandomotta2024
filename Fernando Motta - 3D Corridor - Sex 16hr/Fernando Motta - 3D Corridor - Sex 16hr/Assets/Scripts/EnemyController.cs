using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum EnemyStates { Idle, Chase, Attack, Hit, Death }
    public EnemyStates state = EnemyStates.Idle;
    public UnityEngine.AI.NavMeshAgent nav;
    public float chaseDistance = 50;
    public float attackDistance = 2;

    public int hp = 0;

    public float stunDuration = 1;
    public float deathDuration =5;
    public float attackDuration =5;

    public GameObject hitParticle;

    private bool locked = false;
    private bool dead = false;

    private PlayerController player;
    private float distance = 1000;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        switch (state)
        {
            case EnemyStates.Idle:
                IdleUpdate();
                break;
            case EnemyStates.Chase:
                ChaseUpdate();
                break;
            case EnemyStates.Attack:
                AttackUpdate();
                break;
            case EnemyStates.Hit:
                HitUpdate();
                break;
            case EnemyStates.Death:
                DeathUpdate();
                break;
        }
    }

    void ChaseUpdate()
    {
            if (distance < attackDistance)
            {
                state = EnemyStates.Attack;
            }
            else if( distance >= chaseDistance)
            {
                state = EnemyStates.Idle;
            }
            else
            {
                nav.isStopped = false;
                nav.SetDestination(player.transform.position);
            }        
    }

    void IdleUpdate()
    {
        if (distance < chaseDistance)
        {
            if(distance < attackDistance)
            {
                state = EnemyStates.Attack;
            }
            else
            {
                state = EnemyStates.Chase;
            }
        }
        else
        {
            nav.isStopped = true;
        }
    }
    void AttackUpdate()
    {
        if (distance >= attackDistance)
        {
            if (distance >= chaseDistance)
            {
                state = EnemyStates.Idle;
            }
            else
            {
                state = EnemyStates.Chase;
            }
        }
        else 
        {
            EnterAttack();
        }
    }

    void HitUpdate()
    {

    } 

    void DeathUpdate()
    {
        //EnemySpawner.Invoke("KillEnemy");
    }

    public void GetHit(int damage)
    {
        if(dead)
        {
            locked = true;
            hp -= damage;
            Instantiate(hitParticle, transform);
            nav.isStopped = true;
            CancelInvoke("DealDamage");
            CancelInvoke("Unlock");

            if (hp <= 0)
            {
                dead = true;
                state = EnemyStates.Death;
                Invoke("AutoDestroy", deathDuration);
            }
            else
            {
                state = EnemyStates.Idle;
                Invoke("Unlock", stunDuration);
            }
        }
    }

    void Unlock()
    {
        locked = false;
    }

    void AutoDestroy()
    {
        Destroy(gameObject);
    }

    void EnterAttack()
    {
        state = EnemyStates.Attack;
        locked = true;
        nav.isStopped = true;
        CancelInvoke("DealDamage");
        CancelInvoke("Unlock");
        Invoke("DealDamage", 0.5f);
        Invoke("Unlock", attackDuration);
    }

    void DealDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackDistance + 0.5f);
        foreach (var target in hitColliders)
        {
            if(target.CompareTag("Player"))
            {
                player.GetHit(1, Vector3.MoveTowards(target.transform.position, transform.position, 0.5f));
            }
        }
    }
}
