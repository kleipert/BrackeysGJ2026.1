using UnityEngine;
using System.Collections;

public class LungeBoss : MonoBehaviour
{
    [Header("Movement")]
    public float leftX;
    public float rightX;
    public float moveSpeed = 3f;
    private bool movingRight = true;

    [Header("Hit Settings")]
    public int hitsNeeded = 3;
    private int currentHits = 0;

    [Header("Rounds")]
    public int totalRounds = 3;
    private int currentRound = 1;

    [Header("Fly Settings")]
    public float flyHeight = 3f;
    public float flySpeed = 5f;
    public float stayInAirTime = 3f;

    [Header("Tornado Settings")]
    public GameObject tornadoPrefab;
    public float tornadoOffsetX = 1.5f;
    public float tornadoOffsetY = 0f;

    private Rigidbody2D rb;
    private Animator anim;

    private float groundY;
    private bool isFlying = false;
    private bool isInvincible = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        groundY = transform.position.y;
    }

    void Update()
    {
        if (!isFlying)
            Patrol();
    }

    void Patrol()
    {
        if (movingRight)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            if (transform.position.x >= rightX)
                movingRight = false;
        }
        else
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            if (transform.position.x <= leftX)
                movingRight = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Player") && !isInvincible)
        {
            currentHits++;

            if (currentHits >= hitsNeeded)
            {
                StartCoroutine(FlyRoutine());
            }
        }
    }

    IEnumerator FlyRoutine()
    {
        isFlying = true;
        isInvincible = true;

        rb.gravityScale = 0;

        Vector3 upPos = new Vector3(transform.position.x, groundY + flyHeight, 0);
        while (Vector3.Distance(transform.position, upPos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, upPos, flySpeed * Time.deltaTime);
            yield return null;
        }

        SpawnTornadoLeft();
        yield return new WaitForSeconds(0.3f);
        SpawnTornadoRight();

        yield return new WaitForSeconds(stayInAirTime);

        Vector3 downPos = new Vector3(transform.position.x, groundY, 0);
        while (Vector3.Distance(transform.position, downPos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, downPos, flySpeed * Time.deltaTime);
            yield return null;
        }

        rb.gravityScale = 3;

        currentHits = 0;
        isFlying = false;
        isInvincible = false;

        currentRound++;

        if (currentRound > totalRounds)
        {
            StartCoroutine(FinalPhase());
            yield break;
        }
    }

    void SpawnTornadoLeft()
    {
        Vector3 spawnPos = new Vector3(
            transform.position.x - tornadoOffsetX,
            transform.position.y + tornadoOffsetY,
            0
        );

        GameObject t = Instantiate(tornadoPrefab, spawnPos, Quaternion.identity);
        t.GetComponent<TornadoMovement>().direction = -1;
    }

    void SpawnTornadoRight()
    {
        Vector3 spawnPos = new Vector3(
            transform.position.x + tornadoOffsetX,
            transform.position.y + tornadoOffsetY,
            0
        );

        GameObject t = Instantiate(tornadoPrefab, spawnPos, Quaternion.identity);
        t.GetComponent<TornadoMovement>().direction = 1;
    }

    IEnumerator FinalPhase()
    {
        Debug.Log("Finale Phase startet!");

        rb.gravityScale = 0;
        anim.SetBool("Splitting", true);

        yield return new WaitForSeconds(3.5f);

        Destroy(gameObject);
    }
}
