using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Text scoreText;
    public TextMeshProUGUI highScoreText;
    public GameObject Play_BTN;
    public GameObject gameOver;

    private int score;
    private int highScore;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.gameObject.SetActive(false); 
        Pause();
    }

    public void Play()
    {
        score = 0;
        scoreText.text = score.ToString();

        Play_BTN.SetActive(false);
        gameOver.SetActive(false);
        highScoreText.gameObject.SetActive(false);

        Time.timeScale = 1f;
        player.enabled = true;

        Boms[] boms = FindObjectsOfType<Boms>();
        for (int i = 0; i < boms.Length; i++)
        {
            Destroy(boms[i].gameObject);
        }

        player.transform.position = Vector3.zero;
        typeof(Player).GetField("direction", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(player, Vector3.zero);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        player.enabled = false;
    }

    public void GameOver()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        highScoreText.text = "High Score: " + highScore.ToString();
        gameOver.SetActive(true);
        Play_BTN.SetActive(true);
        highScoreText.gameObject.SetActive(true);
        Pause();
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
    }
}
