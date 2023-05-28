using Assets.Scripts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Manager : MonoBehaviour
{
	public static Game Game { get; set; }

	[Header("Game Objects")]
	public GameObject mainCam;
	public GameObject extendedUI;
	public GameObject pauseUI;
	public List<GameObject> beforePauseUI;

	[Header("General Settings")]
	public bool randomizeAfterStart = true;

	[Header("Player Settings")]
	public bool playerOtoEnabledStart = true;

	[Header("Spawn Settings")]
	public bool spawnMyPlayer = true;
	public bool spawnBots = true;
	public bool spawnFoods = true;

	[Header("Object Settings")]
	public float maxObjectSize = 30;

	[Header("Device Settings")]
	public bool isMobileDevice = false;

	private void Start()
	{
		Game = new Game();
		if (Application.isMobilePlatform || isMobileDevice)
			Game.General.IsMobileDevice = true;
		Game.Player.IsOtoEnabled = playerOtoEnabledStart;
		Game.General.Menu = GameObject.Find("Menu");
	}

	public async void StartGame()
	{
		Cursor.visible = false;
		Manager.Game.General.Menu.GetComponent<Animator>().Play("MenuUp");
		Manager.Game.Sound.Controller.SwitchToIngameMusic();
		ToggleExtendedUI();
		Randomizer my_randomizer = FindFirstObjectByType<Randomizer>();
		if (randomizeAfterStart)
		{
			await my_randomizer.RandomizeGround();
			Game.Camera.Controller.LookAtTheGround();
			await my_randomizer.CalculateAvaibleness();
			await my_randomizer.CalculatePoints();
			await my_randomizer.GenerateFoods();
			await my_randomizer.GenerateBots();
			await my_randomizer.SpawnMyPlayer();
		}
		await Task.Delay(1000); //Waiting for Start Funcs
		Game.Camera.Controller.LookAtThePlayer(true);
		ToggleUI();
		await Task.Delay(100);
		Manager.Game.General.OtoButton.GetComponent<ButtonPressed>().CheckOto();
		await Task.Delay(100);
		Game.Chat.Controller.AnnounceBots();
	}

	public void StopGame()
	{
		TogglePause();
		ToggleUI();
		ToggleExtendedUI();
		Manager.Game.General.Menu.GetComponent<Animator>().Play("MenuDown");
		Manager.Game.Sound.Controller.SwitchToMenuMusic();
		Game.Camera.Controller.LookAtTheDancers();
		foreach (var item in Manager.Game.Foods)
			GameObject.Destroy(item.Object);
		foreach (var item in Manager.Game.Bots)
			GameObject.Destroy(item.Object);
		FindFirstObjectByType<EasyObjectsFade>().playerTransform = null;
		GameObject.Destroy(Manager.Game.Ground.Object);
		GameObject.Destroy(Manager.Game.Player.Object);
		Cursor.visible = true;
	}

	public void TogglePause()
	{
		Manager.Game.Sound.Controller.ToggleMusic();
		var paused = Manager.Game.General.IsPaused;
		if (paused)
		{
			Cursor.visible = false;
			paused = false;
			pauseUI.SetActive(false);
			foreach (var item in beforePauseUI)
				item.SetActive(true);
		}
		else
		{
			Cursor.visible = true;
			paused = true;
			pauseUI.SetActive(true);
			foreach (var item in beforePauseUI)
				item.SetActive(false);
		}
		Manager.Game.General.IsPaused = paused;
	}

	public void ToggleExtendedUI()
	{
		if (extendedUI.activeSelf)
			extendedUI.SetActive(false);
		else
			extendedUI.SetActive(true);
	}

	public void ToggleUI()
	{
		if (isMobileDevice)
			FindFirstObjectByType<CanvasController>().DisplayMobileUI();
		else
			FindFirstObjectByType<CanvasController>().DisplayDesktopUI();
	}

}
