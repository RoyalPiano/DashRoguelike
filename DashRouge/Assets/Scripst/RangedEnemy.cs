using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : MonoBehaviour
{
    public PlayerBehaviour player;
    NavMeshAgent agent;
    public List<DropItem> dropList;
    public GameObject heal;
    public GameObject projectile;
    public GameObject coin;

    public double health = 2;
    private double aggroRange = 20;
    private double attackRange = 10;
    private double attackRate = 0.5;
    private double attackCountdown = 0;
    private Animator animator;
    private bool ???????????????? = false;
    void Start()
    {
        dropList.Add(new DropItem(heal, 70));
        dropList.Add(new DropItem(coin, 30));

        if (GameObject.FindGameObjectsWithTag("player").Length != 0)
            player = GameObject.FindGameObjectWithTag("player").GetComponent<PlayerBehaviour>();

        agent = GetComponent<NavMeshAgent>();
        InvokeRepeating("Move", 0, .5f);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        double distance;
        //if (health <= 0)
        //{
        //    transform.tag = "dead";
        //    Destroy(gameObject);
        //}

        if (GameObject.FindGameObjectsWithTag("player").Length != 0)
            distance = Vector3.Distance(transform.position, player.transform.position);
        else
            distance = attackRange + 1;
        if (attackCountdown <= 0 && distance <= attackRange)
        {
            Attack();
            attackCountdown = 1 / attackRate;
        }
        attackCountdown -= Time.deltaTime;
        if (distance < attackRange)
        {
            Stop();
            ???????????????? = true;
        }
        else ???????????????? = false;
    }

    public void takeDamage(double damage)
    {
        health -= damage;
        if (health <= 0)
        {
            transform.tag = "dead";
            Destroy(gameObject);
            CheckDrop();
        }
    }

    void Attack()
    {
        animator.Play("Crashing");
        Instantiate(projectile, transform.position, Quaternion.identity);
    }


    void Move()
    {
        double distanceToPlayer;
        if (GameObject.FindGameObjectsWithTag("player").Length != 0)
        {
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= aggroRange)
            {
                agent.isStopped = false;
                agent.SetDestination(player.transform.position);
                if (???????????????? == false)
                    animator.Play("Walk");
            }
        }
        else
        {
            Stop();
            animator.Play("Idle");
        }
    }

    void Stop()
    {
        agent.isStopped = true;
    }

    public void CheckDrop()
    {
        if (dropList.Count > 0)
        {
            int rnd = (int)Random.Range(0, 100);

            foreach (var item in dropList)
            {
                if (item.chance < rnd)
                {
                    item.CreateDropItem(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z));
                    return;
                }
            }
        }
    }
}
