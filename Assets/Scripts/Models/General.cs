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

		public void ToggleOto()
		{
			OtoButton.GetComponent<ButtonPressed>().ToggleOto();
		}

		public void TogglePause()
		{
			OtoButton.GetComponent<ButtonPressed>().TogglePause();
		}
	}
}
