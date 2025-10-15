using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // only needed if using TextMeshPro UI

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager instance = null;

    // You can access this from other scripts via: GameManager.Instance
    public static GameManager Instance
    {
        get { return instance; }
    }

    [Header("Score Settings")]
    public int score = 0;
    public int highScore = 0;

    [Header("UI Settings (Optional)")]
    public TextMeshProUGUI coinText; // assign in Inspector if you want a UI coin counter

    void Awake()
    {
        // Ensure only one GameManager exists
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        RefreshUI();
    }

    /// <summary>
    /// Increase score (used for coins, enemies, etc.)
    /// </summary>
    public void IncreaseScore(int amount)
    {
        score += amount;

        // Check for new high score
        if (score > highScore)
        {
            highScore = score;
            Debug.Log("New High Score: " + highScore);
        }

        Debug.Log("New Score: " + score);
        RefreshUI();
    }

    /// <summary>
    /// Update the coin/score UI text if assigned
    /// </summary>
    private void RefreshUI()
    {
        if (coinText != null)
        {
            coinText.text = "Coins: " + score.ToString();
        }
    }

    /// <summary>
    /// Optional method to reset score (e.g. on death or restart)
    /// </summary>
    public void ResetScore()
    {
        score = 0;
        RefreshUI();
    }
}
