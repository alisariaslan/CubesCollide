using Assets.Scripts.Models;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
	public Vector3 offSet;
	public float smoothPositionTime = 0.3f;
	public float smoothRotationTime = 10f;
	public float rotationY = 0f;
	private GameObject target;
	private Vector3 velocity = Vector3.zero;
	private Vector3 north;
	private Vector3 west;
	private Vector3 east;
	private Vector3 south;

	private void Start()
	{
		Manager.Game.Camera.Object = this.gameObject;
		Manager.Game.Camera.SpawnPosition = transform.position;
		Manager.Game.Camera.SpawnRotation = transform.rotation;
		Manager.Game.Camera.SpawnOffset = offSet;
	}

	public void LookAtThePlayer(bool lookCenterToMap)
	{
		var player = Manager.Game.Player.Object;

		if (player == null)
		{
			Debug.Log("PLAYER IS NULL!");
			return;
		}

		target = player;
		offSet = Manager.Game.Camera.SpawnPosition - target.transform.position;
		var x = offSet.x;
		var y = offSet.y;
		var z = offSet.z;

		x = 0;
		y /= 2;
		z = -5;
		offSet = new Vector3(x, y, z);
		north = offSet;
		west = new Vector3(-5, y, 0);
		south = new Vector3(0, y, 5);
		east = new Vector3(5, y, 0);

		if (lookCenterToMap) //ONLY VERTICAL SOLUTIONS
		{
			var playerPos = Manager.Game.Player.Object.transform.position;
			if (playerPos.z > 0)
				offSet = south;
			else offSet = north;
		}


	}

	public void LookAtTheGround()
	{
		var ground = Manager.Game.Ground.Object;
		if (ground == null)
			return;

		var scale = Manager.Game.Ground.Size;
		target = ground;
		offSet = new Vector3(scale * 2.5f, scale * 2.5f, scale * 5f);
	}

	public void LookAtTheDancers()
	{
		target = null;
		transform.position = Manager.Game.Camera.SpawnPosition;
		transform.rotation = Manager.Game.Camera.SpawnRotation;
		offSet = Manager.Game.Camera.SpawnOffset;
	}

	public void CompassRight()
	{
		if (offSet == north)
			offSet = west;
		else if (offSet == west)
			offSet = south;
		else if (offSet == south)
			offSet = east;
		else if (offSet == east)
			offSet = north;
	}

	public void CompassLeft()
	{
		if (offSet == north)
			offSet = east;
		else if (offSet == east)
			offSet = south;
		else if (offSet == south)
			offSet = west;
		else if (offSet == west)
			offSet = north;
	}

	public void CompassUp()
	{
		if (offSet == east)
			offSet = west;
		else if (offSet == west)
			offSet = east;
		else if (offSet == north)
			offSet = south;
		else if (offSet == south)
			offSet = north;
	}

	public void CompassReset()
	{
		offSet = north;
	}

	private void LateUpdate()
	{
		if (offSet == null || target == null)
			return;

		Vector3 targetPosition = target.transform.position + offSet;
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothPositionTime);
		Vector3 targetRotation = target.transform.position - transform.position;

		if (rotationY != 0f)
			targetRotation.y = rotationY;

		transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetRotation), Time.time * smoothRotationTime);
	}

}


