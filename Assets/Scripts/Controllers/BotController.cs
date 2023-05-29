using NUnit.Framework;
using UnityEngine;
using UnityEngine.Windows;

public class BotController : MonoBehaviour
{
	public int botIndex = 0;
	public float movementSpeed = 1.0f;
	public bool localFreeze = true;
	public Vector2 direction = Vector2.zero;
	private float inputX = 0;
	private float inputY = 0;

	private void StartBot()
	{
		
		localFreeze = false;
	}

	private void Update()
	{
		if (localFreeze)
			return;

		inputX = direction.x;
		inputY = direction.y;

		if (Manager.Game.General.IsPaused)
			return;

		float gposX = gameObject.transform.position.x;
		float gposY = gameObject.transform.position.y;
		float gposZ = gameObject.transform.position.z;

		if (inputX > 0)
		{ gameObject.transform.position = new Vector3(gposX + (Time.deltaTime * movementSpeed), gposY, gposZ); }
		else if (inputX < 0)
		{ gameObject.transform.position = new Vector3(gposX - (Time.deltaTime * movementSpeed), gposY, gposZ); }
		else if (inputY > 0)
		{ gameObject.transform.position = new Vector3(gposX, gposY, gposZ + (Time.deltaTime * movementSpeed)); }
		else if (inputY < 0)
		{ gameObject.transform.position = new Vector3(gposX, gposY, gposZ - (Time.deltaTime * movementSpeed)); }

	}

	private void OnCollisionEnter(Collision other)
	{
		Debug.Log("OnCollisionEnter: " + other.transform.tag);
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("OnTriggerEnter: " + other.transform.tag);
	}
}
