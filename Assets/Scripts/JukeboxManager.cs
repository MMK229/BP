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
    public int musicVolume = 50; // This will act as the base for publish/playout volume
    
    [Range(0, 100)]
    [Tooltip("How loud the music is for remote users")]
    public int publishVolume = 50; // Use this directly for Agora's publish volume
    
    [Range(0, 100)]
    [Tooltip("How loud the music is for local user (via Agora)")]
    public int playoutVolume = 50; // Use this directly for Agora's playout volume
    
    [Header("References")]
    public AudioSource localAudioSource; // Keep for clip assignment, but Agora will handle local playback via mixing
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable xrInteractable; // XR Interaction component
    
    // Network synchronized variables
    [SyncVar(hook = nameof(OnTrackIndexChanged))]
    private int _networkedTrackIndex = -1; // -1 means OFF, 0 to N-1 are tracks
    
    [SyncVar(hook = nameof(OnPlayingStateChanged))]
    private bool _networkedIsPlaying = false;
    
    // Private fields
    private IRtcEngine _rtcEngine;
    private JoinChannelVideo _mainController;
    private bool _isInitialized = false;

    // Coroutine to wait for the RtcEngine to be ready from JoinChannelVideo
    private IEnumerator InitializeAfterEngineReady()
    {
        // Wait until CoolJoinChannelVideo initializes RtcEngine
        _mainController = FindObjectOfType<JoinChannelVideo>();
    
        // Wait until we have a valid RtcEngine
        int timeoutCounter = 0;
        while (_mainController == null || _mainController.RtcEngine == null)
        {
            timeoutCounter++;
            if (timeoutCounter > 1000) // Timeout after about 100 seconds (1000 * 0.1s)
            {
                Debug.LogError("JukeboxManager: Timed out waiting for RtcEngine to initialize.");
                yield break;
            }
        
            _mainController = FindObjectOfType<JoinChannelVideo>();
            yield return new WaitForSeconds(0.1f); // Wait a bit before checking again
        }
    
        _rtcEngine = _mainController.RtcEngine;
        Debug.Log("JukeboxManager: Successfully got RtcEngine from JoinChannelVideo.");
        
        _isInitialized = true;
        
        // If we're not the server, sync to current server state
        if (!isServer)
        {
            SyncToNetworkedState();
        }
    }
    
    void Start()
    {
        // Start the coroutine to wait for the Agora engine
        StartCoroutine(InitializeAfterEngineReady());
        
        // Create local audio source if not assigned
        if (localAudioSource == null)
        {
            localAudioSource = gameObject.AddComponent<AudioSource>();
            localAudioSource.playOnAwake = false;
            localAudioSource.loop = true; // Music tracks usually loop
        }
        
        // Setup XR Interaction
        SetupXRInteraction();
    }
    
    void SetupXRInteraction()
    {
        // Get or create XRSimpleInteractable component
        if (xrInteractable == null)
        {
            xrInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
            if (xrInteractable == null)
            {
                xrInteractable = gameObject.AddComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
            }
        }
        
        // Ensure we have a collider for XR interaction
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            // Add a box collider if none exists
            BoxCollider boxCol = gameObject.AddComponent<BoxCollider>();
            boxCol.isTrigger = true;
            Debug.Log("JukeboxManager: Added BoxCollider for XR interaction");
        }
        
        // Subscribe to XR interaction events
        xrInteractable.selectEntered.AddListener(OnXRSelectEntered);
        xrInteractable.activated.AddListener(OnXRActivated);
        
        Debug.Log("JukeboxManager: XR Interaction setup complete");
    }
    
    // Called when XR controller selects (grabs) the jukebox
    void OnXRSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log($"Jukebox selected by: {args.interactorObject.transform.name}");
        OnJukeboxTap();
    }
    
    // Called when XR controller activates (trigger press) while selecting
    void OnXRActivated(ActivateEventArgs args)
    {
        Debug.Log($"Jukebox activated by: {args.interactorObject.transform.name}");
        OnJukeboxTap();
    }
    
    // This method will be called by your VR interaction system (e.g., on pointer click, on gaze select, etc.)
    public void OnJukeboxTap()
    {
        Debug.Log("Jukebox Tapped!");
        
        // Send command to server to handle the tap
        CmdHandleJukeboxTap();
    }

    [Command(requiresAuthority = false)]
    void CmdHandleJukeboxTap()
    {
        Debug.Log("Server: Handling jukebox tap command");
        
        // Only server processes the actual logic
        if (!isServer) return;
        
        // Stop current music if playing to transition cleanly
        if (_networkedIsPlaying)
        {
            ServerStopMusic();
        }
        
        // Cycle to the next state (OFF -> Song 1 -> Song 2 -> ... -> OFF)
        _networkedTrackIndex++;
        if (_networkedTrackIndex >= availableTracks.Count)
        {
            _networkedTrackIndex = -1; // Cycle back to OFF
        }

        if (_networkedTrackIndex == -1)
        {
            Debug.Log("Server: Cycling to OFF state.");
            _networkedIsPlaying = false;
        }
        else
        {
            Debug.Log($"Server: Cycling to track index {_networkedTrackIndex}: {availableTracks[_networkedTrackIndex].trackName}");
            ServerPlayMusic();
        }
    }

    // Server-side music control
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
        
        // The actual playback will be handled by the SyncVar hooks on all clients
    }

    [Server]
    void ServerStopMusic()
    {
        Debug.Log("Server: StopMusic called.");
        _networkedIsPlaying = false;
    }

    // SyncVar hook - called on all clients when track index changes
    void OnTrackIndexChanged(int oldIndex, int newIndex)
    {
        Debug.Log($"Client: Track index changed from {oldIndex} to {newIndex}");
        
        if (!_isInitialized)
        {
            // If not initialized yet, we'll sync later
            return;
        }
        
        // Update local state and handle playback
        if (newIndex == -1 || !_networkedIsPlaying)
        {
            ClientStopMusic();
        }
        else if (_networkedIsPlaying)
        {
            ClientPlayMusic(newIndex);
        }
    }

    // SyncVar hook - called on all clients when playing state changes
    void OnPlayingStateChanged(bool oldState, bool newState)
    {
        Debug.Log($"Client: Playing state changed from {oldState} to {newState}");
        
        if (!_isInitialized)
        {
            // If not initialized yet, we'll sync later
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

    // Client-side music playback (called via SyncVar hooks)
    void ClientPlayMusic(int trackIndex)
    {
        Debug.Log($"Client: Playing music for track index {trackIndex}");
        
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

        // Ensure any previously playing music is stopped cleanly
        ClientStopMusic(); 
        
        // Assign the AudioClip to the local AudioSource for reference/potential future use,
        // but Agora will handle the actual local playback via audio mixing loopback.
        if (localAudioSource != null && track.audioClip != null)
        {
            localAudioSource.clip = track.audioClip;
            // localAudioSource.Play() is REMOVED to avoid double audio when Agora's loopback is active.
            Debug.Log($"Client: Local AudioSource clip set to: {track.trackName}");
        }
        else
        {
            Debug.LogWarning("Client: Local AudioSource or AudioClip is null, local clip assignment skipped.");
        }

        // Stream through Agora for both remote users AND local user (loopback: true)
        if (!string.IsNullOrEmpty(track.audioFilePath))
        {
            string fullPath = Path.Combine(Application.streamingAssetsPath, track.audioFilePath);
            
            if (!File.Exists(fullPath))
            {
                Debug.LogError($"Client: Audio file not found at path: {fullPath}");
                return;
            }
            
            Debug.Log($"Client: Starting Agora audio mixing with file: {fullPath}");
            
            int result = _rtcEngine.StartAudioMixing(
                filePath: fullPath,
                loopback: true,   // true = send to remote users AND play locally
                cycle: -1          // -1 = loop indefinitely
            );

            if (result != 0)
            {
                Debug.LogError($"Client: Failed to start Agora audio mixing with error code: {result}");
                return;
            }

            // Adjust volumes for both publishing (remote) and playout (local via Agora)
            _rtcEngine.AdjustAudioMixingPublishVolume(publishVolume);
            _rtcEngine.AdjustAudioMixingPlayoutVolume(playoutVolume);
            
            Debug.Log($"Client: Started remote streaming and local Agora playback of: {track.trackName}");
        }
        else
        {
            Debug.LogWarning("Client: Audio file path is empty, cannot play for remote users or local Agora playback.");
        }
    }

    void ClientStopMusic()
    {
        Debug.Log("Client: Stopping music");
        
        // Stop local playback if it was somehow started directly by AudioSource
        if (localAudioSource != null && localAudioSource.isPlaying)
        {
            localAudioSource.Stop();
            Debug.Log("Client: Stopped local AudioSource playback.");
        }
        
        // Stop remote and local playback via Agora audio mixing
        if (_rtcEngine != null)
        {
            _rtcEngine.StopAudioMixing();
            Debug.Log("Client: Stopped Agora audio mixing.");
        }
    }

    // Sync to current networked state (called when client initializes after server)
    void SyncToNetworkedState()
    {
        Debug.Log($"Client: Syncing to networked state - Track: {_networkedTrackIndex}, Playing: {_networkedIsPlaying}");
        
        if (_networkedIsPlaying && _networkedTrackIndex >= 0)
        {
            ClientPlayMusic(_networkedTrackIndex);
        }
        else
        {
            ClientStopMusic();
        }
    }
    
    // Public methods for external control (optional)
    public void ForceStop()
    {
        if (isServer)
        {
            ServerStopMusic();
        }
    }
    
    public void SetTrack(int trackIndex)
    {
        if (isServer && trackIndex >= -1 && trackIndex < availableTracks.Count)
        {
            if (_networkedIsPlaying)
            {
                ServerStopMusic();
            }
            
            _networkedTrackIndex = trackIndex;
            
            if (trackIndex >= 0)
            {
                ServerPlayMusic();
            }
        }
    }
    
    // Properties for UI/debugging
    public int CurrentTrackIndex => _networkedTrackIndex;
    public bool IsPlaying => _networkedIsPlaying;
    public string CurrentTrackName => (_networkedTrackIndex >= 0 && _networkedTrackIndex < availableTracks.Count) 
        ? availableTracks[_networkedTrackIndex].trackName 
        : "OFF";
    
    // Clean up when destroyed
    void OnDestroy()
    {
        ClientStopMusic();
        
        // Unsubscribe from XR events to prevent memory leaks
        if (xrInteractable != null)
        {
            xrInteractable.selectEntered.RemoveListener(OnXRSelectEntered);
            xrInteractable.activated.RemoveListener(OnXRActivated);
        }
    }
}