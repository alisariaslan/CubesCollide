using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Assets.Scripts.Xml;
using UnityEngine;

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
#if DEBUG
            if (Input.GetKeyDown(KeyCode.F10))
                FindFirstObjectByType<AdHelper>().LoadInterstitialAd();
#endif
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
        XmlSerializer serializer = new XmlSerializer(typeof(GameModes));
        using (StringWriter writer = new StringWriter())
        {
            serializer.Serialize(writer, gameModes);
            string xmlString = writer.ToString();
            PlayerPrefs.SetString("save", xmlString);
        }
    }

    public void DeSerializeXML()
    {
        string save = PlayerPrefs.GetString("save", null);
        if(string.IsNullOrEmpty(save))
        {
            PlayerPrefs.SetString("save", textAsset.text);
            save = textAsset.text;
        }
        using (TextReader reader = new StringReader(save)) // Use StringReader to read from a string
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GameModes));
            gameModes = (GameModes)serializer.Deserialize(reader);
        }
    }
}
