using UnityEngine;
using Unity.Services.Vivox;
using System.Threading.Tasks;

public class VoiceManager : MonoBehaviour
{
    string currentChannel;

    public async void LoginVivox(string displayName)
    {
        var opts = new LoginOptions() { DisplayName = displayName };
        await VivoxService.Instance.LoginAsync(opts);
        Debug.Log("Logged in Vivox as " + displayName);
    }

    public async void JoinPrivateChannel(string myId, string otherId)
    {
        // Canal privado determinista
        string channelName = $"p2p_{(string.Compare(myId, otherId) < 0 ? myId + "_" + otherId : otherId + "_" + myId)}";

        currentChannel = channelName;

        await VivoxService.Instance.JoinGroupChannelAsync(channelName, ChatCapability.TextAndAudio);

        Debug.Log("Unido al canal " + channelName);
    }

    public async void LeaveChannel()
    {
        if (!string.IsNullOrEmpty(currentChannel))
        {
            await VivoxService.Instance.LeaveChannelAsync(currentChannel);
            Debug.Log("Saliste del canal " + currentChannel);
            currentChannel = null;
        }
    }

    public void MuteMic() => VivoxService.Instance.MuteInputDevice();
    public void UnmuteMic() => VivoxService.Instance.UnmuteInputDevice();

    void OnEnable()
    {
        VivoxService.Instance.ParticipantAddedToChannel += OnParticipantAdded;
        VivoxService.Instance.ParticipantRemovedFromChannel += OnParticipantRemoved;
    }

    void OnDisable()
    {
        if (VivoxService.Instance != null)
        {
            VivoxService.Instance.ParticipantAddedToChannel -= OnParticipantAdded;
            VivoxService.Instance.ParticipantRemovedFromChannel -= OnParticipantRemoved;
        }
    }

    // Ahora los handlers reciben IParticipant
    void OnParticipantAdded(VivoxParticipant participant)
    {
        Debug.Log($"{participant.PlayerId} entró al canal {participant.ChannelName}");
    }

    void OnParticipantRemoved(VivoxParticipant participant)
    {
        Debug.Log($"{participant.PlayerId} salió del canal {participant.ChannelName}");
    }
}
