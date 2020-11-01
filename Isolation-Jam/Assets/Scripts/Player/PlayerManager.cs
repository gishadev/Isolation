using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton
    public static PlayerManager Instance { private set; get; }
    #endregion

    [Header("Spawning/Respawning")]
    public PlayerController player;
    public Transform playerSpawnpoint;
    public float respawnTime = 5f;

    [Header("Interactions")]
    public LayerMask interactableMask;

    [Header("Guns")]
    public GunData defaultGun;

    public IInteractable SelectedInteractTarget { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GiveGun(defaultGun);
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
        UIManager.Instance.respawning.TriggerUIRespawning(respawnTime);
        yield return new WaitForSeconds(respawnTime);
        GiveGun(defaultGun);
        player.Health = player.maxHealth;
        player.gameObject.SetActive(true);

        AudioManager.Instance.PlaySFX("Player_Spawn");
    }
    #endregion

    #region Interactions
    void CheckForInteractionTarget()
    {
        // Проверяем, может ли определенная цель для взамодействия быть выбрана.
        Transform temp = GetInteractTargetOnPos(player.transform.position);
        IInteractable interactable = null;
        if (temp != null)
            temp.TryGetComponent(out interactable);

        if (interactable != null && interactable.IsReadyForInteraction())
        {
            SelectedInteractTarget = interactable;
            UIManager.Instance.ShowInteractionText(temp.transform.position);
        }
        else
        {
            SelectedInteractTarget = null;
            UIManager.Instance.HideInteractionText();
        }
    }
    Transform GetInteractTargetOnPos(Vector3 pos)
    {
        Transform result = null;
        Collider[] colls = Physics.OverlapSphere(pos, 0.5f, interactableMask);
        if (colls.Length > 0)
        {
            result = colls[0].transform;
        }

        return result;
    }
    //InteractionTarget GetInteractTargetOnCursor()
    //{
    //    Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hitInfo;
    //    InteractionTarget result = null;

    //    if (Physics.Raycast(r, out hitInfo))
    //    {
    //        Collider[] colls = Physics.OverlapSphere(hitInfo.point, 0.5f, interactableMask);
    //        if (colls.Length > 0)
    //        {
    //            colls[0].TryGetComponent(out result);
    //        }

    //    }

    //    return result;
    //}
    #endregion

    #region Gun Giving
    public void GiveGun(GunData gunData)
    {
        // Убираем старую пушку.
        if (player.currentGun != null)
            Destroy(player.currentGun.gameObject);

        // Спавним новую пушку.
        GameObject gunGO = Instantiate(gunData.prefab, Vector3.zero, Quaternion.identity, player.transform);
        gunGO.transform.localPosition = gunData.offsetPosition;
        gunGO.transform.localRotation = Quaternion.Euler(gunData.offsetRotation);

        Gun newGun = gunGO.GetComponent<Gun>();
        newGun.gunData = gunData;

        // Назначаем новую пушку.
        player.currentGun = newGun;

        // Обновляем UI.
        UIManager.Instance.gun.ResetGun(gunData);

        if (gunData.isEndlessAmmo)
            UIManager.Instance.gun.UpdateAmmoCount(true);
        else
            UIManager.Instance.gun.UpdateAmmoCount(newGun.gunData.ammoCount);
        // Обновляем анимацию.
        player.pAnimations.UpdateUpperState(newGun.gunData.upperAnimationState);

    }
    #endregion
}
