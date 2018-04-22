using System;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [System.Serializable]
    public struct Restriction
    {
        public float width;
        public float height;
    }

    private struct DisplaySpace
    {
        public float left;
        public float right;
        public float top;
        public float bottom;
    }

    public float zoomSpeed = 2.0f;
    public float moveSpeed = 0.1f;
    public float minZoomSize = 1.0f;
    public Restriction restriction;

    private Camera cameraComponent;
    private float maxZoomSize;

	// Use this for initialization
	void Start () {
        this.cameraComponent = GetComponent<Camera>();

        var maxZoomHeight = this.restriction.height / 2.0f;
        var maxZoomWidth = this.restriction.width / (this.cameraComponent.aspect * 2.0f);
        this.maxZoomSize = (maxZoomHeight > maxZoomWidth) ? maxZoomWidth : maxZoomHeight;
	}
	
	// Update is called once per frame
	void Update () {
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        var zoomValue = -1 * this.zoomSpeed * scroll;
        this.cameraComponent.orthographicSize += zoomValue;

        var cameraMove = new Vector3(0, 0, 0);
        cameraMove.x -= (Input.GetKey(KeyCode.A)) ? 1 : 0;
        cameraMove.x += (Input.GetKey(KeyCode.D)) ? 1 : 0;
        cameraMove.y -= (Input.GetKey(KeyCode.S)) ? 1 : 0;
        cameraMove.y += (Input.GetKey(KeyCode.W)) ? 1 : 0;
        cameraMove = cameraMove.normalized * this.moveSpeed * this.cameraComponent.orthographicSize;
        this.transform.Translate(cameraMove);

        Clip();
    }

    void Clip()
    {
        if (this.cameraComponent.orthographicSize < this.minZoomSize) this.cameraComponent.orthographicSize = this.minZoomSize;
        if (this.cameraComponent.orthographicSize > this.maxZoomSize) this.cameraComponent.orthographicSize = this.maxZoomSize;

        var currentSpace = calcDisplaySpace();

        var overTop    = Math.Abs(currentSpace.top)    - (this.restriction.height / 2);
        var overBottom = Math.Abs(currentSpace.bottom) - (this.restriction.height / 2);
        var overLeft   = Math.Abs(currentSpace.left)   - (this.restriction.width / 2);
        var overRight  = Math.Abs(currentSpace.right)  - (this.restriction.width / 2);

        if (overTop > 0) this.transform.Translate(new Vector3(0, -1 * overTop, 0));
        else if (overBottom > 0) this.transform.Translate(new Vector3(0, overBottom, 0));

        if (overLeft > 0) this.transform.Translate(new Vector3(overLeft, 0, 0));
        else if (overRight > 0) this.transform.Translate(new Vector3(-1 * overRight, 0, 0));
    }

    DisplaySpace calcDisplaySpace()
    {
        DisplaySpace outSpace;
        var displayHeight = this.cameraComponent.orthographicSize * 2;
        var displayWidth = displayHeight * this.cameraComponent.aspect;

        outSpace.top = this.transform.position.y + displayHeight / 2;
        outSpace.bottom = this.transform.position.y - displayHeight / 2;
        outSpace.left = this.transform.position.x - displayWidth / 2;
        outSpace.right = this.transform.position.x + displayWidth / 2;

        return outSpace;
    }
}
