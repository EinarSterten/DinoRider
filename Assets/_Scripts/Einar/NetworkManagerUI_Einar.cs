using System.Collections;
using TMPro; // for TMP_InputField
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using System;

public class NetworkManagerUI_Einar : NetworkBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private TMP_InputField joinCodeInputField;

    [SerializeField] private PlayerSpawner_Reworked spawner;

    [SerializeField] private RelayManager relayManager; // Reference to your RelayManager

    private void Awake()
    {
        // Host button
        hostButton.onClick.AddListener(() => StartCoroutine(StartHostRoutine()));

        // Join button
        joinButton.onClick.AddListener(() => StartCoroutine(StartJoinRoutine()));
    }

    private IEnumerator StartHostRoutine()
    {
        hostButton.interactable = false;

        // Setup relay
        var relayTask = relayManager.SetupRelayAsync(4, RelayManager.ConnectionType.Udp);
        yield return new WaitUntil(() => relayTask.IsCompleted);

        if (relayTask.Exception != null)
        {
            Debug.LogError($"Relay setup failed: {relayTask.Exception}");
            hostButton.interactable = true;
            yield break;
        }

        string joinCode = relayTask.Result;
        Debug.Log($"Relay Join Code: {joinCode}");

        // Start host
        NetworkManager.Singleton.OnServerStarted += spawner.SpawnHostPlayer;
        NetworkManager.Singleton.StartHost();

        hostButton.interactable = true;
    }

    private IEnumerator StartJoinRoutine()
    {
        string joinCode = joinCodeInputField.text;

        if (string.IsNullOrEmpty(joinCode))
        {
            Debug.LogWarning("Join code is empty");
            yield break;
        }

        // Join relay
        var joinTask = relayManager.JoinRelayAsync(joinCode);
        yield return new WaitUntil(() => joinTask.IsCompleted);

        if (joinTask.Exception != null)
        {
            Debug.LogError($"Join relay failed: {joinTask.Exception}");
            yield break;
        }

        // Start client
        NetworkManager.Singleton.StartClient();
    }
}