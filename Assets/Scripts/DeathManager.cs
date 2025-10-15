using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    [Header("UI")]
    public Animator deathPanelAnimator;
    public float restartDelay = 1.5f;

    bool isDying;

    public void PlayerDied()
    {
        if (isDying) return;
        isDying = true;

        if (deathPanelAnimator)
            deathPanelAnimator.SetTrigger("Death");

        Time.timeScale = 0.6f;
        Invoke(nameof(Reload), restartDelay);
    }

    void Reload()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
