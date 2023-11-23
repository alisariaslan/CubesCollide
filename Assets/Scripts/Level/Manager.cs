using Assets.Scripts.Models;
using Assets.Scripts.Xml;
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
        {
            Application.targetFrameRate = 60;
            Game.General.IsMobileDevice = true;
        }
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



    public async void StartGame(bool isTryAgain)
    {
        Destroy(Game?.Ground?.Object);
        Game.Ground.Object = null;
        Game.General.IsPaused = true;
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
        text += ("\nGame level: " + (Game.General.SelectedLevelIndex+1));
        await Game.Chat?.Controller?.Text_Line(text);
        if (Game.General.SelectedGameMode == GameMode.BETHEBIGGEST)
        {
            text = ("Eat in order, Be the Biggest!");
            _ = Game.Chat?.Controller?.Text_Line(text);
        }
        Game.General.IsPaused = false;
    }

    public void QuickStart()
    {
        Game.General.LevelController.DeSerializeXML();
        Game.General.SelectedLevelIndex = Manager.Game.General.LevelController.GetSavedLevelIndex(Game.General.SelectedGameMode);
        StartGame(false);
    }

    public void LevelStart()
    {
        StartGame(false);
    }
    public void LevelResume()
    {
        if (!Game.General.IsPaused)
            return;
        Game.Canvas.Controller.SafeCursorVisibleShow(false);
        Game.Audio.Controller.PlayMusic();
        Game.General.IsPaused = false;
        Game.Canvas.Controller.EnableFixedUI();
        Game.Canvas.Controller.DisablePauseUI();
        Game.Canvas.Controller.EnableControllersUI();
    }

    public void LevelPause()
    {
        if (Game.General.IsPaused)
            return;
        Game.Canvas.Controller.SafeCursorVisibleShow(true);
        Game.Audio.Controller.StopMusic();
        Game.General.IsPaused = true;
        Game.Canvas.Controller.DisableFixedUI();
        Game.Canvas.Controller.EnablePauseUI();
        Game.Canvas.Controller.DisableControllersUI();
    }

    public void LevelTryAgain(bool yes)
    {
        Game.Canvas.Controller.DisableTryAgainUI();
        if (yes)
            StartGame(true);
        else
            StopGame(false);
    }

    public void LevelContinue(bool yes)
    {
        Game.Canvas.Controller.DisableContinueUI();
        if (yes)
            StartGame(false);
        else
            StopGame(false);
    }
    
    public void LevelsCompleted()
    {
        Game.Canvas.Controller.DisableWinUI();
        StopGame(false);
    }

    public async void PlayerDead()
    {
        //Game.General.AdController.LoadInterstitialAd();
        Game.Canvas.Controller.DisableControllersUI();
        Game.Audio.Controller.StopMusic();
        await Game.Chat.Controller.Text_Line(Game.Player.DeadReason + "\nTry Again!");
        Game.Camera.Controller.LookAtTheGround();
        Game.Canvas.Controller.DisableFixedUI();
        Game.Canvas.Controller.EnableTryAgainUI();
        Game.Canvas.Controller.SafeCursorVisibleShow(true);
        FindFirstObjectByType<AdHelper>().LoadInterstitialAd();
        //Game.General.AdController.ShowAd();
    }

    public async void PlayerWin()
    {
        //Game.General.AdController.LoadInterstitialAd();
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
        Game.Canvas.Controller.EnableFinishedUI();

        Game.Canvas.Controller.SafeCursorVisibleShow(true);
        //Game.General.AdController.ShowAd();
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



}
