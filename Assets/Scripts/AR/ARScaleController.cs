using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

public class ARScaleController : MonoBehaviour
{

    //Inspector
    public Slider scaleController;

    //Private
    ARSessionOrigin aRSessionOrigin;

    private void Awake() {
        aRSessionOrigin = GetComponent<ARSessionOrigin>();
    }


    void Start()
    {
        scaleController.onValueChanged.AddListener(OnSliderValueChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSliderValueChange(float _value) {
        if (scaleController != null)
            aRSessionOrigin.transform.localScale = Vector3.one / _value;
    }

}
