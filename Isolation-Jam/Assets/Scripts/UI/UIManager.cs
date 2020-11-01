using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Instance { private set; get; }
    #endregion

    public GameObject menu;
    public Timer timer;
    public UIRespawning respawning;
    public UIHealth health;
    public UIGun gun;
    public GameObject interactionText;

    Camera cam;

    void Awake()
    {
        Instance = this;
        cam = Camera.main;
    }

    public void OnPlay()
    {
        menu.SetActive(false);
        timer.StartTimer();
    }

    public void ShowInteractionText(Vector3 objectPosition)
    {
        Vector3 canvasPosition = cam.WorldToScreenPoint(objectPosition);
        interactionText.transform.position = canvasPosition;

        interactionText.SetActive(true);
    }

    public void HideInteractionText()
    {
        interactionText.SetActive(false);
    }
}
