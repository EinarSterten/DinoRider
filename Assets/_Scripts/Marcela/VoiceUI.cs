using UnityEngine;
using UnityEngine.UI;

public class VoiceUI : MonoBehaviour
{
    public VoiceManager voiceManager;

    public void OnLoginPlayer1()
    {
        voiceManager.LoginVivox("Jugador1");
    }

    public void OnLoginPlayer2()
    {
        voiceManager.LoginVivox("Jugador2");
    }

    public void OnJoin()
    {
        voiceManager.JoinPrivateChannel("Jugador1", "Jugador2");
    }

    public void OnLeave()
    {
        voiceManager.LeaveChannel();
    }

    public void OnMute() => voiceManager.MuteMic();
    public void OnUnmute() => voiceManager.UnmuteMic();

}