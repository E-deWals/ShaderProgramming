using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ChaseTarget : MonoBehaviour
{
	public Transform target;
	public float visionAngle=45;
	public float chaseRange=5;
	public float turnSpeedDegreesSecond;
	public float moveSpeedPerSecond;
	public float currentAngleDegrees;
	public MeshRenderer eye;


    void Start()
    {
		SetEyeColor(true);
	}

	void SetEyeColor(bool enabled) {
		if (enabled) {
			eye.material.EnableKeyword("_EMISSION");
		} else {
			eye.material.DisableKeyword("_EMISSION");
		}
	}


	void Update()
    {
		// TODO: 
		//  - Make the enemy chase the player (target)
		//  - Make the enemy (slowly) rotate towards the player
		//  - Only do this when the player is in range (max distance) and in sight (max angle)
		//  - Draw rays in the scene view to indicate the vision range.
		// See the slides for details!
		// (You can use SetEyeColor for visual debugging)

		Vector3 difference =  target.position - transform.position;
		difference.y = 0;
		float rad2deg = 360 / (2 * MathF.PI);

        float angle = Mathf.Atan2(difference.z, difference.x) * rad2deg;
		Vector3 currentForward = new Vector3((MathF.Cos(currentAngleDegrees / rad2deg)), 0f, MathF.Sin(currentAngleDegrees / rad2deg));


		float currentAngle = Mathf.Atan2(transform.rotation.z, transform.rotation.x);
		float length = difference.magnitude;

		if (length < chaseRange && angle < visionAngle)
		{
			//transform.position += direction * (moveSpeedPerSecond / length) * Time.deltaTime;

			float x = MathF.Cos(angle) * length;
			float z = MathF.Sin(angle) * length;
			Vector3 desiredRotation = new Vector3(x, 0, z);

			float currentX = Mathf.Cos(currentAngle) * length;
			float currentZ = Mathf.Sin(currentAngle) * length;
			Vector3 currentRotation = new Vector3(currentX, 0, currentZ);

			transform.forward = (desiredRotation);
			//Quaternion rotation = Quaternion.Euler(x, 0, z);
			//transform.rotation = rotation;
		}
		
	}
}
