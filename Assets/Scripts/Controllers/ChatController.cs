using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoBehaviour
{
	private Text text;
	private List<string> lastTenString;
	// Start is called before the first frame update
	void Start()
	{
		Manager.Game.Chat.Object = gameObject;
		text = GetComponent<Text>();
		text.text = "";
		lastTenString = new List<string>();
	}

	// Update is called once per frame
	public void Text_PlayerJoined(string playerName)
	{
		lastTenString.Add("Player \"" + playerName + "\" is joined.");
		CompleteText();
	}
	public void Text_Line(string str)
	{
		lastTenString.Add(str);
		CompleteText();
	}
	public void Text_Paragraph(string paragraph)
	{

	}

	private void PrepareTexts()
	{
		text.text = "";
		if (lastTenString.Count > 10)
			lastTenString.RemoveAt(0);
	}

	private void CompleteText()
	{
		if (text == null)
			return;

		PrepareTexts();
		foreach (var item in lastTenString)
		{
			text.text += item + "\n";
		}

	}

	public async void AnnounceBots()
	{
		foreach (var item in Manager.Game.Bots)
		{
			await Task.Delay(100);
			Text_PlayerJoined(item.Name);
		}
		await SlowClear(1000);
	}

	private async Task SlowClear(int delay)
	{
		while (lastTenString.Count > 0)
		{
			lastTenString.RemoveAt(lastTenString.Count - 1);
			await Task.Delay(delay);
			CompleteText();
		}
	}
}
