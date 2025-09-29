using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;

    private NetworkObject player1NetworkObject; // Store Player 1's NetworkObject

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnDestroy()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
    }

    private void OnClientConnected(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer) return;
        // Decide which prefab to spawn
        GameObject prefabToSpawn = DecidePrefab(clientId);
        var networkObject = Instantiate(prefabToSpawn);
        var netObj = networkObject.GetComponent<NetworkObject>();

        // Spawn as player object
        netObj.SpawnAsPlayerObject(clientId);

        if (prefabToSpawn == playerPrefab1)
        {
            // Store reference to Player 1's NetworkObject
            player1NetworkObject = netObj;
        }
        else if (prefabToSpawn == playerPrefab2 && player1NetworkObject != null)
        {

            // Assign Player 1's NetworkObjectId to Player 2's follow script
            var followScript = networkObject.GetComponent<Player2Follow>();
            if (followScript != null)
            {
                followScript.SetPlayer1(player1NetworkObject.NetworkObjectId);
            }

            // Make Player 2 a child of Player 1
            var player2Object = networkObject.transform;
            var player1Object = player1NetworkObject.transform;

            //// Set Player 2's parent to Player 1
            //player2Object.parent = player1Object;

            // Optional: Reset local position if needed
            player2Object.localPosition = Vector3.zero;
        }
    }

    private GameObject DecidePrefab(ulong clientId)
    {
        // Example logic: even IDs get Player 1, odd IDs get Player 2
        if (clientId % 2 == 1)
        {
            return playerPrefab1;
        }
        else
        {
            return playerPrefab2;
        }
    }
}
//{
//    public GameObject playerPrefab1;
//    public GameObject playerPrefab2;

//    private NetworkObject player1NetworkObject; // Store Player 1's NetworkObject

//    private void Start()
//    {
//        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
//    }

//    private void OnDestroy()
//    {
//        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
//    }

//    private void OnClientConnected(ulong clientId)
//    {
//        // Decide which prefab to spawn
//        GameObject prefabToSpawn = DecidePrefab(clientId);
//        var networkObject = Instantiate(prefabToSpawn);
//        var netObj = networkObject.GetComponent<NetworkObject>();

//        // Spawn as player object
//        netObj.SpawnAsPlayerObject(clientId);

//        if (prefabToSpawn == playerPrefab1)
//        {
//            // Store reference to Player 1's NetworkObject
//            player1NetworkObject = netObj;
//        }
//        else if (prefabToSpawn == playerPrefab2 && player1NetworkObject != null)
//        {
//            // Assign Player 1's NetworkObjectId to Player 2's follow script
//            var followScript = networkObject.GetComponent<Player2Follow>();
//            if (followScript != null)
//            {
//                followScript.SetPlayer1(player1NetworkObject.NetworkObjectId);
//            }
//        }
//    }

//    private GameObject DecidePrefab(ulong clientId)
//    {
//        // Example logic: even IDs get Player 1, odd IDs get Player 2
//        if (clientId % 2 == 0)
//        {
//            return playerPrefab1;
//        }
//        else
//        {
//            return playerPrefab2;
//        }
//    }
//}
