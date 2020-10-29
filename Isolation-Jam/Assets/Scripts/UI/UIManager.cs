using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Instance { private set; get; }
    #endregion

    public UIHealth health;

    void Awake()
    {
        Instance = this;
    }
}
