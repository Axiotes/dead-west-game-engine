using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimentação")]
    public float moveSpeed = 5f;

    [Header("Limites do Cenário")]
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -4f;
    public float maxY = 4f;

    [Header("Tiro")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;

    private Animator anim;
    private bool canShoot = true;
    private GameManager manager;
    private int currentLives = 3;

    private float h;
    private float v;
    private Vector2 lastDirection = Vector2.right;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Init(GameManager gm)
    {
        manager      = gm;
        currentLives = 3;
        transform.position = Vector3.zero;

        anim.SetFloat("SpeedX", 0f);
        anim.SetBool("IsShooting", false);
        anim.ResetTrigger("TakeDamage");
        anim.SetBool("IsDead", false);
        anim.Play("Idle");
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
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        Vector2 inputDir = new Vector2(h, v).normalized;
        if (inputDir != Vector2.zero)
            lastDirection = inputDir;

        if (Input.GetKeyDown(KeyCode.Space) && canShoot)
            StartCoroutine(Shoot());
    }

    void FixedUpdate()
    {
        Vector3 dir = new Vector3(h, v, 0f).normalized;
        transform.Translate(dir * moveSpeed * Time.fixedDeltaTime);

        Vector3 p = transform.position;
        p.x = Mathf.Clamp(p.x, minX, maxX);
        p.y = Mathf.Clamp(p.y, minY, maxY);
        transform.position = p;

        anim.SetFloat("SpeedX", h);
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        anim.Play("Shoot");

        GameObject b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bullet = b.GetComponent<Bullet>();
        if (bullet != null)
            bullet.Init(lastDirection);

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
            manager.PlayerDead();
    }

    public void Die()
    {
        anim.SetBool("IsDead", true);
    }
}
