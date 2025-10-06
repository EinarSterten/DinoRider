using System.Collections;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerSpawner_Reworked : MonoBehaviour
{
    [SerializeField] private GameObject dinoPrefab;   // assign in inspector
    [SerializeField] private GameObject cowboyPrefab; // assign in inspector

    private NetworkObject dinoNO;

    public void StartHostAndSpawn()
    {
        // Remove existing event handlers to avoid duplicates
        NetworkManager.Singleton.OnServerStarted -= SpawnHostPlayer;
        NetworkManager.Singleton.OnServerStarted += SpawnHostPlayer;

        NetworkManager.Singleton.StartHost();
    }

    public void SpawnHostPlayer()
    {
        NetworkManager.Singleton.OnServerStarted -= SpawnHostPlayer;

        ulong hostId = NetworkManager.Singleton.LocalClientId;
        GameObject go = Instantiate(dinoPrefab);
        NetworkObject no = go.GetComponent<NetworkObject>();
        no.SpawnAsPlayerObject(hostId);
        dinoNO = no;

        // Spawn client players if connected
        StartCoroutine(SpawnClientPlayers());
    }

    private IEnumerator SpawnClientPlayers()
    {
        // Wait until at least 2 clients connected (host + one client)
        while (NetworkManager.Singleton.ConnectedClients.Count < 2)
            yield return null;

        // Get the second client (assuming index 1)
        var client = NetworkManager.Singleton.ConnectedClientsList[1];
        ulong clientId = client.ClientId;

        GameObject go2 = Instantiate(cowboyPrefab);
        NetworkObject no2 = go2.GetComponent<NetworkObject>();
        no2.SpawnAsPlayerObject(clientId);
        StartCoroutine(MountCowboy(no2));
    }

    private IEnumerator MountCowboy(NetworkObject cowboy)
    {
        yield return null; // wait for next frame

        if (dinoNO != null)
        {
            cowboy.TrySetParent(dinoNO, false);
        }

        var nt = cowboy.GetComponent<NetworkTransform>();
        if (nt != null)
        {
            nt.SyncPositionX = false;
            nt.SyncPositionY = false;
            nt.SyncPositionZ = false;
            nt.SyncRotAngleX = true;
            nt.SyncRotAngleY = true;
            nt.SyncRotAngleZ = true;
        }

        // Spawn enemies after mounting
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SpawnEnemies();
        }
        else
        {
            Debug.LogWarning("GameManager instance not found");
        }
    }
}