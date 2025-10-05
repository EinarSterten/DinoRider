using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI_Einar : MonoBehaviour
{
    [SerializeField] private Button serverButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    [SerializeField] private PlayerSpawner_Reworked spawner; 

    private void Awake()
    {
        serverButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });
        hostButton.onClick.AddListener(() =>
        {
            spawner.StartHostAndSpawn(); 
        });
        clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }
}
