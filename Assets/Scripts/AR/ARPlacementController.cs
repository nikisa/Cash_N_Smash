using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacementController : MonoBehaviour
{

    //Inspector
    public Camera ARCamera;
    public GameObject arenaReference;

    //Private
    ARRaycastManager aRRaycastManager;
    static List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();


    private void Awake() {
        aRRaycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        Vector3 centerOfScreen = new Vector3(Screen.width / 2, Screen.height / 2);
        Ray ray = ARCamera.ScreenPointToRay(centerOfScreen);


        if (aRRaycastManager.Raycast(ray, raycastHits, TrackableType.PlaneWithinPolygon)) {

            Pose hitPose = raycastHits[0].pose;
            Vector3 placingPosition = hitPose.position;
            arenaReference.transform.position = placingPosition;

        }

    }
}
