using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour {

	public float translate_factor = 0.3f;
	public float rotate_factor = 5.0f;
	public bool third_person = true;

	// Move the camera, and maybe create a new plane
	void Update () {
		float dx = Input.GetAxis("Horizontal");
		float dz = Input.GetAxis("Vertical");

		if (Input.GetKeyUp(KeyCode.T))
        {
			if (third_person)
            {
				transform.position = Vector3.up;
				transform.rotation = Quaternion.identity;
				third_person = false;
            } 
			else
            {
				transform.position = 35 * Vector3.up;
				transform.rotation = Quaternion.Euler(90, 0, 0);
				third_person = true;
			}
        }
		
		if (third_person)
        {
			if (Camera.current != null)
			{
				// translate forward or backwards
				Camera.current.transform.Translate(0, 0, dz * translate_factor);

			}
		} 
		else
        {
			// move the camera based on the keyboard input
			if (Camera.current != null)
			{
				// translate forward or backwards
				Camera.current.transform.Translate(0, 0, dz * translate_factor);

				// rotate left or right
				Camera.current.transform.Rotate(0, dx * rotate_factor, 0);

			}
		}
		

	}

}
