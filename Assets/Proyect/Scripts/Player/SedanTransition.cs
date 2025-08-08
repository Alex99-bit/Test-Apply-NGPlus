using ArcadeVP;
using UnityEngine;

public class SedanTransition : MonoBehaviour
{
    public GameObject cameraSedan;
    public GameObject player;
    public InputManager_ArcadeVP inputManagerArcade;
    public ArcadeVehicleController arcadeVehicleController;

    private bool isInCar = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        cameraSedan.SetActive(false);
        player.SetActive(true);

        inputManagerArcade.enabled = false;
        arcadeVehicleController.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionStay(Collision collision)
    {
        // Shows the button to enter or exit the sedan
        if (collision.gameObject.CompareTag("Sedan"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isInCar)
                {
                    // Enter the car
                    cameraSedan.SetActive(true);
                    player.SetActive(false);
                    inputManagerArcade.enabled = true;
                    arcadeVehicleController.enabled = true;
                    isInCar = true;
                }
                else
                {
                    // Exit the car: place player next to the sedan
                    cameraSedan.SetActive(false);
                    player.SetActive(true);
                    inputManagerArcade.enabled = false;
                    arcadeVehicleController.enabled = false;
                    isInCar = false;

                    // Place player next to the car (e.g., right side)
                    Vector3 exitOffset = collision.transform.right * 2f; // 2 units to the right
                    player.transform.position = collision.transform.position + exitOffset;
                }
            }
        }
    }
}
