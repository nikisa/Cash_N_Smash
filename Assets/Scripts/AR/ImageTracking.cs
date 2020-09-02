using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;


[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracking : MonoBehaviour
{

    //Inspector
    public CharacterSelection characterSelection;
    [SerializeField]
    private GameObject[] placeablePrefabs;

    //Private
    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();
    private ARTrackedImageManager trackedImageManager;

    private void Awake() {

        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        foreach (GameObject prefab in placeablePrefabs) {
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, prefab.transform.rotation);
            newPrefab.name = prefab.name;
            spawnedPrefabs.Add(prefab.name, newPrefab);
        }
    }

    private void OnEnable() {
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }

    private void OnDisable() {
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    }

    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs) {
        foreach (ARTrackedImage trackedImage in eventArgs.added) {
            UpdateImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated) {
            UpdateImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed) {
            spawnedPrefabs[trackedImage.name].SetActive(false);
        }
    }


    private void UpdateImage(ARTrackedImage trackedImage) {
        string name = trackedImage.referenceImage.name;
        Vector3 position = trackedImage.transform.position;

        GameObject prefab = spawnedPrefabs[name];
        prefab.transform.position = position;
        prefab.SetActive(true);
        characterSelection.selectedCharacter = (int) prefab.GetComponent<PlayerController>().characterID;
        characterSelection.uiReset.SetActive(true);
        characterSelection.uiFight.SetActive(true);
        characterSelection.uiInfo.SetActive(false);
        characterSelection.actualCharacter = prefab;

        foreach (GameObject go in spawnedPrefabs.Values) {
            if (go.name != name) {
                go.SetActive(false);
            }
        }
    }


}
