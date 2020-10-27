using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    #region PRIVATE_FIELDS
    Transform playerTrans;
    #endregion

    #region COMPONENTS
    NavMeshAgent agent;
    #endregion

    #region METHODS
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTrans = FindObjectOfType<PlayerController>().transform;
    }

    void Update()
    {
        agent.SetDestination(playerTrans.position);
    }
    #endregion
}
