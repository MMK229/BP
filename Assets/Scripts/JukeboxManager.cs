using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Agora.Rtc;
using Mirror;
using UnityEngine.XR.Interaction.Toolkit;

public class JukeboxManager : NetworkBehaviour
{
    [System.Serializable]
    public class MusicTrack
    {
        public string trackName;
        public AudioClip audioClip;
        [Tooltip("Path to the file relative to the streaming assets folder")]
        public string audioFilePath;
    }

    [Header("Music Settings")]
    public List<MusicTrack> availableTracks = new List<MusicTrack>();
    
    [Range(0, 100)]
    [Tooltip("Volume loudness for remote users")]
    public int publishVolume = 50;
    
    [Range(0, 100)]
    [Tooltip("How loud the music is for local user (via Agora)")]
    public int playoutVolume = 50;
    
    [Header("References")]
    public AudioSource localAudioSource;
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable xrInteractable;
    
    [SyncVar(hook = nameof(OnTrackIndexChanged))]
    private int _networkedTrackIndex = -1;
    
    [SyncVar(hook = nameof(OnPlayingStateChanged))]
    private bool _networkedIsPlaying = false;
    
    private IRtcEngine _rtcEngine;
    private JoinChannelVideo _mainController;
    private bool _isInitialized = false;

    // Korutina keďže JukeboxManager sa musí spustiť až po JoinChannelVideo
    private IEnumerator InitializeAfterEngineReady()
    {
        // Najprv potrebujeme počkať aby JoinChannelVideo správne všetko nastavilo
        _mainController = FindObjectOfType<JoinChannelVideo>();
    
        // Počkáme dokedy máme RtcEngine
        int timeoutCounter = 0;
        while (_mainController == null || _mainController.RtcEngine == null)
        {
            timeoutCounter++;
            if (timeoutCounter > 1000)
            {
                Debug.LogError("JukeboxManager: Timed out waiting for RtcEngine to initialize.");
                yield break;
            }
        
            _mainController = FindObjectOfType<JoinChannelVideo>();
            yield return new WaitForSeconds(0.1f);
        }
    
        _rtcEngine = _mainController.RtcEngine;
        _isInitialized = true;
        if (!isServer)
        {
            SyncToNetworkedState();
        }
    }
    
    void Start()
    {
        // Ošetrenia
        
        // Počkáme až keď sa načíta Agora aby to správne fungovalo
        StartCoroutine(InitializeAfterEngineReady());
        // Ak neexistuje AudioSource tak ho vyrobíme
        if (localAudioSource == null)
        {
            localAudioSource = gameObject.AddComponent<AudioSource>();
            localAudioSource.playOnAwake = false;
            localAudioSource.loop = true;
        }
    }
    
    // Pre istotu sa jukebox aktivuje aj pri výbere aj aktivácii (trigger press)
    void OnXRSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log($"Jukebox selected by: {args.interactorObject.transform.name}");
        CmdHandleJukeboxTap();
    }
    void OnXRActivated(ActivateEventArgs args)
    {
        Debug.Log($"Jukebox activated by: {args.interactorObject.transform.name}");
        CmdHandleJukeboxTap();
    }
    
    [Command(requiresAuthority = false)]
    void CmdHandleJukeboxTap()
    {
        Debug.Log("Server: Handling jukebox tap command");

        if (!isServer) return;
        
        // Ak už niečo hrá tak hudbu zastavíme
        if (_networkedIsPlaying)
        {
            ServerStopMusic();
        }
        
        // Cyklujeme cez piesne (OFF -> id1 -> id2 -> ... -> OFF)
        _networkedTrackIndex++;
        if (_networkedTrackIndex >= availableTracks.Count)
        {
            _networkedTrackIndex = -1; // Toto je OFF state, s týmto budeme vypínať hudbu
        }

        if (_networkedTrackIndex == -1)
        {
            Debug.Log("Server: Cycling to OFF state.");
            _networkedIsPlaying = false;
        }
        else
        {
            Debug.Log($"Server: Cycling to track index {_networkedTrackIndex}: {availableTracks[_networkedTrackIndex].trackName}");
            ServerPlayMusic(); // Hráme track podľa id
        }
    }

    // Hranie hudby na serveri (aby každý počul to isté)
    [Server]
    void ServerPlayMusic()
    {
        Debug.Log("Server: PlayMusic called.");
        if (_networkedTrackIndex < 0 || _networkedTrackIndex >= availableTracks.Count)
        {
            Debug.LogError($"Server: Cannot play music: Invalid track index {_networkedTrackIndex}. Available tracks: {availableTracks.Count}.");
            return;
        }
        _networkedIsPlaying = true;
    }

    // Stop hudby
    [Server]
    void ServerStopMusic()
    {
        Debug.Log("Server: StopMusic called.");
        _networkedIsPlaying = false;
    }

    // SyncVar - keď sa zmení track id
    void OnTrackIndexChanged(int oldIndex, int newIndex)
    {
        Debug.Log($"Client: Track index changed from {oldIndex} to {newIndex}");
        
        if (!_isInitialized)
        {
            return;
        }

        if (newIndex == -1 || !_networkedIsPlaying)
        {
            ClientStopMusic();
        }
        else if (_networkedIsPlaying)
        {
            ClientPlayMusic(newIndex);
        }
    }

    // SyncVar - zavolá sa na klientoch keď sa zmení stav
    void OnPlayingStateChanged(bool oldState, bool newState)
    {
        Debug.Log($"Client: Playing state changed from {oldState} to {newState}");
        
        if (!_isInitialized)
        {
            return;
        }
        
        if (newState && _networkedTrackIndex >= 0)
        {
            ClientPlayMusic(_networkedTrackIndex);
        }
        else
        {
            ClientStopMusic();
        }
    }

    // Playback hudby
    void ClientPlayMusic(int trackIndex)
    {
        Debug.Log($"Client: Playing music for track index {trackIndex}");
        
        // Debugy, treba rtcEngine a validný track id
        if (_rtcEngine == null)
        {
            Debug.LogError("Client: Cannot play music: RTC Engine is not initialized.");
            return;
        }
        
        if (trackIndex < 0 || trackIndex >= availableTracks.Count)
        {
            Debug.LogError($"Client: Cannot play music: Invalid track index {trackIndex}. Available tracks: {availableTracks.Count}.");
            return;
        }

        MusicTrack track = availableTracks[trackIndex];

        // Najprv zastavíme všetku hudbu
        ClientStopMusic(); 
        
        // Nastavíme AudioClip na lokálny AudioSource
        if (localAudioSource != null && track.audioClip != null)
        {
            localAudioSource.clip = track.audioClip;
            Debug.Log($"Client: Local AudioSource clip set to: {track.trackName}");
        }
        else
        {
            Debug.LogWarning("Client: Local AudioSource or AudioClip is null, local clip assignment skipped.");
        }

        // Agora stream
        if (!string.IsNullOrEmpty(track.audioFilePath))
        {
            string fullPath = Path.Combine(Application.streamingAssetsPath, track.audioFilePath);
            
            if (!File.Exists(fullPath))
            {
                Debug.LogError($"Client: Audio file not found at path: {fullPath}");
                return;
            }
            
            // Agora audio mixing
            int result = _rtcEngine.StartAudioMixing(
                filePath: fullPath,
                loopback: true,
                cycle: -1
            );

            if (result != 0)
            {
                Debug.LogError($"Client: Failed to start Agora audio mixing with error code: {result}");
                return;
            }
            _rtcEngine.AdjustAudioMixingPublishVolume(publishVolume);
            _rtcEngine.AdjustAudioMixingPlayoutVolume(playoutVolume);
            Debug.Log($"Client: Started remote streaming and local Agora playback of: {track.trackName}");
        }
        else
        {
            Debug.LogWarning("Client: Audio file path is empty, cannot play for remote users or local Agora playback.");
        }
    }

    // Zastavenie hudby
    void ClientStopMusic()
    {
        
        Debug.Log("Client: Stopping music");
        if (localAudioSource != null && localAudioSource.isPlaying)
        {
            localAudioSource.Stop();
            Debug.Log("Client: Stopped local AudioSource playback.");
        }
        
        if (_rtcEngine != null)
        {
            _rtcEngine.StopAudioMixing();
            Debug.Log("Client: Stopped Agora audio mixing.");
        }
    }

    // Synchronizácia
    void SyncToNetworkedState()
    {
        if (_networkedIsPlaying && _networkedTrackIndex >= 0)
        {
            ClientPlayMusic(_networkedTrackIndex);
        }
        else
        {
            ClientStopMusic();
        }
    }
    
    
    // Cleanup pri vymazaní
    void OnDestroy()
    {
        ClientStopMusic();
        if (xrInteractable != null)
        {
            xrInteractable.selectEntered.RemoveListener(OnXRSelectEntered);
            xrInteractable.activated.RemoveListener(OnXRActivated);
        }
    }
}