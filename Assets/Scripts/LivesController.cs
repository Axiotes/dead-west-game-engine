using UnityEngine;

public class LivesController : MonoBehaviour
{
    public GameObject lifeIconPrefab;

    // private int maxLives = 3;

    public void ResetLives()
    {
        // garante 3 ícones
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(true);
    }

    public void LoseLife()
    {
        // desativa último ícone ativo
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                transform.GetChild(i).gameObject.SetActive(false);
                break;
            }
        }
    }
}
