using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { private set; get; }
    #endregion

    public bool IsPlaying { private set; get; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        AudioManager.Instance.PlaySFX("Player_Spawn");
    }

    void Update()
    {
        if (!IsPlaying)
            if (Input.anyKeyDown)
                StartGame();
    }

    public void StartGame()
    {
        IsPlaying = true;
        UIManager.Instance.OnPlay();
        EnemiesManager.Instance.OnPlay();
    }

    public void Lose()
    {
        IsPlaying = false;
        SceneManager.LoadScene(0);
    }
}
