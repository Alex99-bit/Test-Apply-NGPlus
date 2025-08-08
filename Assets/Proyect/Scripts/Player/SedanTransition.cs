using ArcadeVP;
using UnityEngine;

public class SedanTransition : MonoBehaviour
{
    [Header("References")]
    public GameObject cameraSedan;
    public GameObject player; // Referencia al jugador
    public SkinnedMeshRenderer[] playerMeshRenderers; // Para ocultar el modelo del jugador
    public GameObject playerCinemachine; // Referencia al jugador
    public InputManager_ArcadeVP inputManagerArcade;
    public ArcadeVehicleController arcadeVehicleController;

    [Header("Interaction")]
    public float interactionDistance = 2f; // Distancia para entrar
    public Transform exitPoint; // Punto exacto donde aparecerá el jugador al salir

    private bool isInCar = false;

    void Start()
    {
        cameraSedan.SetActive(false);
        foreach (var renderer in playerMeshRenderers)
        {
            renderer.enabled = true; // Asegurarse de que el modelo del jugador esté visible al inicio
        }
        playerCinemachine.SetActive(true);

        inputManagerArcade.enabled = false;
        arcadeVehicleController.enabled = false;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Solo permitir entrar si está cerca y no está dentro
        if (!isInCar && distanceToPlayer <= interactionDistance && Input.GetKeyDown(KeyCode.E))
        {
            EnterCar();
        }
        // Permitir salir si está dentro
        else if (isInCar && Input.GetKeyDown(KeyCode.E))
        {
            ExitCar();
        }
    }

    void EnterCar()
    {
        cameraSedan.SetActive(true);
        foreach (var renderer in playerMeshRenderers)
        {
            renderer.enabled = false; // Ocultar el modelo del jugador
        }
        playerCinemachine.SetActive(false);
        inputManagerArcade.enabled = true;
        arcadeVehicleController.enabled = true;
        isInCar = true;
    }

    void ExitCar()
    {
        cameraSedan.SetActive(false);
        foreach (var renderer in playerMeshRenderers)
        {
            renderer.enabled = true; // Mostrar el modelo del jugador
        }
        playerCinemachine.SetActive(true);

        inputManagerArcade.enabled = false;
        arcadeVehicleController.enabled = false;
        isInCar = false;

        // Colocar al jugador junto al coche
        if (exitPoint != null)
        {
            player.transform.position = exitPoint.position;
            player.transform.rotation = exitPoint.rotation;
        }
        else
        {
            // Si no hay exitPoint definido, usar lado derecho del coche
            Vector3 exitOffset = transform.right * 2f;
            player.transform.position = transform.position + exitOffset;
            player.transform.LookAt(transform.position);
        }
    }
}
