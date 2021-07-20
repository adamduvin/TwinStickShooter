using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldspaceUI : MonoBehaviour
{
    Camera mainCamera;
    [SerializeField]
    private GameObject cameraParent;
    Quaternion originalRotation;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        originalRotation = mainCamera.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = mainCamera.transform.rotation;
    }
}
