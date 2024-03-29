using Assets.Scripts.Xml;
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
        Manager.Game.General.SelectedLevelIndex = (1);
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

    public void UpdateLevelsUI()
    {
        Manager.Game.General.LevelController.DeSerializeXML();
        int index = 0;
        foreach (var item in Manager.Game.General.LevelController.gameModes.BETHEBIGGEST.Levels.LevelList)
        {
            levelGroup.transform.GetChild(index).GetComponent<Button>().interactable = item.IsUnlocked;
            levelGroup.transform.GetChild(index).GetComponentInChildren<Text>().text = "Level: " + item.LevelName;
            if (item.IsSaved)
                levelGroup.transform.GetChild(index).GetComponentInChildren<Text>().text = "Level: " + (index + 1) + " <<";
            index++;
        }
        Manager.Game.General.LevelController.SerializeXML();
    }

    public void GameLevelSelected(GameMode mode, string levelName)
    {
        Manager.Game.General.LevelController.DeSerializeXML();
        if (mode == GameMode.BETHEBIGGEST)
        {
            Manager.Game.General.LevelController.gameModes.BETHEBIGGEST.Levels.LevelList.ForEach(item => item.IsSaved = false);
            int index = 0;
            foreach (var item in Manager.Game.General.LevelController.gameModes.BETHEBIGGEST.Levels.LevelList)
            {
                if (item.LevelName.Equals(levelName))
                {
                    Manager.Game.General.LevelController.gameModes.BETHEBIGGEST.Levels.LevelList[index].IsSaved = true;
                    Manager.Game.General.SelectedLevelIndex = index;
                }
                index++;
            }
        }
        Manager.Game.General.LevelController.SerializeXML();
        UpdateLevelsUI();
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
    public void EnableFinishedUI()
    {
        if (Manager.Game.General.SelectedLevelIndex + 1 == Manager.Game.General.LevelController.gameModes.BETHEBIGGEST.Levels.LevelList.Count)
        {
            Animator.Play("WinEnter", 7);

        }
        else
        {
            Animator.Play("ContinueEnter", 8);
        }
    }
    public void DisableWinUI()
    {
        Animator.Play("WinExit", 7);
    }
    public void DisableContinueUI()
    {
        Animator.Play("ContinueExit", 8);
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
    public void EnterScoreboard()
    {
        Animator.Play("ScoreboardEnter", 5);
    }
    public void ExitScoreboard()
    {
        Animator.Play("ScoreboardExit", 5);
    }

    public void SafeCursorVisibleShow(bool state)
    {
        if (!Manager.Game.General.IsMobileDevice)
            Cursor.visible = state;
    }
}
