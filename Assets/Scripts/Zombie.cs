using UnityEngine;

public class Zombie : MonoBehaviour
{
    public int level;
    public float speedFactor;
    private Transform player;
    private Animator anim;
    private int hitsTaken = 0;

    void Awake()
    {
        anim = GetComponent<Animator>();

        PlayerController pc = Object.FindFirstObjectByType<PlayerController>();
        if (pc != null)
        {
            player = pc.transform;
        }
        else
        {
            Debug.LogError("PlayerController não encontrado na cena! Verifique se o objeto Player está ativo e possui o script PlayerController.");
        }
    }

    void Update()
    {
        if (player == null) return;

        Vector3 dir = (player.position - transform.position).normalized;
        transform.Translate(dir * speedFactor * Time.deltaTime);

        string state = dir.x >= 0
            ? $"ZombieLevel{level}Right"
            : $"ZombieLevel{level}Left";
        anim.Play(state);
    }

    public void TakeHit()
    {
        hitsTaken++;
        if (hitsTaken >= level)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            col.GetComponent<PlayerController>()?.TakeDamage();
    }
}
