using Assets.Scripts;
using Assets.Scripts.Models;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float movementSpeed = 1.0f;
	public Vector2 direction = Vector2.zero;

	[Header("SFX")]
	public AudioClip wallHitSound;
	public AudioClip botHitSound;

	private float inputX = 0;
	private float inputY = 0;

	// Start is called before the first frame update
	void Start()
	{
		FindFirstObjectByType<EasyObjectsFade>().playerTransform = transform;
		StartAsync();
	}

	async void StartAsync()
	{
		await Task.Delay(50);
		Manager.Game.Player.Object = this.gameObject;
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.transform.CompareTag(Manager.Game.General.WallTag))
		{
			BounceBack(other.transform.position);
			Manager.Game.Sound.PlayQuickSound(wallHitSound);
		} else if(other.transform.CompareTag(Manager.Game.General.BotTag))
		{
			if(other.transform.localScale.y < this.transform.localScale.y)
			{
				if (other.transform.localScale.y < this.transform.localScale.y / 2)
				{
					//OTHER DEAD
				}
				else
				{

					//OTHER SCALE DOWN
				}
			}
			else
			{
				if(other.transform.localScale.y >  this.transform.localScale.y * 2)
				{
					//PLAYER IS DEAD
					FindFirstObjectByType<EasyObjectsFade>().playerTransform = null;
					GameObject.Destroy(Manager.Game.Player.Object);
					Manager.Game.Camera.Controller.LookAtTheGround();
					Manager.Game.Chat.Controller.Text_Line("Your cube has eaten. Try again!");
				} else
				{
					//PLAYER BOUNCE BACK
					BounceBack(other.transform.position);
					Manager.Game.Sound.PlayQuickSound(botHitSound);
				}
			}
			
		}
	}

	void OnTriggerEnter(Collider other)
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.W))
			DirectionUp();
		else if (Input.GetKeyDown(KeyCode.A))
		{
			DirectionLeft();
			if (Manager.Game.Player.IsOtoEnabled)
				Manager.Game.Camera.Controller.CompassLeft();
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			DirectionDown();
			if (Manager.Game.Player.IsOtoEnabled)
				Manager.Game.Camera.Controller.CompassUp();
		}
		else if (Input.GetKeyDown(KeyCode.D))
		{
			DirectionRight();
			if (Manager.Game.Player.IsOtoEnabled)
				Manager.Game.Camera.Controller.CompassRight();
		}
		else if (Input.GetKeyDown(KeyCode.Space))
			direction = Vector2.zero;
		else if (Input.GetKeyDown(KeyCode.O))
			Manager.Game.General.ToggleOto();
		else if (Input.GetKeyDown(KeyCode.Escape))
			Manager.Game.General.TogglePause();
		else if (Input.GetKeyDown(KeyCode.C))
			Manager.Game.Chat.Controller.ClearTextOne();

		if (Input.GetKeyDown(KeyCode.UpArrow))
			Manager.Game.Camera.Controller.CompassUp();
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
			Manager.Game.Camera.Controller.CompassLeft();
		else if (Input.GetKeyDown(KeyCode.RightArrow))
			Manager.Game.Camera.Controller.CompassRight();

		inputX = direction.x;
		inputY = direction.y;

		float gposX = gameObject.transform.position.x;
		float gposY = gameObject.transform.position.y;
		float gposZ = gameObject.transform.position.z;

		if (Manager.Game.General.IsPaused)
			return;

		if (inputX > 0)
		{ gameObject.transform.position = new Vector3(gposX + (Time.deltaTime * movementSpeed), gposY, gposZ); }
		else if (inputX < 0)
		{ gameObject.transform.position = new Vector3(gposX - (Time.deltaTime * movementSpeed), gposY, gposZ); }
		else if (inputY > 0)
		{ gameObject.transform.position = new Vector3(gposX, gposY, gposZ + (Time.deltaTime * movementSpeed)); }
		else if (inputY < 0)
		{ gameObject.transform.position = new Vector3(gposX, gposY, gposZ - (Time.deltaTime * movementSpeed)); }

	}
	public void DirectionUp()
	{
		if (Helper.AsXZRawReversed(Manager.Game.Camera.Controller.offSet) == Vector2.up)
			direction = Vector2.up;
		else if (Helper.AsXZRawReversed(Manager.Game.Camera.Controller.offSet) == Vector2.right)
			direction = Vector2.right;
		else if (Helper.AsXZRawReversed(Manager.Game.Camera.Controller.offSet) == Vector2.down)
			direction = Vector2.down;
		else if (Helper.AsXZRawReversed(Manager.Game.Camera.Controller.offSet) == Vector2.left)
			direction = Vector2.left;
	}

	public void DirectionDown()
	{
		if (Helper.AsXZRawReversed(Manager.Game.Camera.Controller.offSet) == Vector2.up)
			direction = Vector2.down;
		else if (Helper.AsXZRawReversed(Manager.Game.Camera.Controller.offSet) == Vector2.right)
			direction = Vector2.left;
		else if (Helper.AsXZRawReversed(Manager.Game.Camera.Controller.offSet) == Vector2.down)
			direction = Vector2.up;
		else if (Helper.AsXZRawReversed(Manager.Game.Camera.Controller.offSet) == Vector2.left)
			direction = Vector2.right;
	}

	public void DirectionRight()
	{
		if (Helper.AsXZRawReversed(Manager.Game.Camera.Controller.offSet) == Vector2.up)
			direction = Vector2.right;
		else if (Helper.AsXZRawReversed(Manager.Game.Camera.Controller.offSet) == Vector2.right)
			direction = Vector2.down;
		else if (Helper.AsXZRawReversed(Manager.Game.Camera.Controller.offSet) == Vector2.down)
			direction = Vector2.left;
		else if (Helper.AsXZRawReversed(Manager.Game.Camera.Controller.offSet) == Vector2.left)
			direction = Vector2.up;
	}

	public void DirectionLeft()
	{
		if (Helper.AsXZRawReversed(Manager.Game.Camera.Controller.offSet) == Vector2.up)
			direction = Vector2.left;
		else if (Helper.AsXZRawReversed(Manager.Game.Camera.Controller.offSet) == Vector2.left)
			direction = Vector2.down;
		else if (Helper.AsXZRawReversed(Manager.Game.Camera.Controller.offSet) == Vector2.down)
			direction = Vector2.right;
		else if (Helper.AsXZRawReversed(Manager.Game.Camera.Controller.offSet) == Vector2.right)
			direction = Vector2.up;
	}

	public void BounceBack(Vector3 target)
	{
		if (direction == Vector2.up)
			direction = Vector2.down;
		else if (direction == Vector2.down)
			direction = Vector2.up;
		else if (direction == Vector2.right)
			direction = Vector2.left;
		else if (direction == Vector2.left)
			direction = Vector2.right;
	}

	public void SetCompassFromHere(KeyCode keyCode)
	{
		if (keyCode == (KeyCode.UpArrow))
			Manager.Game.Camera.Controller.CompassUp();
		else if (keyCode == (KeyCode.LeftArrow))
			Manager.Game.Camera.Controller.CompassLeft();
		else if (keyCode == (KeyCode.RightArrow))
			Manager.Game.Camera.Controller.CompassRight();
		else if (keyCode == (KeyCode.R))
			Manager.Game.Camera.Controller.CompassReset();
	}

	public void SetDirectionFromHere(KeyCode keyCode)
	{
		if (keyCode == (KeyCode.W))
		{
			DirectionUp();
		}
		else if (keyCode == (KeyCode.A))
		{
			DirectionLeft();
			if (Manager.Game.Player.IsOtoEnabled)
				Manager.Game.Camera.Controller.CompassLeft();
		}
		else if (keyCode == (KeyCode.S))
		{
			DirectionDown();
			if (Manager.Game.Player.IsOtoEnabled)
				Manager.Game.Camera.Controller.CompassUp();
		}
		else if (keyCode == (KeyCode.D))
		{
			DirectionRight();
			if (Manager.Game.Player.IsOtoEnabled)
				Manager.Game.Camera.Controller.CompassRight();
		}
		else if (keyCode == (KeyCode.Space))
			direction = Vector2.zero;

	}

}


