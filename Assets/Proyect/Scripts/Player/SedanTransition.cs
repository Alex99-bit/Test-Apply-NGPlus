using ArcadeVP;
using UnityEngine;

/// <summary>
/// Handles player interaction for entering and exiting the sedan vehicle.
/// </summary>
public class SedanTransition : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Camera to activate when inside the sedan.")]
    public GameObject cameraSedan;

    [Tooltip("Reference to the player GameObject.")]
    public GameObject player;

    [Tooltip("Player mesh renderers to hide/show the player model.")]
    public SkinnedMeshRenderer[] playerMeshRenderers;

    [Tooltip("Cinemachine camera for the player.")]
    public GameObject playerCinemachine;

    [Tooltip("Input manager for vehicle controls.")]
    public InputManager_ArcadeVP inputManagerArcade;

    [Tooltip("Arcade vehicle controller script.")]
    public ArcadeVehicleController arcadeVehicleController;

    [Tooltip("UI text shown when player can interact (Press E).")]
    public GameObject pressE_txt;

    [Header("Interaction")]
    [Tooltip("Distance required to interact with the sedan.")]
    public float interactionDistance = 2f;

    [Tooltip("Transform where the player will appear when exiting the car.")]
    public Transform exitPoint;
    
    [Tooltip("Reference to the PlayerMecha script for player controls.")]
    public PlayerMecha playerMecha;

    private CharacterController playerController;

    private bool isInCar = false;

    void Start()
    {
        // Hide interaction text and sedan camera at start
        pressE_txt.SetActive(false);
        cameraSedan.SetActive(false);

        // Ensure player model is visible at start
        foreach (var renderer in playerMeshRenderers)
        {
            renderer.enabled = true;
        }

        // Enable player camera and disable vehicle controls
        playerCinemachine.SetActive(true);
        inputManagerArcade.enabled = false;
        arcadeVehicleController.enabled = false;

        if (player != null)
        {
            playerController = player.GetComponent<CharacterController>();
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Show "Press E" text if player is close and not inside the car
        if (!isInCar && distanceToPlayer <= interactionDistance)
        {
            pressE_txt.SetActive(true);

            // Enter car on E key press
            if (Input.GetKeyDown(KeyCode.E))
            {
                EnterCar();
            }
        }
        else
        {
            pressE_txt.SetActive(false);

            // Allow exit if inside the car and E is pressed
            if (isInCar && Input.GetKeyDown(KeyCode.E))
            {
                ExitCar();
            }
        }
    }

    /// <summary>
    /// Handles logic for entering the car.
    /// </summary>
    void EnterCar()
    {
        cameraSedan.SetActive(true);

        // Hide player model
        foreach (var renderer in playerMeshRenderers)
        {
            renderer.enabled = false;
        }

        playerMecha.enabled = false; // Disable player controls

        playerCinemachine.SetActive(false);
        inputManagerArcade.enabled = true;
        arcadeVehicleController.enabled = true;
        isInCar = true;
    }

    /// <summary>
    /// Handles logic for exiting the car.
    /// </summary>
    void ExitCar()
    {
        cameraSedan.SetActive(false);
        playerMecha.enabled = true;

        foreach (var renderer in playerMeshRenderers)
        {
            renderer.enabled = true;
        }

        if (playerController != null)
        {
            playerController.enabled = false;
        }

        
        player.transform.position = exitPoint.position;
        player.transform.rotation = exitPoint.rotation;

        if (playerController != null)
        {
            playerController.enabled = true;
        }

        playerCinemachine.SetActive(true);
        inputManagerArcade.enabled = false;
        arcadeVehicleController.enabled = false;
        isInCar = false;
    }
}
