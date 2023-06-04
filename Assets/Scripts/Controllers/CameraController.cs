using System.Threading.Tasks;
using UnityEngine;

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
	public Compass Direction_ { get; set; } = Compass.North;
	public Compass Direction
	{
		get { return Direction_; }
		set
		{
			if (value == Compass.North)
				offSet = north;
			else if (value == Compass.South)
				offSet = south;
			else if (value == Compass.West)
				offSet = west;
			else if (value == Compass.East)
				offSet = east;
			Direction_ = value;
		}
	}

	async void Start()
	{
		await Task.Delay(10);
		Manager.Game.Camera.Object = this.gameObject;
		Manager.Game.Camera.SpawnPosition = transform.position;
		Manager.Game.Camera.SpawnRotation = transform.rotation;
		Manager.Game.Camera.SpawnOffset = GetComponent<CameraController>().offSet;
	}
	public void LookAtThePlayer()
	{
		target = Manager.Game.Player.Object; if (target == null)
		{
			Debug.Log("PLAYER IS NULL!");
			return;
		}
		GetComponent<EasyObjectsFade>().playerTransform = target.transform;
		//LOOK TO CENTER OF MAP
		if (target.transform.position.z > 0)
			Direction = Compass.South;
		else Direction = Compass.North;
	}

	public void CalculateChanges()
	{
		float x = 0;
		float y = 0;
		if (target.transform.localScale.y < .1f)
		{
			y = target.transform.localScale.y * 10f;
			smoothPositionTime = 0.3f;
		}
		else if (target.transform.localScale.y < 1)
		{
			y = target.transform.localScale.y * 3f;
			smoothPositionTime = 0.13f;
		}
		else
		{
			y = target.transform.localScale.y * 1.5f;
			smoothPositionTime = 0.3f;
		}
		float z = y * -1.5f;
		north = new Vector3(x, y, z);
		west = new Vector3(z, y, x);
		south = new Vector3(x, y, -z);
		east = new Vector3(-z, y, x);
		switch (Direction)
		{
			case Compass.North:
				Direction = Compass.North;
				break;
			case Compass.West:
				Direction = Compass.West;
				break;
			case Compass.East:
				Direction = Compass.East;
				break;
			case Compass.South:
				Direction = Compass.South;
				break;
		}
	}

	public void LookAtTheGround()
	{
		FindFirstObjectByType<EasyObjectsFade>().playerTransform = null;
		var ground = Manager.Game.Ground.Object;
		if (ground == null)
			return;
		var scale = Manager.Game.Ground.Size;
		target = ground;
		Direction = Compass.North;
		offSet = new Vector3(scale * 2.5f, scale * 2.5f, scale * 5f);
	}

	public void LookAtTheDancers()
	{
		FindFirstObjectByType<EasyObjectsFade>().playerTransform = null;
		target = null;
		transform.position = Manager.Game.Camera.SpawnPosition;
		transform.rotation = Manager.Game.Camera.SpawnRotation;
		Direction = Compass.North;
		offSet = Manager.Game.Camera.SpawnOffset;
	}

	public void CompassRight()
	{
		if (Direction == Compass.North)
			Direction = Compass.West;
		else if (Direction == Compass.West)
			Direction = Compass.South;
		else if (Direction == Compass.South)
			Direction = Compass.East;
		else if (Direction == Compass.East)
			Direction = Compass.North;

	}

	public void CompassLeft()
	{
		if (Direction == Compass.North)
			Direction = Compass.East;
		else if (Direction == Compass.East)
			Direction = Compass.South;
		else if (Direction == Compass.South)
			Direction = Compass.West;
		else if (Direction == Compass.West)
			Direction = Compass.North;
	}

	public void CompassUp()
	{
		if (Direction == Compass.East)
			Direction = Compass.West;
		else if (Direction == Compass.West)
			Direction = Compass.East;
		else if (Direction == Compass.North)
			Direction = Compass.South;
		else if (Direction == Compass.South)
			Direction = Compass.North;
	}

	public void CompassReset()
	{
		Direction = Compass.North;
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

		if (target.CompareTag("Player"))
			CalculateChanges();
	}

}


