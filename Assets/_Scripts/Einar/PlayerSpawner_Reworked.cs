using System.Collections;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerSpawner_Reworked : MonoBehaviour
{
    [SerializeField] private GameObject dinoPrefab;
    [SerializeField] private GameObject cowboyPrefab;

    private NetworkObject dinoNO;

    private void Awake()
    {
        if (NetworkManager.Singleton == null) return;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void Start()
    {
        if (!NetworkManager.Singleton.IsHost)
            NetworkManager.Singleton.StartHost();

        StartCoroutine(ForceSpawnHostPlayer());
    }

    private IEnumerator ForceSpawnHostPlayer()
    {
        yield return null; // let host fully init
        ulong localId = NetworkManager.Singleton.LocalClientId;
        Debug.LogError($"🚀 FORCE-SPAWNING host client {localId}");
        OnClientConnected(localId);
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.LogError($"🔥 CALLBACK FIRED for client {clientId}");
        if (!NetworkManager.Singleton.IsServer) return;

        bool isDino = NetworkManager.Singleton.ConnectedClients.Count == 1;

        NetworkObject no = Instantiate(isDino ? dinoPrefab : cowboyPrefab)
                          .GetComponent<NetworkObject>();

        no.SpawnAsPlayerObject(clientId);

        if (isDino)
            dinoNO = no;
        else
            StartCoroutine(MountCowboy(no));
    }

    private IEnumerator MountCowboy(NetworkObject cowboy)
    {
        yield return null;
        cowboy.TrySetParent(dinoNO, false);

        // disable POSITION sync only – rotation still replicates
        NetworkTransform nt = cowboy.GetComponent<NetworkTransform>();
        nt.SyncPositionX = false;
        nt.SyncPositionY = false;
        nt.SyncPositionZ = false;
        nt.SyncRotAngleX = true;   // keep these
        nt.SyncRotAngleY = true;
        nt.SyncRotAngleZ = true;
    }
}