using System.Collections;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject[] zombiePrefabs;
    public float spawnInterval = 2f;
    private float timer;
    private float elapsed;

    void Update()
    {
        if (!gameObject.activeInHierarchy) return;
        elapsed += Time.deltaTime;
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            Spawn();
            timer = 0;
        }
        // aumenta dificuldade a cada 30s
        if (elapsed >= 30f && spawnInterval > 0.5f)
        {
            spawnInterval -= 0.2f;
            elapsed = 0;
        }
    }

    void Spawn()
    {
        // escolhe canto aleatório
        Vector2 pos = Vector2.zero;
        int side = Random.Range(0,4);
        float x = side<2 ? -10 : 10;
        float y = side%2==0 ? -5 : 5;
        pos = new Vector2(x, y);
        // escolhe level baseado no tempo do jogo
        float t = Time.timeSinceLevelLoad;
        int idx = t < 30 ? 0 : t < 60 ? Random.Range(0,2) : Random.Range(0,3);
        Instantiate(zombiePrefabs[idx], pos, Quaternion.identity);
    }
}
