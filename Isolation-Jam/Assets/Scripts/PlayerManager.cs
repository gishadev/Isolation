using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton
    public static PlayerManager Instance { private set; get; }
    #endregion

    [Header("Spawning")]
    public PlayerController player;
    public Transform playerSpawnpoint;
    public float respawnTime = 5f;
    [Space]
    public LayerMask interactableMask;

    public InteractionTarget SelectedInteractTarget { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void LateUpdate()
    {
        CheckForInteractionTarget();
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
        player.Health = player.maxHealth;
        player.gameObject.SetActive(true);
    }
    #endregion

    #region Interactions
    void CheckForInteractionTarget()
    {
        // Проверяем, может ли определенная цель для взамодействия быть выбрана.
        InteractionTarget temp = GetInteractTargetOnCursor();
        if (temp != null
            && Vector3.Distance(temp.transform.position, player.transform.position) < temp.interactableRadius
            && temp.IsReadyForInteraction())
        {
            SelectedInteractTarget = temp;
            UIManager.Instance.ShowInteractionText(temp.transform.position);
        }

        else
        {
            SelectedInteractTarget = null;
            UIManager.Instance.HideInteractionText();
        }
    }

    InteractionTarget GetInteractTargetOnCursor()
    {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        InteractionTarget result = null;

        if (Physics.Raycast(r, out hitInfo))
        {
            Collider[] colls = Physics.OverlapSphere(hitInfo.point, 0.5f, interactableMask);
            if (colls.Length > 0)
            {
                colls[0].TryGetComponent(out result);
            }
                
        }

        return result;
    }
    #endregion
}
