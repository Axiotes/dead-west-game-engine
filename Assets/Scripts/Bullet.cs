using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 dir;
    private PlayerController owner;

    public void Init(PlayerController pc)
    {
        owner = pc;
        dir = pc.FacingDirection();
    }

    void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Zombie"))
        {
            col.GetComponent<Zombie>().TakeHit();
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
