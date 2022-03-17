using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class MouseOrbitImproved : MonoBehaviour {
	
	public Transform target;
	public float distance = 5.0f;
	public float xSpeed = 120.0f;
	public float ySpeed = 120.0f;
	public float scrollrate = 3.0f;
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;
	
	public float distanceMin = .5f;
	public float distanceMax = 15f;
	public Vector3 Offset;


	public bool Clip;
	public string TargetBone;

	private Rigidbody _rigidbody;
	private GameObject TargetGO;
	
	public float x = 0.0f;
	float y = 0.0f;
	
	// Use this for initialization
	void Start () 
	{
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
		
		_rigidbody = GetComponent<Rigidbody>();
		
		// Make the rigid body not change rotation
		if (_rigidbody != null)
		{
			_rigidbody.freezeRotation = true;
		}
	}


	private Vector3 GetTarget()
	{
		Transform t = target;

		if (!string.IsNullOrEmpty (TargetBone)) 
		{
			t = target.Find (TargetBone);
			if (t == null) {
				t = target;
			}
		}

		Vector3 dest = t.position + Offset;
		return dest;
	}

	void LateUpdate () 
	{
		Vector3 tgt = GetTarget();


			if (Input.GetMouseButton(0))
			{
				x += Input.GetAxis("Mouse X") * xSpeed/* * distance */ * 0.04f;
				y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
			}

			y = ClampAngle(y, yMinLimit, yMaxLimit);

			
			Quaternion rotation = Quaternion.Euler(y, x, 0);
			
			distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel")*scrollrate, distanceMin, distanceMax);

			if (Clip) 
			{
				RaycastHit hit;
				if (Physics.Linecast (tgt, transform.position, out hit)) 
				{
					distance -=  hit.distance;
				}
			}
			Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
			Vector3 position = rotation * negDistance + tgt;
			
			transform.rotation = rotation;
			transform.position = position;
	}
	
	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
}