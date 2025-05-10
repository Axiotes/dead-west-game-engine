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
        player = FindObjectOfType<PlayerController>().transform;
    }

    void Update()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        transform.Translate(dir * speedFactor * Time.deltaTime);
        // animação de acordo com x
        anim.Play(dir.x >= 0 ? $"ZombieLevel{level}Right" : $"ZombieLevel{level}Left");
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
            col.GetComponent<PlayerController>().TakeDamage();
    }
}
