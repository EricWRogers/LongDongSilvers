using UnityEngine;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;

public class NetCodeSetup : MonoBehaviour
{
    [Header("Debug Join Code")]
    public string joinCode; 

    private UnityTransport transport;

    async void Start()
    {
        transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

        //initUnity services (required before using authentication/relay)
        await InitServices();
    }

    async Task InitServices()
    {
        //init unity services (must be called before relay/auth usage)
        await UnityServices.InitializeAsync();

        //if not already signed in, sign in anon
        //This gives the player a temporary Unity ID for Relay
        if (!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

        Debug.Log("Services ready");
    }

    public async void StartHost()
    {
        //Create a Relay allocation for up to 3 clients (4 including host)
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

        //Generate a join code
        joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        Debug.Log("JOIN CODE: " + joinCode);

        //Configure transport with Relay server data 
        transport.SetHostRelayData(
            allocation.RelayServer.IpV4,
            (ushort)allocation.RelayServer.Port,
            allocation.AllocationIdBytes,
            allocation.Key,
            allocation.ConnectionData
        );

        NetworkManager.Singleton.StartHost();

        Debug.Log("Host started");
    }

    public async void StartClient(string code)
    {
        //Join an existing Relay allocation using the join code
        JoinAllocation allocation =
            await RelayService.Instance.JoinAllocationAsync(code);

        transport.SetClientRelayData(
            allocation.RelayServer.IpV4,
            (ushort)allocation.RelayServer.Port,
            allocation.AllocationIdBytes,
            allocation.Key,
            allocation.ConnectionData,
            allocation.HostConnectionData
        );

        NetworkManager.Singleton.StartClient();

        Debug.Log("Client started");
    }
}