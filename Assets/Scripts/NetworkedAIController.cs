using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NetworkedAIController : NetworkBehaviour
{
    private NavMeshAgent navMeshAgent;
    
    public List<Transform> waypoints = new List<Transform>();

    private int index = 0;
    
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public override void OnNetworkSpawn()
    {
        // CRITICAL: If this instance is running on a client, disable NavMesh processing
        if (!IsServer)
        {
            navMeshAgent.enabled = false;
            return;
        }
        
        // Exit early if we are not the server/host
        StartCoroutine(SetDest());
    }

    private IEnumerator SetDest()
    {
        yield return new WaitForSeconds(5.0f);
        
        navMeshAgent.SetDestination(waypoints[index].position);

        index++;
        
        if (index == waypoints.Count)
        {
            index = 0;
        }

        StartCoroutine(SetDest());
    }
}