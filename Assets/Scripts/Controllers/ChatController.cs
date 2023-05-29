using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoBehaviour
{
	private Text text;
	private List<string> lastTenString;

	void Start()
	{
		Manager.Game.Chat.Object = gameObject;
		text = GetComponent<Text>();
		text.text = "";
		lastTenString = new List<string>(); 
		timer = Manager.Game.Chat.ClearInterval;
	}

	float timer = 0f;
	bool isEnabled = false;

	void Update()
	{
		if (isEnabled)
		{
			timer -= Time.deltaTime;
			if (timer < 0)
			{
				ClearTextOne();
				timer = Manager.Game.Chat.ClearInterval;
			}
		}
	}

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

	public async void AnnounceBots()
	{
		foreach (var item in Manager.Game.Bots)
		{
			await Task.Delay(100);
			Text_PlayerJoined(item.Name);
		}
	}

	private void CompleteText()
	{
		while (lastTenString.Count > 10)
			lastTenString.RemoveAt(0);

		text.text = "";
		foreach (var item in lastTenString)
			text.text += item + "\n";

		isEnabled = true;
		timer = Manager.Game.Chat.ClearInterval;
	}

	public void ClearTextOne()
	{
		if (lastTenString.Count > 0)
			lastTenString.RemoveAt(0);

		Debug.Log("Deleted");

		if (lastTenString.Count == 0)
			isEnabled = false;

		text.text = "";
		foreach (var item in lastTenString)
			text.text += item + "\n";
	}





}
