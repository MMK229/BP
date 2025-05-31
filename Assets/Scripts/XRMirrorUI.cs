using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System.Collections;

namespace Mirror.VR
{
    /// <summary>
    /// VR-friendly NetworkManager HUD that uses Canvas UI elements instead of IMGUI
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/VR Network Manager HUD")]
    [RequireComponent(typeof(NetworkManager))]
    [HelpURL("https://mirror-networking.gitbook.io/docs/components/network-manager-hud")]
    public class VRNetworkManagerHUD : MonoBehaviour
    {
        [Header("Network Settings")]
        [Tooltip("Reference to the NetworkManager component")]
        public NetworkManager manager;

        [Header("UI References")]
        [Tooltip("Canvas that contains all the UI elements")]
        public Canvas networkCanvas;
        
        [Tooltip("Panel containing connection options")]
        public GameObject connectPanel;
        
        [Tooltip("Panel showing connection status")]
        public GameObject statusPanel;
        
        [Tooltip("Button to host a game (Server + Client)")]
        public Button hostButton;
        
        [Tooltip("Button to join as client")]
        public Button clientButton;
        
        [Tooltip("Button to start server only")]
        public Button serverButton;
        
        [Tooltip("Input field for the server address")]
        public InputField addressInput;
        
        [Tooltip("Input field for the server port")]
        public InputField portInput;
        
        [Tooltip("Button to cancel connection attempt")]
        public Button cancelButton;
        
        [Tooltip("Button to mark client as ready")]
        public Button readyButton;
        
        [Tooltip("Button to disconnect/stop host")]
        public Button disconnectButton;
        
        [Tooltip("Text showing current connection status")]
        public Text statusText;

        [Header("VR Settings")]
        [Tooltip("The VR player prefab that should be destroyed after connecting")]
        public GameObject vrPlayerPrefab;
        
        [Tooltip("Time in seconds to wait before destroying the local VR player prefab")]
        public float destroyLocalPlayerDelay = 0.5f;

        void Awake()
        {
            // Get reference to NetworkManager if not set
            if (manager == null)
                manager = GetComponent<NetworkManager>();

            // Setup UI button listeners
            if (hostButton != null)
                hostButton.onClick.AddListener(OnHostButtonClicked);
            
            if (clientButton != null)
                clientButton.onClick.AddListener(OnClientButtonClicked);
            
            if (serverButton != null)
                serverButton.onClick.AddListener(OnServerButtonClicked);
            
            if (cancelButton != null)
                cancelButton.onClick.AddListener(OnCancelButtonClicked);
            
            if (readyButton != null)
                readyButton.onClick.AddListener(OnReadyButtonClicked);
            
            if (disconnectButton != null)
                disconnectButton.onClick.AddListener(OnDisconnectButtonClicked);

            // Initialize address input field
            if (addressInput != null)
                addressInput.text = manager.networkAddress;

            // Initialize port input field if a port transport is used
            if (portInput != null && Transport.active is PortTransport portTransport)
                portInput.text = portTransport.Port.ToString();

            // Initial UI state
            UpdateUIState();
        }

        void Update()
        {
            // Update UI based on current connection state
            UpdateUIState();
        }

        void UpdateUIState()
        {
            // Update UI based on connection state
            bool isConnected = NetworkClient.isConnected || NetworkServer.active;
            
            if (connectPanel != null)
                connectPanel.SetActive(!isConnected);
            
            if (statusPanel != null)
                statusPanel.SetActive(isConnected);
            
            if (readyButton != null)
                readyButton.gameObject.SetActive(NetworkClient.isConnected && !NetworkClient.ready);
            
            // Update status text
            UpdateStatusText();
        }

        void UpdateStatusText()
        {
            if (statusText == null)
                return;

            if (NetworkServer.active && NetworkClient.active)
            {
                statusText.text = $"Host: running via {Transport.active}";
            }
            else if (NetworkServer.active)
            {
                statusText.text = $"Server: running via {Transport.active}";
            }
            else if (NetworkClient.isConnected)
            {
                statusText.text = $"Client: connected to {manager.networkAddress} via {Transport.active}";
            }
            else if (NetworkClient.active)
            {
                statusText.text = $"Connecting to {manager.networkAddress}...";
            }
            else
            {
                statusText.text = "Not connected";
            }
        }

        #region Button Event Handlers
        
        void OnHostButtonClicked()
        {
            UpdateNetworkSettings();
            manager.StartHost();
            StartCoroutine(DestroyLocalVRPlayer());
        }

        void OnClientButtonClicked()
        {
            UpdateNetworkSettings();
            manager.StartClient();
            StartCoroutine(DestroyLocalVRPlayer());
        }

        void OnServerButtonClicked()
        {
            UpdateNetworkSettings();
            manager.StartServer();
        }

        void OnCancelButtonClicked()
        {
            manager.StopClient();
        }

        void OnReadyButtonClicked()
        {
            NetworkClient.Ready();
            if (NetworkClient.localPlayer == null)
                NetworkClient.AddPlayer();
        }

        void OnDisconnectButtonClicked()
        {
            if (NetworkServer.active && NetworkClient.active)
            {
                manager.StopHost();
            }
            else if (NetworkClient.isConnected)
            {
                manager.StopClient();
            }
            else if (NetworkServer.active)
            {
                manager.StopServer();
            }
        }
        
        #endregion

        void UpdateNetworkSettings()
        {
            // Update address from input field
            if (addressInput != null)
                manager.networkAddress = addressInput.text;

            // Update port if using a port transport
            if (portInput != null && Transport.active is PortTransport portTransport)
            {
                if (ushort.TryParse(portInput.text, out ushort port))
                    portTransport.Port = port;
            }
        }

        /// <summary>
        /// Destroys the local VR player prefab after connecting to the network
        /// </summary>
        IEnumerator DestroyLocalVRPlayer()
        {
            // Wait a moment for the connection to establish
            yield return new WaitForSeconds(destroyLocalPlayerDelay);

            // Only proceed if we're connected
            if (!NetworkClient.isConnected)
                yield break;

            if (vrPlayerPrefab != null)
            {
                // Find all instances of the VR player prefab in the scene
                GameObject[] vrPlayers = GameObject.FindGameObjectsWithTag(vrPlayerPrefab.tag);
                foreach (GameObject player in vrPlayers)
                {
                    // Only destroy if it's not a networked player (i.e., it's our local VR rig)
                    if (player.GetComponent<NetworkIdentity>() == null || !player.GetComponent<NetworkIdentity>().isServer)
                    {
                        Debug.Log("Destroying local VR player: " + player.name);
                        Destroy(player);
                    }
                }
            }
        }
    }
}