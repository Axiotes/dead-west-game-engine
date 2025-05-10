using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;

    private Animator anim;
    private bool canShoot = true;
    private GameManager manager;
    private int currentLives = 3;
    private float h;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Init(GameManager gm)
    {
        manager = gm;
        currentLives = 3;
        transform.position = Vector3.zero;
        anim.SetFloat("SpeedX", 0f);
        anim.SetBool("IsShooting", false);
        anim.ResetTrigger("TakeDamage");
        anim.SetBool("IsDead", false);
    }

    public void ResetPlayer()
    {
        anim.SetFloat("SpeedX", 0f);
        anim.SetBool("IsShooting", false);
        anim.ResetTrigger("TakeDamage");
        anim.SetBool("IsDead", false);
    }

    void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.Space) && canShoot)
        {
            StartCoroutine(Shoot());
        }
    }

    void Move()
    {
        h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(h, v, 0).normalized;
        transform.Translate(dir * moveSpeed * Time.deltaTime);
        anim.SetFloat("SpeedX", h);
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        anim.Play("Shoot");
        
        // Instancia a bala e chama o método Init para definir a direção
        GameObject b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bullet = b.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.Init(this); // Passa o player para definir a direção
        }

        yield return new WaitForSeconds(fireRate); 
        anim.Play("Idle");
        canShoot = true;
    }

    public void TakeDamage()
    {
        currentLives--;
        anim.SetTrigger("TakeDamage");
        anim.Play("Damage");
        manager.livesPanel.GetComponent<LivesController>().LoseLife();

        if (currentLives <= 0)
        {
            manager.PlayerDead();
        }
    }

    public Vector2 FacingDirection()
    {
        if (h > 0) return Vector2.right;
        if (h < 0) return Vector2.left;
        return Vector2.right;
    }

    public void Die()
    {
        anim.SetBool("IsDead", true);
    }
}
