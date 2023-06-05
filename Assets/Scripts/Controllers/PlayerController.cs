using Assets.Scripts;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float movementSpeed = 1.0f;
	public Vector2 direction = Vector2.zero;
	public bool localFreeze = false;
	public bool freezeInput = false;

	async void Start()
	{
		Manager.Game.Player.Object = this.gameObject;
		movementSpeed = Manager.Game.General.PlayerSpeed;
		await Task.Delay(1000);
		GetComponent<BoxCollider>().enabled = true;
	}

	void OnDestroy()
	{
		var fader = FindFirstObjectByType<EasyObjectsFade>();
		if (fader != null && fader.playerTransform != null && fader.playerTransform.CompareTag("Player"))
			fader.playerTransform = null;
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.transform.CompareTag(Manager.Game.General.WallTag))
		{
			BounceBack(other.transform.position);
			if (Manager.Game.Audio.Controller.soundEnabled)
				Manager.Game.Audio.SoundSource.PlayOneShot(Manager.Game.Audio.Controller.playerHitsWall);
		}
		else if (other.transform.CompareTag(Manager.Game.General.BotTag))
		{
			if (other.transform.localScale.y < this.transform.localScale.y)
			{
				if (Manager.Game.Calculation.Calculation_Destroy(other.transform.localScale.y, this.transform.localScale.y))
				{
					//OTHER DESTROY
					if (Manager.Game.Audio.Controller.soundEnabled)
						Manager.Game.Audio.SoundSource.PlayOneShot(Manager.Game.Audio.Controller.eatCube);
					var botController = other.transform.GetComponent<BotController>();

					Manager.Game.Bots[botController.Index].IsDead = true;
					_ = Manager.Game.Chat.Controller.Text_PlayerDead(Manager.Game.Bots[botController.Index].Name);
					GrowFromPlayer(other.transform);
					Manager.Game.General.PlayerScore += other.transform.localScale.y;
					Manager.Game.General.Counters.UpdateScore();
					Destroy(other.gameObject);
					Manager.Game.General.Counters.UpdatePlayers();
				}
				else
				{
					//OTHER SHRINK
					BounceBack(other.transform.position);
					if (Manager.Game.Audio.Controller.soundEnabled)
						Manager.Game.Audio.SoundSource.PlayOneShot(Manager.Game.Audio.Controller.playerHitsBot);
					var scale = other.transform.localScale.y - ((other.transform.localScale.y / 100) * Manager.Game.Calculation.ShrinkFromPlayer);
					other.transform.localScale = new Vector3(scale, scale, scale);
					other.transform.position = new Vector3(other.transform.position.x, scale / 2, other.transform.position.z);
					Shrink(Manager.Game.Calculation.ShrinkFromCollide);
				}
			}
			else
			{
				// other.transform.localScale.y / Manager.Game.Calculation.DestroyDivider > this.transform.localScale.y)
				if (Manager.Game.Calculation.Calculation_Destroy(this.transform.localScale.y, other.transform.localScale.y))
				{
					//PLAYER DESTROY
					Destroy(Manager.Game.Player.Object);
					if (Manager.Game.Audio.Controller.soundEnabled)
						Manager.Game.Audio.SoundSource.PlayOneShot(Manager.Game.Audio.Controller.meDead);
					Manager.Game.Player.DeadReason = "Your cube has eaten.";
					Manager.Game.General.Controller.PlayerDead();
				}
				else
				{
					//PLAYER SHRINK
					BounceBack(other.transform.position);
					if (Manager.Game.Audio.Controller.soundEnabled)
						Manager.Game.Audio.SoundSource.PlayOneShot(Manager.Game.Audio.Controller.playerHitsBot);
					Shrink(Manager.Game.Calculation.ShrinkFromPlayer);
				}
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (Manager.Game.General.SelectedGameMode == GameMode.BETHEBIGGEST)
			return;

		if (other.transform.CompareTag(Manager.Game.General.FoodTag))
		{
			if (other.transform.localScale.y > transform.localScale.y)
			{
				//FOOD SHRINK & PLAYER BOUNCE
				BounceBack(other.transform.position);
				if (Manager.Game.Audio.Controller.soundEnabled)
					Manager.Game.Audio.SoundSource.PlayOneShot(Manager.Game.Audio.Controller.playerHitsBot);
				var scale = other.transform.localScale.y - ((other.transform.localScale.y / 100) * Manager.Game.Calculation.ShrinkFood);
				other.transform.localScale = new Vector3(scale, scale, scale);
				other.transform.position = new Vector3(other.transform.position.x, scale / 2, other.transform.position.z);
			}
			else
			{
				//FOOD DESTROY
				if (Manager.Game.Audio.Controller.soundEnabled)
					Manager.Game.Audio.SoundSource.PlayOneShot(Manager.Game.Audio.Controller.eatFood);
				var food_controller = other.GetComponent<FoodController>();
				Manager.Game.Foods[food_controller.Index].IsEaten = true;
				Manager.Game.Foods[food_controller.Index].EatenTime = DateTime.Now;
				GrowFromFood(other.transform);
				Manager.Game.General.PlayerScore += other.transform.localScale.y;
				Manager.Game.General.Counters.UpdateScore();
				Destroy(other.gameObject);
				Manager.Game.General.Counters.UpdateFoods();
			}
		}
	}

	private void GrowFromFood(Transform eaten)
	{
		float myScale = gameObject.transform.localScale.y;
		var other_y = eaten.transform.localScale.y;
		myScale += Manager.Game.Calculation.Calculation_GrowFromFood(other_y, myScale);
		gameObject.transform.localScale = new Vector3(myScale, myScale, myScale);
		gameObject.transform.position = new Vector3(transform.position.x, myScale / 2, transform.position.z);
	}
	private void GrowFromPlayer(Transform eaten)
	{
		float myScale = gameObject.transform.localScale.y;
		var other_y = eaten.transform.localScale.y;
		myScale = Manager.Game.Calculation.Calculation_GrowFromPlayer(other_y, myScale);
		gameObject.transform.localScale = new Vector3(myScale, myScale, myScale);
		gameObject.transform.position = new Vector3(transform.position.x, myScale / 2, transform.position.z);
	}

	private void Shrink(float percentage)
	{
		float myScale = gameObject.transform.localScale.y;
		myScale = myScale - ((myScale / 100) * percentage);
		gameObject.transform.localScale = new Vector3(myScale, myScale, myScale);
		gameObject.transform.position = new Vector3(transform.position.x, myScale / 2, transform.position.z);
	}

	float everySecond = 1f;
	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.W) && !freezeInput)
			DirectionUp();
		else if (Input.GetKeyDown(KeyCode.A) && !freezeInput)
		{
			DirectionLeft();
			if (Manager.Game.Player.IsOtoEnabled)
				Manager.Game.Camera.Controller.CompassLeft();
		}
		else if (Input.GetKeyDown(KeyCode.S) && !freezeInput)
		{
			DirectionDown();
			if (Manager.Game.Player.IsOtoEnabled)
				Manager.Game.Camera.Controller.CompassUp();
		}
		else if (Input.GetKeyDown(KeyCode.D) && !freezeInput)
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

		if (Input.GetKeyDown(KeyCode.UpArrow))
			Manager.Game.Camera.Controller.CompassUp();
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
			Manager.Game.Camera.Controller.CompassLeft();
		else if (Input.GetKeyDown(KeyCode.RightArrow))
			Manager.Game.Camera.Controller.CompassRight();

		if (localFreeze || Manager.Game.General.IsPaused)
			return;

		if (Input.GetKeyDown(KeyCode.F12)) //HACK
			Manager.Game.General.Controller.PlayerWin();
		else if(Input.GetKeyDown(KeyCode.F11))  //HACK
		{
			if (Manager.Game.Audio.Controller.soundEnabled)
				Manager.Game.Audio.SoundSource.PlayOneShot(Manager.Game.Audio.Controller.eatCube);
			var bot = Manager.Game.Bots[0];
			foreach (var item in Manager.Game.Bots)
			{
				if(!item.IsDead)
				{
					bot = item;
					break;
				}
			}
			var botController = bot.Object.transform.GetComponent<BotController>();
			Manager.Game.Bots[botController.Index].IsDead = true;
			_ = Manager.Game.Chat.Controller.Text_PlayerDead(Manager.Game.Bots[botController.Index].Name);
			GrowFromPlayer(bot.Object.transform);
			Manager.Game.General.PlayerScore += bot.Object.transform.localScale.y;
			Manager.Game.General.Counters.UpdateScore();
			Destroy(bot.Object.gameObject);
			Manager.Game.General.Counters.UpdatePlayers();
		}

		if (Manager.Game.General.SelectedGameMode == GameMode.BETHEBIGGEST)
		{
			movementSpeed = Manager.Game.General.PlayerSpeed;
			Manager.Game.General.Counters.UpdateScale();
		}
		else
		{
			movementSpeed = Manager.Game.Calculation.Calculation_Set_Speed(Manager.Game.General.PlayerSpeed, transform.localScale.y);
			everySecond -= Time.deltaTime;
			if (everySecond < 0)
			{
				Manager.Game.Calculation.Calculation_Decrease_Scale(transform);
				everySecond = 1;
			}
		}

		if (transform.localScale.y < 0.01f)
		{
			Destroy(Manager.Game.Player.Object);
			Manager.Game.Player.DeadReason = "Your cube has shattered for being too small.";
			if (Manager.Game.Audio.Controller.soundEnabled)
				Manager.Game.Audio.SoundSource.PlayOneShot(Manager.Game.Audio.Controller.meDead);
			Manager.Game.General.Controller.PlayerDead();
			return;
		}

		float gposX = gameObject.transform.position.x;
		float gposY = gameObject.transform.position.y;
		float gposZ = gameObject.transform.position.z;

		if (direction.x > 0)
		{ gameObject.transform.position = new Vector3(gposX + (Time.deltaTime * movementSpeed), gposY, gposZ); }
		else if (direction.x < 0)
		{ gameObject.transform.position = new Vector3(gposX - (Time.deltaTime * movementSpeed), gposY, gposZ); }
		else if (direction.y > 0)
		{ gameObject.transform.position = new Vector3(gposX, gposY, gposZ + (Time.deltaTime * movementSpeed)); }
		else if (direction.y < 0)
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

	public async void BounceBack(Vector3 target)
	{
		freezeInput = true;
		if (direction == Vector2.up)
			direction = Vector2.down;
		else if (direction == Vector2.down)
			direction = Vector2.up;
		else if (direction == Vector2.right)
			direction = Vector2.left;
		else if (direction == Vector2.left)
			direction = Vector2.right;
		await Task.Delay(250);
		freezeInput = false;
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


