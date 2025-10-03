using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class Player2Follow : NetworkBehaviour
{
    private NetworkVariable<ulong> player1ObjectID = new NetworkVariable<ulong>();

    public Vector3 offset = new Vector3(0, 6f, 0);

    private NetworkObject player1Object; // Reference to Player 1's NetworkObject
    private new AnticipatedNetworkTransform transform;
    public override void OnNetworkSpawn()
    {
        player1ObjectID.Initialize(this);
        base.OnNetworkSpawn();
        transform = GetComponent<AnticipatedNetworkTransform>();
        player1ObjectID.OnValueChanged += SetPlayer1;
    }

    // Call this from your spawner to set Player 1's NetworkObjectId
    private void SetPlayer1(ulong old, ulong player1NetworkObjectId)    
    {
        // Find Player 1's NetworkObject in the scene
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(player1NetworkObjectId, out var netObj))
        {
            player1Object = netObj.GetComponent<NetworkObject>();
        }
        else
        {
            Debug.LogWarning("Player 1 object not found");
        }
    }

    public void SetPlayer1(ulong player1ObjectID)
    {
        this.player1ObjectID.Value = player1ObjectID;
        if (IsServer)
        {
            SetPlayer1(0, player1ObjectID);
        }
    }

    private void FixedUpdate()
    {
        // Ensure this runs on all clients
        if (player1Object != null)
        {
            // Follow Player 1 with an offset
            transform.AnticipateMove(player1Object.transform.position + offset);
        }
    }
    public Camera playerCamera;

    void Start()
    {
        if (IsLocalPlayer)
        {
            // Enable camera only for local player
            playerCamera.gameObject.SetActive(true);
        }
        else
        {
            // Disable for remote players
            playerCamera.gameObject.SetActive(false);
        }
    }

}