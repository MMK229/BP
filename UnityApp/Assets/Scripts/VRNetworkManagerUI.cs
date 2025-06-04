using UnityEngine;
using UnityEngine.UI;
using Mirror;

namespace Mirror.VR
{
    /// <summary>
    /// Helper class to set up the VR Network Manager HUD UI
    /// </summary>
    public class VRNetworkUISetup : MonoBehaviour
    {
        [Header("Network Components")]
        [Tooltip("Reference to the NetworkManager")]
        public NetworkManager networkManager;
        
        [Tooltip("Reference to the VR Network Manager HUD")]
        public VRNetworkManagerHUD vrNetworkHUD;
        
        [Header("UI References")]
        [Tooltip("Canvas that will be attached to the VR camera")]
        public Canvas networkCanvas;
        
        [Tooltip("The VR player prefab reference")]
        public GameObject vrPlayerPrefab;

        [Header("UI Elements")]
        [Tooltip("Panel for connection controls")]
        public GameObject connectPanel;
        
        [Tooltip("Panel for status display")]
        public GameObject statusPanel;
        
        [Tooltip("Host button")]
        public Button hostButton;
        
        [Tooltip("Client button")]
        public Button clientButton;
        
        [Tooltip("Server button")]
        public Button serverButton;
        
        [Tooltip("IP Address input field")]
        public InputField addressInput;
        
        [Tooltip("Port input field")]
        public InputField portInput;
        
        [Tooltip("Cancel connection button")]
        public Button cancelButton;
        
        [Tooltip("Ready button")]
        public Button readyButton;
        
        [Tooltip("Disconnect button")]
        public Button disconnectButton;
        
        [Tooltip("Status text")]
        public Text statusText;

        private void Awake()
        {
            SetupNetworkComponents();
            ConfigureVRCanvas();
        }

        private void SetupNetworkComponents()
        {
            // Create VRNetworkManagerHUD component if needed
            if (vrNetworkHUD == null && networkManager != null)
            {
                vrNetworkHUD = networkManager.gameObject.GetComponent<VRNetworkManagerHUD>();
                if (vrNetworkHUD == null)
                {
                    vrNetworkHUD = networkManager.gameObject.AddComponent<VRNetworkManagerHUD>();
                }
            }

            // Configure VRNetworkManagerHUD
            if (vrNetworkHUD != null)
            {
                vrNetworkHUD.manager = networkManager;
                vrNetworkHUD.networkCanvas = networkCanvas;
                vrNetworkHUD.connectPanel = connectPanel;
                vrNetworkHUD.statusPanel = statusPanel;
                vrNetworkHUD.hostButton = hostButton;
                vrNetworkHUD.clientButton = clientButton;
                vrNetworkHUD.serverButton = serverButton;
                vrNetworkHUD.addressInput = addressInput;
                vrNetworkHUD.portInput = portInput;
                vrNetworkHUD.cancelButton = cancelButton;
                vrNetworkHUD.readyButton = readyButton;
                vrNetworkHUD.disconnectButton = disconnectButton;
                vrNetworkHUD.statusText = statusText;
                vrNetworkHUD.vrPlayerPrefab = vrPlayerPrefab;
            }
        }

        private void ConfigureVRCanvas()
        {
            if (networkCanvas == null)
                return;

            // Make sure canvas is set up for VR
            networkCanvas.renderMode = RenderMode.WorldSpace;
            
            // Set reasonable world space size and position
            RectTransform canvasRect = networkCanvas.GetComponent<RectTransform>();
            if (canvasRect != null)
            {
                canvasRect.sizeDelta = new Vector2(1f, 0.75f); // Width x Height in world units
                canvasRect.localScale = new Vector3(0.001f, 0.001f, 0.001f); // Scale down to reasonable size
            }
            
            // Add a canvas follower script to make it follow the VR camera
            VRCanvasFollower follower = networkCanvas.gameObject.GetComponent<VRCanvasFollower>();
            if (follower == null)
            {
                follower = networkCanvas.gameObject.AddComponent<VRCanvasFollower>();
            }
        }
    }

    /// <summary>
    /// Makes a canvas follow the VR camera
    /// </summary>
    public class VRCanvasFollower : MonoBehaviour
    {
        [Tooltip("The distance in front of the camera to position the canvas")]
        public float distanceFromCamera = 1.5f;
        
        [Tooltip("How quickly the canvas moves to follow the camera")]
        public float followSpeed = 5f;

        private Transform cameraTransform;
        private bool cameraFound = false;

        private void Start()
        {
            FindVRCamera();
        }

        private void Update()
        {
            if (!cameraFound)
            {
                FindVRCamera();
                return;
            }

            // Position the canvas in front of the camera
            Vector3 targetPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
            
            // Make the canvas face the camera
            transform.rotation = Quaternion.Lerp(
                transform.rotation, 
                Quaternion.LookRotation(transform.position - cameraTransform.position), 
                Time.deltaTime * followSpeed
            );
        }

        private void FindVRCamera()
        {
            // Try to find VR camera
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                cameraTransform = mainCamera.transform;
                cameraFound = true;
                
                // Initial positioning
                transform.position = cameraTransform.position + cameraTransform.forward * distanceFromCamera;
                transform.rotation = Quaternion.LookRotation(transform.position - cameraTransform.position);
            }
        }
    }
}