using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Counters : MonoBehaviour
{
	public Text scoreText;
	public Text playersText;
	public Text foodsText;
	public Text scaleText;
	public Text speedText;

	private void Start()
	{
		Manager.Game.General.Counters = this;
	}

	public async void UpdateScore()
	{
		await Task.Delay(0);
		scoreText.text = Manager.Game.General.PlayerScore.ToString("0.00");
	}

	public async void UpdateFoods()
	{
		await Task.Delay(0);
		int count = 0;
		foreach (var item in Manager.Game.Foods)
		{
			if (!item.IsEaten)
				count++;
		}
		foodsText.text = count.ToString();
	}

	public async void UpdatePlayers()
	{
		await Task.Delay(0);
		int count = 0;
		foreach (var item in Manager.Game.Bots)
		{
			if (!item.IsDead && !item.IsPlayer)
				count++;
		}
		playersText.text = count.ToString();
		if (count == 0)
			Manager.Game.General.Controller.PlayerWin();

		if (Manager.Game.General.SelectedGameMode == GameMode.BETHEBIGGEST)
		{
			var index = 0;
			foreach (var item in Manager.Game.Bots)
			{
				if (item.IsDead && index < Manager.Game.Bots.Count -1 && !Manager.Game.Bots[index + 1].IsDead)
				{
					var nextItem = Manager.Game.Bots[index + 1];
					nextItem.Object.transform.GetChild(0).gameObject.SetActive(true);
				}
				index++;
			}
		}
	}

	public async void UpdateScale()
	{
		await Task.Delay(0);
		float scale = Manager.Game.Player.Object.transform.localScale.y;
		scaleText.text = scale.ToString("0.00");
		UpdateSpeed();
	}

	public async void UpdateSpeed()
	{
		await Task.Delay(0);
		float speed = Manager.Game.Player.Controller.movementSpeed;
		speedText.text = speed.ToString("0.00");
	}
}
