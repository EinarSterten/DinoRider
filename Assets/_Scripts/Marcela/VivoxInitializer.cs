using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Vivox;

public class VivoxInitializer : MonoBehaviour
{
    [Header("Dev-only: SOLO para pruebas locales")]
    public string vivoxServer;
    public string vivoxDomain;
    public string vivoxIssuer;
    public string vivoxTokenKey;

    async void Awake()
    {
        // Inicialización de Unity Services con credenciales Vivox (dev)
        var options = new InitializationOptions()
            .SetVivoxCredentials(vivoxServer, vivoxDomain, vivoxIssuer, vivoxTokenKey);

        await UnityServices.InitializeAsync(options);

        // Login anónimo a Unity Authentication
        if (!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

        // Inicializar Vivox
        await VivoxService.Instance.InitializeAsync();

        Debug.Log("Vivox inicializado");
    }
}
