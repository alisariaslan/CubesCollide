using UnityEngine;

public class BotController : MonoBehaviour
{
	public float movementSpeed = 1.0f;
	public bool freeze = true;
	// Start is called before the first frame update
	private void Start()
	{
		
	}

	private void Update()
	{
		if (freeze)
			return;

		//float inputX = 0f;
		//float inputY = 0f;
		float gposX = gameObject.transform.position.x;
		float gposY = gameObject.transform.position.y;
		float gposZ = gameObject.transform.position.z;

	}

	private void OnCollisionEnter(Collision collision)
	{
		Debug.Log("Bot Collision! " + collision.transform.tag);
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Bot Trigger! " + other.transform.tag);
	}
}
