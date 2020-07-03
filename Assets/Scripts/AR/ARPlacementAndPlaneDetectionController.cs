using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class ARPlacementAndPlaneDetectionController : MonoBehaviour
{
    //Inspector
    public GameObject placeButton;
    public GameObject adjustButton;
    public GameObject searchGameButton;
    public GameObject scaleController;
    public TextMeshProUGUI informUIPanel;

    //Private
    ARPlaneManager aRPlaneManager;
    ARPlacementController aRPlacementController;

    private void Awake() {
        aRPlaneManager = GetComponent <ARPlaneManager>();
        aRPlacementController = GetComponent <ARPlacementController>();

    }

    private void Start() {
        placeButton.SetActive(true);
        scaleController.SetActive(true);
        adjustButton.SetActive(false);
        searchGameButton.SetActive(false);
        informUIPanel.text = "Place the Fighting Ring on a detected plane";
    }

    public void EnableARPlacementAndPlaneDetection() {
        aRPlacementController.enabled = true;
        aRPlaneManager.enabled = true;
        SetPlanes(true);
        scaleController.SetActive(true);
        placeButton.SetActive(true);
        adjustButton.SetActive(false);
        searchGameButton.SetActive(false);
        informUIPanel.text = "Place the Fighting Ring on a detected plane";
    }

    public void DisableARPlacementAndPlaneDetection() {
        aRPlacementController.enabled = false;
        aRPlaneManager.enabled = false;
        SetPlanes(false);
        scaleController.SetActive(false);
        placeButton.SetActive(false);
        adjustButton.SetActive(true);
        searchGameButton.SetActive(true);
        informUIPanel.text = "Fighting Ring placed. Ready to find a match!";
    }


    void SetPlanes(bool _value) {
        foreach (var plane in aRPlaneManager.trackables) {
            plane.gameObject.SetActive(_value);
        }
    }
}
