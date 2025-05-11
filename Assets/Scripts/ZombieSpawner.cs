using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Prefabs de Zumbi (0: lvl1, 1: lvl2, 2: lvl3)")]
    public GameObject[] zombiePrefabs;

    [Header("Configuração de Spawn")]
    public float initialSpawnInterval = 2f;
    public float minSpawnInterval = 0.5f;

    private float spawnInterval;
    private float timer;
    private float elapsed;

    void Awake()
    {
        ResetSpawner();
    }

    /// <summary>
    /// Restaura spawnInterval, timer e elapsed aos valores iniciais.
    /// Deve ser chamado sempre que o jogo reiniciar.
    /// </summary>
    public void ResetSpawner()
    {
        spawnInterval = initialSpawnInterval;
        timer         = 0f;
        elapsed       = 0f;
    }

    void Update()
    {
        if (!gameObject.activeInHierarchy) 
            return;

        elapsed += Time.deltaTime;
        timer   += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            Spawn();
            timer = 0f;
        }

        if (elapsed >= 30f && spawnInterval > minSpawnInterval)
        {
            spawnInterval = Mathf.Max(minSpawnInterval, spawnInterval - 0.2f);
            elapsed       = 0f;
        }
    }

    void Spawn()
    {
        int side = Random.Range(0, 4);
        float x = side < 2 ? -10f : 10f;
        float y = (side % 2) == 0 ? -5f : 5f;
        Vector2 pos = new Vector2(x, y);

        float t = Time.timeSinceLevelLoad;
        int idx = t < 30f 
                  ? 0 
                  : (t < 60f 
                     ? Random.Range(0, 2) 
                     : Random.Range(0, 3));

        Instantiate(zombiePrefabs[idx], pos, Quaternion.identity);
    }
}
