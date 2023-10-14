using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public GameMode mode;
    public string levelName;
    
    public void InvokeSelect()
    {
        FindObjectOfType<CanvasController>().GameLevelSelected(mode,levelName);
    }
}
