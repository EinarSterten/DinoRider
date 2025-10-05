using System.Collections;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerSpawner_Reworked : MonoBehaviour
{
    [SerializeField] private GameObject dinoPrefab;   // drag in inspector
    [SerializeField] private GameObject cowboyPrefab; // drag in inspector

    private NetworkObject dinoNO;

    public void StartHostAndSpawn()
    {
        if (NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsServer)
        {
            Debug.LogWarning("Already running as host or client");
            return;
        }
        NetworkManager.Singleton.OnServerStarted += SpawnHostPlayer;
        NetworkManager.Singleton.StartHost();
    }

    private void SpawnHostPlayer()
    {
        NetworkManager.Singleton.OnServerStarted -= SpawnHostPlayer;

        // host player
        ulong hostId = NetworkManager.Singleton.LocalClientId;
        GameObject go = Instantiate(dinoPrefab);
        NetworkObject no = go.GetComponent<NetworkObject>();
        no.SpawnAsPlayerObject(hostId);
        dinoNO = no;

        // Start coroutine to spawn client players if needed
        StartCoroutine(SpawnClientPlayers());
    }

    private IEnumerator SpawnClientPlayers()
    {
        while (NetworkManager.Singleton.ConnectedClients.Count < 2)
            yield return null;

        ulong clientId = NetworkManager.Singleton.ConnectedClientsList[1].ClientId;
        GameObject go2 = Instantiate(cowboyPrefab);
        NetworkObject no2 = go2.GetComponent<NetworkObject>();
        no2.SpawnAsPlayerObject(clientId);
        StartCoroutine(MountCowboy(no2));
    }

    private IEnumerator MountCowboy(NetworkObject cowboy)
    {
        yield return null;
        cowboy.TrySetParent(dinoNO, false);
        NetworkTransform nt = cowboy.GetComponent<NetworkTransform>();
        nt.SyncPositionX = false;
        nt.SyncPositionY = false;
        nt.SyncPositionZ = false;
        nt.SyncRotAngleX = true;
        nt.SyncRotAngleY = true;
        nt.SyncRotAngleZ = true;

        //Spawn enemies
        GameManager.Instance.SpawnEnemies();
    }
}