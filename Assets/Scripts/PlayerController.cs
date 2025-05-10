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

    // Eixos
    private float h;
    private float v;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Init(GameManager gm)
    {
        manager = gm;
        currentLives = 3;
        // Volta para o centro ou posição inicial
        transform.position = Vector3.zero;

        // Reset de parâmetros de animação
        anim.SetFloat("SpeedX", 0f);
        anim.SetBool("IsShooting", false);
        anim.ResetTrigger("TakeDamage");
        anim.SetBool("IsDead", false);
    }

    public void ResetPlayer()
    {
        // Mesmo reset de animações
        anim.SetFloat("SpeedX", 0f);
        anim.SetBool("IsShooting", false);
        anim.ResetTrigger("TakeDamage");
        anim.SetBool("IsDead", false);
    }

    void Update()
    {
        // Captura os inputs
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        // Disparo
        if (Input.GetKeyDown(KeyCode.Space) && canShoot)
            StartCoroutine(Shoot());
    }

    void FixedUpdate()
    {
        // Move o player
        Vector3 dir = new Vector3(h, v, 0).normalized;
        transform.Translate(dir * moveSpeed * Time.fixedDeltaTime);

        // Aplica clamp para limitar posição dentro dos bounds
        Vector3 p = transform.position;
        p.x = Mathf.Clamp(p.x, minX, maxX);
        p.y = Mathf.Clamp(p.y, minY, maxY);
        transform.position = p;

        // Atualiza parâmetro de animação
        anim.SetFloat("SpeedX", h);
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        anim.Play("Shoot");

        // Instancia a bala e define direção
        GameObject b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bullet = b.GetComponent<Bullet>();
        if (bullet != null)
            bullet.Init(this);

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

    public Vector2 FacingDirection()
    {
        if (h > 0) return Vector2.right;
        if (h < 0) return Vector2.left;
        // Mantém direção anterior ou padrão para a direita
        return Vector2.right;
    }

    public void Die()
    {
        anim.SetBool("IsDead", true);
    }
}
