using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Models
{
	public class General
	{
		public GameMode SelectedGameMode { get; set; }
		public List<int> SelectedGameLevels{ get; set; } = new List<int>();
		public int SelectedGameLevel { get; set; }
		private float PlayerScore_ { get; set; } = 0;
		public float PlayerScore { get { return PlayerScore_; } set { PlayerScore_ = value; } }
		public float DefaultPayerSpeed { get; set; } = 3;
		public float DefaultBotSpeed { get; set; } = 1;
		public float PlayerSpeed { get; set; } = 3;
		public float BotSpeed { get; set; } = 1;
		public bool IsMobileDevice { get; set; }
		public bool IsPaused { get; set; } = true;
		public GameObject OtoButton { get; set; }
		public GameObject PauseButton { get; set; }
		public LevelController LevelController { get; set; }
		public AdController AdController { get; set; }
		public string FoodTag { get; private set; } = "Food";
		public string BotTag { get; private set; } = "Bot";
		public string WallTag { get; private set; } = "Wall";
		public Counters Counters { get; set; }
		public Manager Controller { get; set; }
		public void ToggleOto()
		{
			if (OtoButton == null)
				return;
			OtoButton.GetComponent<ButtonPressed>().ToggleOto();
		}
		public void TogglePause()
		{
			if(PauseButton == null)
				return;
			PauseButton.GetComponent<ButtonPressed>().TogglePause();
		}
	}
}
