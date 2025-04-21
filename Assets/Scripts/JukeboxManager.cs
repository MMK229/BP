using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Agora.Rtc;

public class JukeboxManager : MonoBehaviour
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
    public int musicVolume = 50;
    
    [Range(0, 100)]
    [Tooltip("How loud the music is for remote users")]
    public int publishVolume = 50;
    
    [Range(0, 100)]
    [Tooltip("How loud the music is for local user")]
    public int playoutVolume = 50;
    
    [Header("References")]
    public AudioSource localAudioSource;
    
    // UI Elements
    [Header("UI Elements")]
    public Dropdown trackSelector;
    public Slider volumeSlider;
    public Button playButton;
    public Button stopButton;
    
    // Private fields
    private IRtcEngine _rtcEngine;
    private CoolJoinChannelVideo _mainController;
    private int _currentTrackIndex = -1;
    private bool _isPlaying = false;
    
    private IEnumerator InitializeAfterEngineReady()
    {
        // Wait until CoolJoinChannelVideo initializes RtcEngine
        _mainController = FindObjectOfType<CoolJoinChannelVideo>();
    
        // Wait until we have a valid RtcEngine
        int timeoutCounter = 0;
        while (_mainController == null || _mainController.RtcEngine == null)
        {
            timeoutCounter++;
            if (timeoutCounter > 100) // Timeout after about 10 seconds
            {
                Debug.LogError("Timed out waiting for RtcEngine to initialize.");
                yield break;
            }
        
            Debug.Log($"Waiting for RtcEngine initialization... Attempt {timeoutCounter}");
            _mainController = FindObjectOfType<CoolJoinChannelVideo>();
            yield return new WaitForSeconds(0.1f); // Wait a bit before checking again
        }
    
        _rtcEngine = _mainController.RtcEngine;
        Debug.Log("Successfully got RtcEngine from CoolJoinChannelVideo");
    
        // Set default track if available
        if (availableTracks.Count > 0)
        {
            _currentTrackIndex = 0;
            Debug.Log($"Setting default track to index 0: {availableTracks[0].trackName}");
        }
    }
    
    void Start()
    {
        StartCoroutine(InitializeAfterEngineReady());
        
        // Find main controller and get RtcEngine
        _mainController = FindObjectOfType<CoolJoinChannelVideo>();
        if (_mainController != null)
        {
            _rtcEngine = _mainController.RtcEngine;
            if (_rtcEngine == null)
            {
                Debug.LogError("RtcEngine is null in CoolJoinChannelVideo.");
            }
            else
            {
                Debug.Log("RtcEngine successfully initialized.");
            }
        }
        else
        {
            Debug.LogError("CoolJoinChannelVideo component not found in the scene.");
            return;
        }
    
        // Create local audio source if not assigned
        if (localAudioSource == null)
        {
            localAudioSource = gameObject.AddComponent<AudioSource>();
            localAudioSource.playOnAwake = false;
            localAudioSource.loop = true;
        }
    
        // Set up UI if available
        SetupUI();
    
        // Set default track if available
        if (availableTracks.Count > 0 && _currentTrackIndex < 0)
        {
            _currentTrackIndex = 0;
            Debug.Log($"Setting default track to index 0: {availableTracks[0].trackName}");
        }
        else if (availableTracks.Count == 0)
        {
            Debug.LogWarning("No music tracks available in the Jukebox.");
        }
    }
    
    private void SetupUI()
    {
        // Set up track dropdown
        if (trackSelector != null)
        {
            trackSelector.ClearOptions();
            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
            
            foreach (var track in availableTracks)
            {
                options.Add(new Dropdown.OptionData(track.trackName));
            }
            
            trackSelector.AddOptions(options);
            trackSelector.onValueChanged.AddListener(OnTrackSelected);
        }
        
        // Set up volume slider
        if (volumeSlider != null)
        {
            volumeSlider.value = musicVolume;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }
        
        // Set up buttons
        if (playButton != null)
        {
            playButton.onClick.AddListener(PlayMusic);
        }
        
        if (stopButton != null)
        {
            stopButton.onClick.AddListener(StopMusic);
        }
    }
    
    public void OnTrackSelected(int trackIndex)
    {
        if (trackIndex >= 0 && trackIndex < availableTracks.Count)
        {
            _currentTrackIndex = trackIndex;
            
            // If we're currently playing, restart with the new track
            if (_isPlaying)
            {
                StopMusic();
                PlayMusic();
            }
        }
    }
    
    public void OnVolumeChanged(float volume)
    {
        musicVolume = Mathf.RoundToInt(volume);
        
        // Update local playback volume
        if (localAudioSource != null)
        {
            localAudioSource.volume = musicVolume / 100f;
        }
        
        // Update remote playback volumes
        if (_rtcEngine != null && _isPlaying)
        {
            _rtcEngine.AdjustAudioMixingPublishVolume(publishVolume);
            _rtcEngine.AdjustAudioMixingPlayoutVolume(playoutVolume);
        }
    }
    
public void PlayMusic()
{
    Debug.Log("PlayMusic called");
    
    if (_rtcEngine == null)
    {
        Debug.LogError("Cannot play music: RTC Engine is not initialized");
        return;
    }
    
    if (_currentTrackIndex < 0 || _currentTrackIndex >= availableTracks.Count)
    {
        Debug.LogError($"Cannot play music: Invalid track index {_currentTrackIndex}. Available tracks: {availableTracks.Count}");
        return;
    }

    MusicTrack track = availableTracks[_currentTrackIndex];

    // Stop any currently playing music
    StopMusic();
    
    // For Unity application users - play locally through AudioSource
    if (localAudioSource != null && track.audioClip != null)
    {
        localAudioSource.clip = track.audioClip;
        localAudioSource.volume = musicVolume / 100f;
        localAudioSource.Play();
        Debug.Log($"Started local playback of: {track.trackName}");
    }
    else
    {
        Debug.LogWarning("Local AudioSource or AudioClip is null");
    }

    // For web app users - stream through Agora
    if (!string.IsNullOrEmpty(track.audioFilePath))
    {
        string fullPath = Path.Combine(Application.streamingAssetsPath, track.audioFilePath);
        
        if (!File.Exists(fullPath))
        {
            Debug.LogError($"Audio file not found at path: {fullPath}");
            return;
        }
        
        Debug.Log($"Starting Agora audio mixing with file: {fullPath}");
        
        int result = _rtcEngine.StartAudioMixing(
            filePath: fullPath,
            loopback: false,   // false = only send to remote users, not local
            cycle: -1          // -1 = loop indefinitely
        );

        if (result != 0)
        {
            Debug.LogError($"Failed to start Agora audio mixing with error code: {result}");
            return;
        }

        // Only set publish volume since we don't want local playback through Agora
        _rtcEngine.AdjustAudioMixingPublishVolume(publishVolume);
        // Set playout volume to 0 to ensure we don't hear double audio in Unity
        _rtcEngine.AdjustAudioMixingPlayoutVolume(0);
        
        Debug.Log($"Started remote streaming of: {track.trackName}");
    }
    else
    {
        Debug.LogWarning("Audio file path is empty, cannot play for remote users");
    }

    _isPlaying = true;
    UpdatePlaybackUI();
}

    
    public void StopMusic()
    {
        // Stop local playback
        if (localAudioSource != null && localAudioSource.isPlaying)
        {
            localAudioSource.Stop();
        }
        
        // Stop remote playback via Agora
        if (_rtcEngine != null && _isPlaying)
        {
            _rtcEngine.StopAudioMixing();
        }
        
        _isPlaying = false;
        Debug.Log("Stopped music playback");
        
        // Update UI
        UpdatePlaybackUI();
    }
    
    private void UpdatePlaybackUI()
    {
        if (playButton != null)
        {
            playButton.interactable = !_isPlaying;
        }
        
        if (stopButton != null)
        {
            stopButton.interactable = _isPlaying;
        }
    }
    
void Update()
{
    if (Input.anyKeyDown)
    {
        Debug.Log("Some key pressed.");
    }

    for (int i = 0; i < 9; i++)
    {
        if (Input.GetKeyDown(KeyCode.Alpha1 + i) || Input.GetKeyDown(KeyCode.Keypad1 + i))
        {
            int index = i;
            Debug.Log($"Key {index + 1} pressed.");
            
            if (index < availableTracks.Count)
            {
                Debug.Log($"Valid track index {index}: {availableTracks[index].trackName}");
                OnTrackSelected(index);
                PlayMusic(); // Explicitly play after selecting
            }
            else
            {
                Debug.LogWarning($"Track index {index} is out of range. Only {availableTracks.Count} tracks available.");
            }
        }
    }
    
    // Add space key as play/pause toggle
    if (Input.GetKeyDown(KeyCode.Space))
    {
        if (_isPlaying)
        {
            Debug.Log("Space pressed: Stopping music");
            StopMusic();
        }
        else if (_currentTrackIndex >= 0)
        {
            Debug.Log("Space pressed: Playing music");
            PlayMusic();
        }
    }
}
    
    // Clean up when destroyed
    void OnDestroy()
    {
        StopMusic();
    }
}