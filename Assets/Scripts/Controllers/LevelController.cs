using Assets.Scripts.Xml;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
	public GameObject levelGroup;
	public TextAsset textAsset;
	public GameModes gameModes;

	private async void Start()
	{
		await Task.Delay(100);
		Manager.Game.General.LevelController = this;
	}
	public void ResetLevels()
	{
		cheatLevelCounter = 1;
		DeSerializeXML();
		bool first = true;
		foreach (var item in gameModes.BETHEBIGGEST.Levels.LevelList)
		{
			var index = Convert.ToInt32(item.LevelNo) - 1;
			if (index == levelGroup.transform.childCount)
				break;
			levelGroup.transform.GetChild(index).GetComponentInChildren<Text>().text = "Level " + item.LevelNo;
			if (first)
			{
				item.IsLocked = "false";
				first = false;
			}
			else
				item.IsLocked = "true";
		}
		levelGroup.transform.GetChild(0).GetComponentInChildren<Text>().text = "Level 1 X";
		Manager.Game.General.SelectedGameLevel = (1);
		SerializeXML();
		UpdateLevelsUI();
	}

	public void UpdateLevelsUI()
	{
		DeSerializeXML();
		int latestIndex = 0;
		int latestLevelNo = 0;
		var c = levelGroup.transform.childCount;
		foreach (var item in gameModes.BETHEBIGGEST.Levels.LevelList)
		{
			int levelNo = Convert.ToInt32(item.LevelNo);
			int index = levelNo - 1;
			if (index == c)
				break;
			if (!Convert.ToBoolean(item.IsLocked))
			{
				latestIndex = index;
				latestLevelNo = levelNo;
			}
			levelGroup.transform.GetChild(index).GetComponent<Button>().interactable = !Convert.ToBoolean(item.IsLocked);
			levelGroup.transform.GetChild(index).GetComponentInChildren<Text>().text = "Level " + item.LevelNo;
		}
		levelGroup.transform.GetChild(latestIndex).GetComponentInChildren<Text>().text = "Level " + latestLevelNo + " X";
		Manager.Game.General.SelectedGameLevel = latestLevelNo;

		SerializeXML();
	}

	private void Update()
	{
		if(Manager.Game.General.IsPaused)
		{
			if (Input.GetKeyDown(KeyCode.F12))
				Manager.Game.General.LevelController.UnlockNextLevel(true);
			if (Input.GetKeyDown(KeyCode.F11))
				Manager.Game.General.LevelController.ResetLevels();
		}
	}

	int cheatLevelCounter = 1;
	public void UnlockNextLevel(bool isCheat)
	{
		DeSerializeXML();
		int levelNow = 1;
		if (isCheat)
		{
			levelNow = cheatLevelCounter;
			cheatLevelCounter++;
		}
		else
			levelNow = Manager.Game.General.SelectedGameLevel;
		var index = levelNow - 1;
		var nextIndex = index + 1;
		if (nextIndex > gameModes.BETHEBIGGEST.Levels.LevelList.Count - 1)
			return;
		gameModes.BETHEBIGGEST.Levels.LevelList[nextIndex].IsLocked = "false";
		Debug.Log("Next Level Unlocked");
		SerializeXML();
		UpdateLevelsUI();
	}

	public int GetLatestUnlockedLevel(GameMode gameMode)
	{
		int index = 0;
		if (gameMode == GameMode.BETHEBIGGEST)
		{
			DeSerializeXML(); string levelStr = "1";
			foreach (var item in gameModes.BETHEBIGGEST.Levels.LevelList)
			{
				if (index == levelGroup.transform.childCount)
					break;

				if (!Convert.ToBoolean(item.IsLocked))
					levelStr = item.LevelNo;

				index++;
			}
			return Convert.ToInt32(levelStr);

		}
		return 1;
	}

	private void SerializeXML()
	{
		var path = Path.Combine(Application.persistentDataPath, "Levels.xml");
		XmlSerializer s = new XmlSerializer(typeof(GameModes));
		TextWriter w = new StreamWriter(path);
		s.Serialize(w, gameModes);
		w.Close();
	}
	private void DeSerializeXML()
	{
		var path = Path.Combine(Application.persistentDataPath, "Levels.xml");
		if (!File.Exists(path) || Debug.isDebugBuild)
		{
			File.Create(path).Dispose();
			File.WriteAllText(path, textAsset.text);
		}
		XmlSerializer s = new XmlSerializer(typeof(GameModes));
		gameModes = new GameModes();
		TextReader r = new StreamReader(path);
		gameModes = (GameModes)s.Deserialize(r);
		r.Close();
	}
}
