using Assets.Scripts.Models;
using System.Threading.Tasks;
using UnityEngine;

public class Manager : MonoBehaviour
{
	public static Game Game { get; set; }

	[Header("Game Objects")]
	public GameObject mainCam;

	[Header("Player Settings")]
	public bool playerOtoEnabledStart = true;

	[Header("Spawn Settings")]
	public bool spawnMyPlayer = true;
	public bool spawnBots = true;
	public bool spawnFoods = true;

	[Header("Device Settings")]
	public bool isMobileDevice = false;

	[Header("Chat Settings")]
	public float chatClearInterval = 5f;

	private void Start()
	{
		Game = new Game();
		if (Application.isMobilePlatform || isMobileDevice)
			Game.General.IsMobileDevice = true;
		Game.Player.IsOtoEnabled = playerOtoEnabledStart;
		Game.Chat.ClearInterval = chatClearInterval;
		Game.General.Controller = this;
	}

	private void OnDestroy()
	{
		Game = null;
	}

	public void ExitApp()
	{
		Debug.Log("Quit Game");
		Application.Quit();
	}

	public void QuickStart()
	{
		Debug.Log(Game.General.SelectedGameMode);
		int latestLevel = Manager.Game.General.LevelController.GetLatestUnlockedLevel(Game.General.SelectedGameMode);
		Game.General.SelectedGameLevels.Clear();
		Game.General.SelectedGameLevels.Add(latestLevel);
		StartGame(false);
	}
	public async void StartGame(bool isTryAgain)
	{
		Game.General.IsPaused = true;
		if (!isTryAgain)
			Game.General.SelectedGameLevel = Game.General.SelectedGameLevels[new System.Random().Next(0, Game.General.SelectedGameLevels.Count)];
		Game.General.PlayerScore = 0;
		Game.Canvas.Controller.DisableMenuUI();
		Game.Canvas.Controller.DisableExtendedUI();
		Game.Canvas.Controller.SafeCursorVisibleShow(false);
		Game.Audio.Controller.SwitchToIngameMusic();
		Game.Audio.SoundSource.Stop();
		await Game.Random.Controller.RandomizeGround();
		Game.Camera.Controller.LookAtTheGround();
		await Game.Random.Controller.CalculatePoints();
		Game.Random.Controller.OptimizeRandomize();
		await Game.Random.Controller.GenerateFoods();
		await Game.Random.Controller.GenerateBots();
		//Game.Random.Controller.RandomizeSpawnPositions();
		await Game.Random.Controller.SpawnMyPlayer();
		await Task.Delay(1000); //Waiting for Player Start Funcs
		await Game.Random.Controller.CalculateAvaibleness();
		Game.Canvas.Controller.EnableFixedUI();
		Game.Camera.Controller.LookAtThePlayer();
		await Task.Delay(1000); //Waiting for UI Start Funcs
		Game.General.Counters.UpdatePlayers();
		Game.General.Counters.UpdateFoods();
		Game.General.Counters.UpdateScore();
		Game.General.Counters.UpdateScale();
		Game.Canvas.Controller.EnableControllersUI();
		Game.General.OtoButton.GetComponent<ButtonPressed>().CheckOto();
		var text = ("Game mode: " + Game.General.SelectedGameMode);
		text += ("\nGame level: " + Game.General.SelectedGameLevel);
		await Game.Chat.Controller.Text_Line(text);
		if (Game.General.SelectedGameMode == GameMode.BETHEBIGGEST)
		{
			text = ("Eat in order, Be the Biggest!");
			_= Game.Chat.Controller.Text_Line(text);
		}
		Game.General.IsPaused = false;
	}

	public void TryAgain(bool realy)
	{
		Game.Canvas.Controller.DisableTryAgainUI();
		if (realy)
			StartGame(true);
		else
			StopGame(false);
	}

	public void NextLevel(bool realy)
	{
		Game.Canvas.Controller.DisableWinUI();
		if (realy)
		{
			Destroy(Game.Ground.Object);
			Game.Ground.Object = null;
			StartGame(false);
		}
		else
			StopGame(false);
	}

	public async void PlayerDead()
	{
		Game.Canvas.Controller.DisableControllersUI();
		Game.Audio.Controller.StopMusic();
		await Game.Chat.Controller.Text_Line(Game.Player.DeadReason+"\nTry Again!");
		Game.Camera.Controller.LookAtTheGround();
		Game.Canvas.Controller.DisableFixedUI();
		Game.Canvas.Controller.EnableTryAgainUI();
		Game.Canvas.Controller.SafeCursorVisibleShow(true);
	}

	public async void PlayerWin()
	{
		Game.General.IsPaused = true;
		Game.General.Counters.UpdateScale();
		if (Game.Audio.Controller.soundEnabled)
			Game.Audio.SoundSource.PlayOneShot(Game.Audio.Controller.meWin);
		Game.Canvas.Controller.DisableControllersUI();
		Game.Audio.Controller.StopMusic();
		Manager.Game.General.LevelController.UnlockNextLevel(false);
		await Game.Chat.Controller.Text_Line("Your cube has Win!");
		Destroy(Game.Player.Object);
		Game.Camera.Controller.LookAtTheGround();
		Game.Canvas.Controller.DisableFixedUI();
		Game.Canvas.Controller.EnableWinUI();
		Game.Canvas.Controller.SafeCursorVisibleShow(true);
	}

	public void StopGame(bool paused)
	{
		if (paused)
			Game.Canvas.Controller.DisablePauseUI();
		Game.Canvas.Controller.DisableControllersUI();
		Game.Canvas.Controller.EnableExtendedUI();
		Game.Canvas.Controller.EnableMenuUI();
		Game.Audio.Controller.SwitchToMenuMusic();
		Game.Camera.Controller.LookAtTheDancers();
		foreach (var item in Game.Foods)
			Destroy(item.Object);
		foreach (var item in Game.Bots)
			Destroy(item.Object);
		Destroy(Game.Ground.Object);
		Destroy(Game.Player.Object);
		Game.Canvas.Controller.SafeCursorVisibleShow(true);
		Game.General.IsPaused = true;
	}

	public void Pause()
	{
		if (Game.General.IsPaused)
			return;
		Game.Canvas.Controller.SafeCursorVisibleShow(true);
		Game.Audio.Controller.StopMusic();
		Game.General.IsPaused = true;
		Game.Canvas.Controller.DisableFixedUI();
		Game.Canvas.Controller.EnablePauseUI();
	}

	public void Resume()
	{
		if (!Game.General.IsPaused)
			return;
		Game.Canvas.Controller.SafeCursorVisibleShow(false);
		Game.Audio.Controller.PlayMusic();
		Game.General.IsPaused = false;
		Game.Canvas.Controller.EnableFixedUI();
		Game.Canvas.Controller.DisablePauseUI();
	}

}
