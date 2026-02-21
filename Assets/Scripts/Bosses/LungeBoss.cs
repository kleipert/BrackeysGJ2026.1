using UnityEngine;

public class LungeBoss : MonoBehaviour
{
    private enum BossState { Move, Wait, MoveToMiddle, WaitInMiddle, FlyUp }
    private BossState state = BossState.Move;

    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float rightLimit = 6f;
    [SerializeField] private float leftLimit = -6f;
    [SerializeField] private float middle = 0f;
    [SerializeField] private float waitTime = 1.5f;
    [SerializeField] private float flySpeed = 3f;
    [SerializeField] private float flyLimitY = 5f;

    [SerializeField] private GameObject tornadoPrefab;
    [SerializeField] private Transform tornadoSpawnPoint;

    private Rigidbody2D rb;
    private float timer = 0f;

    private int sideSwitchCount = 0;
    private bool tornadoSpawned = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        switch (state)
        {
            case BossState.Move:
                MoveBetweenLimits();
                break;

            case BossState.Wait:
                Wait();
                break;

            case BossState.MoveToMiddle:
                MoveToMiddle();
                break;

            case BossState.WaitInMiddle:
                WaitInMiddle();
                break;

            case BossState.FlyUp:
                FlyUp();
                break;
        }
    }

    void MoveBetweenLimits()
    {
        rb.linearVelocity = new Vector2(speed, 0);

        if (transform.position.x >= rightLimit && speed > 0)
        {
            sideSwitchCount++;
            state = BossState.Wait;
        }

        if (transform.position.x <= leftLimit && speed < 0)
        {
            sideSwitchCount++;
            state = BossState.Wait;
        }
    }

    void Wait()
    {
        rb.linearVelocity = Vector2.zero;
        timer += Time.deltaTime;

        if (timer >= waitTime)
        {
            timer = 0f;

            if (sideSwitchCount >= 4)
            {
                state = BossState.MoveToMiddle;
                return;
            }

            speed = -speed;
            state = BossState.Move;
        }
    }

    void MoveToMiddle()
    {
        float direction = Mathf.Sign(middle - transform.position.x);
        rb.linearVelocity = new Vector2(direction * Mathf.Abs(speed), 0);

        if (Mathf.Abs(transform.position.x - middle) < 0.1f)
        {
            rb.linearVelocity = Vector2.zero;
            state = BossState.WaitInMiddle;
        }
    }

    void WaitInMiddle()
    {
        rb.linearVelocity = Vector2.zero;
        timer += Time.deltaTime;

        if (timer >= waitTime)
        {
            timer = 0f;
            state = BossState.FlyUp;
        }
    }

    void FlyUp()
    {
        if (transform.position.y < flyLimitY)
        {
            rb.linearVelocity = new Vector2(0, flySpeed);
            return;
        }

        rb.linearVelocity = Vector2.zero;

        if (!tornadoSpawned)
        {
            SpawnTornados();
            tornadoSpawned = true;
        }
    }

    void SpawnTornados()
    {
        GameObject t1 = Instantiate(tornadoPrefab, tornadoSpawnPoint.position, Quaternion.identity);
        t1.GetComponent<TornadoMovement>().direction = 1;

        GameObject t2 = Instantiate(tornadoPrefab, tornadoSpawnPoint.position, Quaternion.identity);
        t2.GetComponent<TornadoMovement>().direction = -1;
    }
}
