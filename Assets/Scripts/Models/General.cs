using UnityEngine;

namespace Assets.Scripts.Models
{
	public class General
	{
		public bool IsMobileDevice { get; set; }
		public bool IsPaused { get; set; }
		public GameObject OtoButton { get; set; }
		public GameObject PauseButton { get; set; }
		public GameObject Menu { get; set; }
		public string FoodTag { get; set; }
		public string BotTag { get; set; } = "Bot";
		public string WallTag { get; private set; } = "Wall";
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
