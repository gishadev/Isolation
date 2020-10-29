using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton
    public static PlayerManager Instance { private set; get; }
    #endregion

    public PlayerController player;
    public Transform playerSpawnpoint;
    public float respawnTime = 5f;

    void Awake()
    {
        Instance = this;
    }

    #region Respawning
    public void TriggerRespawning()
    {
        StartCoroutine(Respawning());
    }

    IEnumerator Respawning()
    {
        player.transform.position = playerSpawnpoint.position;
        yield return new WaitForSeconds(respawnTime);
        player.gameObject.SetActive(true);
    }
    #endregion
}
