using System.Globalization;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoBehaviour
{
	private Text text;

	void Start()
	{
		Manager.Game.Chat.Object = this.gameObject;
		text = GetComponent<Text>();
	}

	public async Task Text_PlayerJoined(string playerName)
	{
		text.text = (char.ToUpper(playerName[0], CultureInfo.InvariantCulture) + playerName.Substring(1) + " is joined.");
		Manager.Game.Canvas.Controller.ChatToggle();
		await Task.Delay(500); //Enter
		await Task.Delay(3000); //Stay
		Manager.Game.Canvas.Controller.ChatExit();
		await Task.Delay(500); //Exit
	}

	public async Task Text_PlayerDead(string playerName)
	{
		text.text = (char.ToUpper(playerName[0], CultureInfo.InvariantCulture) + playerName.Substring(1) + " is eaten.");
		Manager.Game.Canvas.Controller.ChatToggle();
		await Task.Delay(500); //Enter
		await Task.Delay(3000); //Stay
		Manager.Game.Canvas.Controller.ChatExit();
		await Task.Delay(500); //Exit
	}

	public async Task Text_Line(string str)
	{
		text.text = (str);
		Manager.Game?.Canvas?.Controller?.ChatToggle();
		await Task.Delay(500); //Enter
		await Task.Delay(3000); //Stay
		Manager.Game?.Canvas?.Controller?.ChatExit();
		await Task.Delay(500); //Exit
	}

}
