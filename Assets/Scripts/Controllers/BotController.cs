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
}
