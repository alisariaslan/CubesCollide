
using System.Threading.Tasks;
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
	public GameObject winOne, winTwo, winThree;
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
		Manager.Game.General.SelectedGameLevels.Add(1);
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
		if (subObjects[index].text.Contains('X'))
		{
			if (Manager.Game.General.SelectedGameLevels.Count > 1)
			{
				Manager.Game.General.SelectedGameLevels.Remove(level);
				subObjects[index].text = "Level " + level;
			}
		}
		else
		{
			Manager.Game.General.SelectedGameLevels.Add(level);
			subObjects[index].text = "Level " + level + " X";
		}
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
			if (Manager.Game.General.SelectedGameLevel - 1 == Manager.Game.General.LevelController.gameModes.BETHEBIGGEST.Levels.Level.Count - 1)
			{
				SwitchGameModeFinal();
			}
			else
			{
				SwitchGameModeDefault();
			}
		}
	}

	public void SwitchGameModeFinal()
	{
		winOne.SetActive(false);
		winTwo.SetActive(false);
		winThree.SetActive(true);
		winText.text = "Congralutaions, You WIN!\r\nYou succesfully completed all levels in this game mode.";
	}

	public void SwitchGameModeDefault()
	{
		winOne.SetActive(true);
		winTwo.SetActive(true);
		winThree.SetActive(false);
		winText.text = "Congralutaions, You WIN!\r\nDo you wanna continue to next level?";
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
