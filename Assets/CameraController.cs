using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float zoomSpeed = 2.0f;

    private Camera cameraComponent;

	// Use this for initialization
	void Start () {
        this.cameraComponent = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        this.cameraComponent.orthographicSize -= scroll * this.zoomSpeed;
	}
}
