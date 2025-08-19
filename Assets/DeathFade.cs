using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathFade : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private Image image;

    private float targetAlpha = 1f;
    private float currentAlpha = 0f;
    private bool isFading = false;

    private void Update()
    {
        if (!isFading) return;

        currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, Time.deltaTime * speed);

        image.color = new Color(0f, 0f, 0f, currentAlpha);

 
        if (currentAlpha >= 0.99f)
        {
            currentAlpha = 1f;
            image.color = new Color(0f, 0f, 0f, 1f);
            isFading = false;

            SceneManager.LoadScene("MainMenu");
        }
    }

    public void StartFade()
    {
        isFading = true;
        currentAlpha = 0f; 
    }
}
