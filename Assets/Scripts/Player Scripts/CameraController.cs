using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	//The target for the camera to focus on
	public Transform target;
	//Camera offset
	public Vector3 offset;
	//Mouse wheel zoom speed
	public float zoomSpeed = 4f;
	//Minimum Zoom distance
	public float minZoom = 5f;
	//Maximum Zoom distance
	public float maxZoom = 15f;
	//Current Zoom
	private float currentZoom = 10f;
	//Camera Pitch
	public float pitch = 2;
	
	//Yaw Speed
	//public float yawSpeed = 100f;
	//Yaw Input
	//private float currentYaw = 0f;


	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
	{
		//Set the current zoom to - the input from the mouse wheel, multiplied by the zoomspeed
		currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
		//Clamp the current zoom based on the current zoom value and the min and max zoom value
		currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

		//HACK:(DO NOT USE, Experimental)
		//check for input on the horizontal axis (A or D - Left arrow or Right Arrow)
		//currentYaw = Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime;

	}
	void LateUpdate()
	{
		//Set the current camera transform to the targets minus the offset value multiplied by the current zoom
		transform.position = target.position - offset * currentZoom;
		//set the transform to look at the targets world position + Vector3UP multiplied by the pitch
		transform.LookAt(target.position + Vector3.up * pitch);

		//HACK:(Experimental)
		//rotate the camera after the button press
		//transform.RotateAround(target.position, Vector3.up,currentYaw);
	}
}
