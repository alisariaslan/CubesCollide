using System.Collections.Generic;

namespace Assets.Scripts.Models
{
	public class Game
	{
		public Player Player;
		public Ground Ground;
		public List<Food> Foods;
		public List<Bot> Bots;
		public Camera Camera;
		public General General;
		public Chat Chat;
		public Sound Sound;

		public Game() {
			this.Player = new Player();
			this.Ground = new Ground();
			this.Foods = new List<Food>();
			this.Bots = new List<Bot>();
			this.Camera = new Camera();
			this.General = new General();
			this.Chat = new Chat();
			this.Sound = new Sound();
		}
	}
}