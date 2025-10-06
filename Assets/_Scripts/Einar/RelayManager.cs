using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class RelayManager : MonoBehaviour
{
    public enum ConnectionType { Udp, Tcp }

    // Setup relay for hosting
    public async Task<string> SetupRelayAsync(int maxConnections, ConnectionType connectionType)
    {
        await InitializeUnityServices();
        await SignInAnonymously();

        var allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        var relayData = AllocationUtils.ToRelayServerData(allocation, connectionType.ToString().ToLower());
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayData);
        var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        return joinCode; // share this with clients
    }

    // Join relay for clients
    public async Task<string> JoinRelayAsync(string joinCode)
    {
        await InitializeUnityServices();
        await SignInAnonymously();

        var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        var relayData = AllocationUtils.ToRelayServerData(joinAllocation, ConnectionType.Udp.ToString().ToLower());
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayData);
        return joinCode; // optional, for confirmation
    }

    private async Task InitializeUnityServices()
    {
        await UnityServices.InitializeAsync();
    }

    private async Task SignInAnonymously()
    {
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }
}