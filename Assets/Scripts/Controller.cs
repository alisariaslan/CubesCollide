using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
	public float movementSpeed = 1.0f;
	private GameObject Player;
	// Start is called before the first frame update
	void Start()
	{
		Player = this.gameObject;
	}

	// Update is called once per frame
	void Update()
	{
		float inputX = Input.GetAxisRaw("Horizontal");
		float inputY = Input.GetAxisRaw("Vertical");
		float gposX = gameObject.transform.position.x;
		float gposY = gameObject.transform.position.y;
		float gposZ = gameObject.transform.position.z;
		if (inputX > 0)
		{ Player.transform.position = new Vector3(gposX + (Time.deltaTime * movementSpeed), gposY, gposZ); }
		if (inputX < 0)
		{ Player.transform.position = new Vector3(gposX - (Time.deltaTime * movementSpeed), gposY, gposZ); }
		if (inputY > 0)
		{ Player.transform.position = new Vector3(gposX, gposY, gposZ + (Time.deltaTime * movementSpeed)); }
		if (inputY < 0)
		{ Player.transform.position = new Vector3(gposX, gposY, gposZ - (Time.deltaTime * movementSpeed)); }
		if (inputX > 0 && inputY > 0)
		{ Player.transform.position = new Vector3(gposX + (Time.deltaTime * movementSpeed) / 1.25f, gposY, gposZ + (Time.deltaTime * movementSpeed) / 1.25f); }
		if (inputX > 0 && inputY < 0)
		{ Player.transform.position = new Vector3(gposX + (Time.deltaTime * movementSpeed) / 1.25f, gposY, gposZ - (Time.deltaTime * movementSpeed) / 1.25f); }
		if (inputX < 0 && inputY <  0)
		{ Player.transform.position = new Vector3(gposX - (Time.deltaTime * movementSpeed) / 1.25f, gposY, gposZ - (Time.deltaTime * movementSpeed) / 1.25f); }
		if (inputX < 0 && inputY > 0)
		{ Player.transform.position = new Vector3(gposX - (Time.deltaTime * movementSpeed) / 1.25f, gposY, gposZ + (Time.deltaTime * movementSpeed) / 1.25f); }

	}
}
