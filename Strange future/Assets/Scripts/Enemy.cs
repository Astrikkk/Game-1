using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float distanceToReact = 5f;
    [SerializeField] private List<Transform> patrolPoints;
    public int HP = 150;
    public int moveSpeed = 5;
    private int currentPatrolPointIndex;
    private bool isPatrollingForward = true;
    private bool isStunned = false;

    private void FixedUpdate()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (!isStunned)
        {
            if (player != null)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
                if (distanceToPlayer <= distanceToReact)
                {
                    Vector2 direction = (player.transform.position - transform.position).normalized;
                    Move(direction);
                }
                else
                {
                    Patrol();
                }
            }
        }
    }
    private void Move(Vector2 direction)
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }
    private void Patrol()
    {
        if (patrolPoints.Count == 0) return;

        Vector2 currentPatrolPoint = patrolPoints[currentPatrolPointIndex].position;
        transform.position = Vector2.MoveTowards(transform.position, currentPatrolPoint, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, currentPatrolPoint) < 0.1f)
        {
            if (isPatrollingForward)
            {
                currentPatrolPointIndex++;
                if (currentPatrolPointIndex >= patrolPoints.Count)
                {
                    currentPatrolPointIndex = Mathf.Max(0, patrolPoints.Count - 2);
                    isPatrollingForward = false;
                }
            }
            else
            {
                currentPatrolPointIndex--;
                if (currentPatrolPointIndex < 0)
                {
                    currentPatrolPointIndex = Mathf.Min(1, patrolPoints.Count - 1);
                    isPatrollingForward = true;
                }
            }
        }
    }

    private IEnumerator Stun()
    {
        isStunned = true;
        yield return new WaitForSeconds(1f);
        isStunned = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Stun());
        }
        if (other.gameObject.CompareTag("Bullet"))
        {
            HP -= other.gameObject.GetComponent<Bullet>().damage;
            if (HP <= 0) Destroy(gameObject);
        }
    }

}
