using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { private set; get; }
    #endregion

    public PlayerController player;
    public Transform playerSpawnpoint;

    void Awake()
    {
        Instance = this;
    }

    public void Lose()
    {
        SceneManager.LoadScene(0);
    }
}
