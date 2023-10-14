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
        DeSerializeXML();
        bool first = true;
        int index = 0;
        foreach (var item in gameModes.BETHEBIGGEST.Levels.LevelList)
        {
            if (first)
            {
                item.IsUnlocked = true;
                item.IsSaved = true;
                first = false;
            }
            else
            {
                item.IsUnlocked = false;
                item.IsSaved = false;
            }
            index++;
        }
        Manager.Game.General.SelectedLevelIndex = 0;
        SerializeXML();
        Manager.Game.Canvas.Controller.UpdateLevelsUI();
    }


    private void Update()
    {
        if (Manager.Game.General.IsPaused)
        {
            if (Input.GetKeyDown(KeyCode.F12))
                Manager.Game.General.LevelController.UnlockNextLevel(true);
            if (Input.GetKeyDown(KeyCode.F11))
                Manager.Game.General.LevelController.ResetLevels();
        }
    }

    public void UnlockNextLevel(bool isCheat)
    {
        DeSerializeXML();
        if (Manager.Game.General.SelectedLevelIndex + 1 == gameModes.BETHEBIGGEST.Levels.LevelList.Count)
            return;
        foreach (var item in gameModes.BETHEBIGGEST.Levels.LevelList)
            item.IsSaved = false;
        Manager.Game.General.SelectedLevelIndex++;
        gameModes.BETHEBIGGEST.Levels.LevelList[Manager.Game.General.SelectedLevelIndex].IsUnlocked = true;
        gameModes.BETHEBIGGEST.Levels.LevelList[Manager.Game.General.SelectedLevelIndex].IsSaved = true;
        Debug.Log("Next Level Unlocked");
        SerializeXML();
        Manager.Game.Canvas.Controller.UpdateLevelsUI();
    }

    public int GetLatestUnlockedLevel(GameMode gameMode)
    {
        int index = 0;
        if (gameMode == GameMode.BETHEBIGGEST)
        {
            DeSerializeXML();
            foreach (var item in gameModes.BETHEBIGGEST.Levels.LevelList)
            {
                if (index == levelGroup.transform.childCount)
                    break;
                if (item.IsUnlocked is false)
                    return index + 1;
                index++;
            }
        }
        return 1;
    }

    public int GetSavedLevelIndex(GameMode gameMode)
    {
        int index = 0;
        if (gameMode == GameMode.BETHEBIGGEST)
        {
            foreach (var item in gameModes.BETHEBIGGEST.Levels.LevelList)
            {
                if (index == levelGroup.transform.childCount)
                    break;
                if (item.IsSaved)
                    return index;
                index++;
            }
        }
        return 1;
    }

    public void SerializeXML()
    {
        var path = Path.Combine(Application.persistentDataPath, "Levels.xml");
        XmlSerializer s = new XmlSerializer(typeof(GameModes));
        TextWriter w = new StreamWriter(path);
        s.Serialize(w, gameModes);
        w.Close();
    }

    public void DeSerializeXML()
    {
        var path = Path.Combine(Application.persistentDataPath, "Levels.xml");
        if (!File.Exists(path))
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
