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
		public Audio Audio;
		public Canvas Canvas;
		public Random Random;
		public Calculation Calculation;
		public Game() {
			this.Player = new Player();
			this.Ground = new Ground();
			this.Foods = new List<Food>();
			this.Bots = new List<Bot>();
			this.Camera = new Camera();
			this.General = new General();
			this.Chat = new Chat();
			this.Audio = new Audio();
			this.Canvas = new Canvas();
			this.Random = new Random();
			this.Calculation = new Calculation();
		}
	}
}