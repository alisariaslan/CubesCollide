using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject modesPanel;
    public GameObject levelGroup;
    public GameObject ingamePanel;
    public GameObject extendedUI;
    public GameObject mobileControllers;
    public GameObject pcControllers;
    public GameObject controllers;
    public GameObject fixedPanel;
    public GameObject pausePanel;
    public GameObject tryPanel;
    public Text winText;
    private Animator Animator;

    private void Start()
    {
        Manager.Game.Canvas.Object = this.gameObject;
        Animator = GetComponent<Animator>();
        if (Manager.Game.General.IsMobileDevice)
            mobileControllers.SetActive(true);
        else
            pcControllers.SetActive(true);
        Manager.Game.General.SelectedGameMode = (GameMode.BETHEBIGGEST);
        Manager.Game.General.SelectedGameLevel = (1);
    }

    public void GameModeSelected(Text text)
    {
        if (text.text.Contains("Be The Biggest"))
        {
            Manager.Game.General.SelectedGameMode = (GameMode.BETHEBIGGEST);
            text.text = "Be The Biggest X";
        }
        else if (text.text.Contains("Under Atom"))
        {
            Manager.Game.General.SelectedGameMode = (GameMode.UNDERATOM);
            text.text = "Under Atom X";
        }
        else if (text.text.Contains("Purge"))
        {
            Manager.Game.General.SelectedGameMode = (GameMode.PURGE);
            text.text = "Purge X";
        }
    }

    public void GameLevelSelected(int level)
    {
        var subObjects = levelGroup.GetComponentsInChildren<Text>();
        var index = level - 1;
        for (int i = 0; i < subObjects.Length; i++)
        {
            subObjects[i].text = "Level " + (i+1);
        }
        Manager.Game.General.SelectedGameLevel = (level);
        subObjects[index].text = "Level " + level + " X";
    }

    public void EnableExtendedUI()
    {
        extendedUI.SetActive(true);
    }
    public void DisableExtendedUI()
    {
        extendedUI.SetActive(false);
    }
    public void DisableMenuUI()
    {
        Animator.Play("Up", 0);
    }
    public void EnableMenuUI()
    {
        Animator.Play("Down", 0);
    }
    public void EnableFixedUI()
    {
        Animator.Play("FixedDown", 1);
    }
    public void DisableFixedUI()
    {
        Animator.Play("FixedUp", 1);
    }
    public void EnableControllersUI()
    {
        if (Manager.Game.General.IsMobileDevice)
            Animator.Play("MobileSpawn", 4);
        else Animator.Play("PcSpawn", 4);
    }
    public void DisableControllersUI()
    {
        if (Manager.Game.General.IsMobileDevice)
            Animator.Play("MobileExit", 4);
    }
    public void EnablePauseUI()
    {
        Animator.Play("PauseDown", 2);
    }
    public void DisablePauseUI()
    {
        Animator.Play("PauseUp", 2);
    }
    public void EnableTryAgainUI()
    {
        Animator.Play("TryDown", 3);
    }
    public void DisableTryAgainUI()
    {
        Animator.Play("TryUp", 3);
    }
    public void EnableWinUI()
    {
        Animator.Play("WinEnter", 7);
        if (Manager.Game.General.SelectedGameMode == GameMode.BETHEBIGGEST)
        {
            if (Manager.Game.General.SelectedGameLevel == Manager.Game.General.LevelController.gameModes.BETHEBIGGEST.Levels.LevelList.Count)
            {
                SwitchGameModeFinal();
            }
            else
            {
                SwitchGameModeNext();
            }
        }
    }

    public void SwitchGameModeFinal()
    {
        Debug.Log("selected: "+Manager.Game.General.SelectedGameLevel+  ", final: "+ Manager.Game.General.LevelController.gameModes.BETHEBIGGEST.Levels.LevelList.Count);
    }

    public void SwitchGameModeNext()
    {
        Debug.Log("selected: " + Manager.Game.General.SelectedGameLevel + ", next: " + Manager.Game.General.LevelController.gameModes.BETHEBIGGEST.Levels.LevelList.Count);
    }

    public void DisableWinUI()
    {
        Animator.Play("WinExit", 7);
    }

    public void EnterModesMenu()
    {
        Animator.Play("ModesEnter", 5);
    }
    public void ExitModesMenu()
    {
        Animator.Play("ModesExit", 5);
    }
    public void EnterLevelsMenu()
    {
        Animator.Play("LevelsEnter", 5);
    }
    public void ExitLevelsMenu()
    {
        Animator.Play("LevelsExit", 5);
    }
    public void ChatToggle()
    {
        Animator.Play("ChatToggle", 6);
    }
    public void ChatExit()
    {
        Animator.Play("ChatExit", 6);
    }

    public void SafeCursorVisibleShow(bool state)
    {
        if (!Manager.Game.General.IsMobileDevice)
            Cursor.visible = state;
    }
}
