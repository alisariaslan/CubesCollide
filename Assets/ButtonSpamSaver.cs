using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSpamSaver : MonoBehaviour
{
    private Button myButton;

    void Start()
    {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(ButtonClick);
    }

    private  async void ButtonClick()
    {
        myButton.interactable = false;
        await Task.Delay(3000);
        myButton.interactable = true;
    }

}
