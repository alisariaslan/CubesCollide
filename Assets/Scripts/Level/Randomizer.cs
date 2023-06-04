using Assets.Scripts;
using Assets.Scripts.Helpers;
using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Randomizer : MonoBehaviour
{
	[Header("Game Objects")]
	public GameObject player;
	public GameObject bot;
	public GameObject food;
	public GameObject arena;

	[Header("Colors")]
	public bool randomSkyColor = true;
	public bool randomDancerColor = true;
	public bool randomGroundColor = true;
	public bool randomPlayerColor = true;
	public bool randomBotColor = true;
	public bool randomFoodColor = true;

	[Header("Map Settings")]
	public bool optimizeMapForGM = true;
	public int startRandomMapSize = 2;
	public int endRandomMapSize = 2;

	[Header("Player Settings")]
	public bool optimizePlayerForGM = true;
	public float minPlayerSize = .35f;
	public float maxPlayerSize = 1f;

	[Header("Bot Settings")]
	public bool optimizeBotForGM = true;
	public float minBotSize = .35f;
	public float maxBotSize = 15f;
	public int maxBotCount = 1000;
	public int maxXCount = 100;
	public int maxYCount = 100;
	public float XSpacing = 2.5f;
	public float ZSpacing = 2.5f;
	private float XSpacing_;
	private float ZSpacing_;
	public int botSpawnDelay = -1;
	public int minRandomName = 3;
	public int maxRandomName = 7;

	[Header("Food Settings")]
	public bool optimizeFoodForGM = true;
	public float minFoodSize = .1f;
	public float maxFoodSize = 1f;
	public int foodPass = 2;
	public int foodSpawnDelay = 1;
	public float foodDelayMultiplier = 10;

	void Start()
	{
		XSpacing_ = XSpacing;
		ZSpacing_ = ZSpacing;
		Manager.Game.Random.Object = this.gameObject;
		if (randomSkyColor)
		{
			RenderSettings.skybox.SetColor("_Top", Helper.RandomColor());
			RenderSettings.skybox.SetColor("_Bottom", Helper.RandomColor());
		}
		else
		{
			RenderSettings.skybox.SetColor("_Top", new Color(0, 255, 0));
			RenderSettings.skybox.SetColor("_Bottom", new Color(0, 0, 255));
		}

		if (randomDancerColor)
		{
			GameObject.Find("DancerCubeLeft").GetComponent<Renderer>().material.color = Helper.RandomColor();
			GameObject.Find("DancerCubeRight").GetComponent<Renderer>().material.color = Helper.RandomColor();
		}
	}

	public async Task CalculateAvaibleness()
	{
		await Task.Delay(10);
		int calculationCount = 0;
		int chunk = 10;
		bool isFieldsOverLoad = false;
		double map_border_size, map_total_size, map_max_object_count;
		float maxSize = 0;
		foreach (var item in Manager.Game?.Bots)
			if (item.Object.transform.localScale.y > maxSize)
				maxSize = item.Object.transform.localScale.y;
		foreach (var item in Manager.Game?.Foods)
			if (item.Object.transform.localScale.y > maxSize)
				maxSize = item.Object.transform.localScale.y;
		if (Manager.Game.Player.Object.transform.localScale.y > maxSize)
			maxSize = Manager.Game.Player.Object.transform.localScale.y;
		do
		{
			if (isFieldsOverLoad)
				chunk--;
			map_border_size = Manager.Game.Ground.Size * chunk;
			map_total_size = Math.Pow(map_border_size, 2);
			map_max_object_count = map_total_size / maxSize;
			isFieldsOverLoad = map_max_object_count % 2 == 0 ? false : true;
			calculationCount++;
		} while (isFieldsOverLoad);
		Debug.Log("Map Border Size: " + map_border_size);
		Debug.Log("Map Total Size: " + map_total_size);
		Debug.Log("Map Max Object Count: " + map_max_object_count);
		if (calculationCount > 1)
			Debug.Log("Map Recalculation Count: " + calculationCount);
	}
	public async Task CalculatePoints()
	{
		await Task.Delay(10);
		List<Vector3> Points = new List<Vector3>();
		int columnCount = 10 * (int)Manager.Game.Ground.Size;
		int rowCount = columnCount;
		int columnNow;
		int rowNow = 0;
		while (rowNow < rowCount)
		{
			columnNow = 0;
			while (columnNow < columnCount)
			{
				float x = (Manager.Game.Ground.Size * -5f + .5f) + columnNow;
				float z = (Manager.Game.Ground.Size * 5f - .5f) - rowNow;
				Vector3 pos = new Vector3(x, 0, z);
				Points.Add(pos);
				columnNow++;
			}
			rowNow++;
		}
		Manager.Game.Ground.Points = Points;
	}
	public async Task RandomizeGround()
	{
		if (Manager.Game.Ground.Object != null)
			return;
		await Task.Delay(10);
		var my_ground = Instantiate(arena, new Vector3(0, 0, 0), Quaternion.identity, GameObject.Find("InGameObjects").transform);
		if (randomGroundColor)
		{
			Color rndColor = Helper.RandomColor();
			my_ground.GetComponent<Renderer>().material.color = rndColor;
			foreach (var item in my_ground.GetComponentsInChildren<Renderer>())
			{
				if (!item.gameObject.CompareTag("Wall"))
					item.material.color = rndColor;
			}
		}
		if (optimizeMapForGM && Manager.Game.General.SelectedGameMode == GameMode.BETHEBIGGEST)
		{
			startRandomMapSize = Manager.Game.General.SelectedGameLevel;
			endRandomMapSize = Manager.Game.General.SelectedGameLevel;
		}
		var randomSize = new System.Random().Next(startRandomMapSize, endRandomMapSize + 1); Debug.Log("Map Scale: " + randomSize);
		my_ground.transform.localScale = new Vector3(1 * randomSize, 1, 1 * randomSize);
		Manager.Game.Ground.Object = my_ground;
	}
	public void OptimizeRandomize()
	{
		var map_scale = Manager.Game.Ground.Size;

		if (Manager.Game.General.SelectedGameMode == GameMode.BETHEBIGGEST)
		{
			if (optimizePlayerForGM)
			{
				minPlayerSize = 0.15f;
				maxPlayerSize = minPlayerSize;
			}
			if (optimizePlayerForGM)
			{
				Manager.Game.General.MaxObjectSpeed = 1 + map_scale / 4;
			}
			if (optimizeBotForGM)
			{
				minBotSize = 0.1f;
				maxBotSize = minBotSize;
				XSpacing = XSpacing_;
				ZSpacing = ZSpacing_;
				XSpacing += Manager.Game.Ground.Size / 2;
				ZSpacing += Manager.Game.Ground.Size / 2;
			}
			if (optimizeFoodForGM)
				Manager.Game.General.Controller.spawnFoods = false;
		}
		/*
		if (playerSpeedOptimizer)
		{
			Manager.Game.General.Controller.MaxObjectSpeed = 2 + map_scale / 10;
			Manager.Game.General.MaxObjectSpeed = Manager.Game.General.Controller.MaxObjectSpeed;
		}
		if (foodSpawnOptimizer)
		{
			foodSpawnDelay = 1;
			foodDelayMultiplier = Manager.Game.Ground.Size;
			foodPass = (int)Manager.Game.Ground.Size / 2;
			minFoodSize = 0.1f + Manager.Game.Ground.Size / 100;
			maxFoodSize = 1f + Manager.Game.Ground.Size / 10;
		}
		if (botSpawnOptimizer)
		{
			maxBotCount = (int)(Manager.Game.Ground.Size * 100);
			maxXCount = (int)(Manager.Game.Ground.Size * 10);
			maxYCount = maxXCount;
			minBotSize = Manager.Game.Ground.Size / 10;
			maxBotSize = Manager.Game.Ground.Size / 2;
			XSpacing = (Manager.Game.Ground.Size) / 2f;
			ZSpacing = (Manager.Game.Ground.Size) / 2f;
		}
		*/

	}
	public async Task GenerateFoods()
	{
		if (!FindFirstObjectByType<Manager>().spawnFoods)
			return;
		await Task.Delay(10);
		if (Manager.Game.Foods != null && Manager.Game.Foods.Count > 0)
		{
			foreach (var item in Manager.Game.Foods)
				Destroy(item.Object);
			Manager.Game.Foods.Clear();
		}
		List<Food> Foods = new List<Food>();
		int resetCounter = 0;
		int counter = 0;
		int passed = 0;
		foreach (var item in Manager.Game.Ground.Points)
		{
			if (passed % (foodPass + 1) != 0)
			{
				passed++;
				continue;
			}
			var scale = UnityEngine.Random.Range(minFoodSize, maxFoodSize);
			Vector3 spawnPos = new Vector3(item.x, scale / 2, item.z);
			if (foodSpawnDelay > 0 && resetCounter > foodDelayMultiplier)
			{
				await Task.Delay(foodSpawnDelay);
				resetCounter = 0;
			}
			GameObject SpawnedFood = Instantiate(food, spawnPos, Quaternion.identity, GameObject.Find("FoodCollection").transform);
			Food MapFood = new Food()
			{
				Index = counter,
				Object = SpawnedFood,
				IsEaten = false,
				EatenTime = DateTime.Now,
			};
			SpawnedFood.GetComponent<FoodController>().Index = counter;
			Foods.Add(MapFood);
			if (randomFoodColor)
				SpawnedFood.GetComponent<Renderer>().material.color = Helper.RandomColor();
			SpawnedFood.transform.localScale = new Vector3(scale, scale, scale);
			resetCounter++;
			passed++;
			counter++;
		}
		Manager.Game.Foods = Foods;
	}
	public async Task GenerateBots()
	{
		if (!FindFirstObjectByType<Manager>().spawnBots)
			return;
		await Task.Delay(10);
		if (Manager.Game.Bots != null && Manager.Game.Bots.Count > 0)
		{
			foreach (var item in Manager.Game.Bots)
			{
				GameObject.Destroy(item.Object);
			}
			Manager.Game.Bots.Clear();
		}
		List<Bot> Bots = new List<Bot>();
		float lastSize = 0f;
		float biggestSize = 0f;
		float startX = Manager.Game.Ground.Size * -5f + .5f;
		float startZ = Manager.Game.Ground.Size * 5f - .5f;
		float markedX = startX;
		float markedZ = startZ;
		float endX = Manager.Game.Ground.Size * +5f - .5f;
		float endZ = Manager.Game.Ground.Size * -5f + .5f;
		bool atTheStartX = true;
		int xCount = 0;
		int yCount = 0;
		int counter = 0;
		float random_size = 0;
		while (true)
		{
			while (true)
			{
				if (optimizeBotForGM && Manager.Game.General.SelectedGameMode == GameMode.BETHEBIGGEST)
				{
					random_size += minBotSize;
				}
				else
				{
					random_size = UnityEngine.Random.Range(minBotSize, maxBotSize);
				}
				if (atTheStartX)
				{
					startX = markedX + random_size / 2;
					atTheStartX = false;
				}
				else
				{
					startX += random_size / 2 + lastSize / 2 + XSpacing;
					if (startX + random_size / 2 > endX)
					{
						if (optimizeBotForGM && Manager.Game.General.SelectedGameMode == GameMode.BETHEBIGGEST)
						{
							random_size -= minBotSize;
						}
						break;
					}
				}
				startZ = markedZ - random_size / 2;
				Vector3 spawnPos = new Vector3(startX, random_size / 2, startZ);
				if (botSpawnDelay > 0)
					await Task.Delay(botSpawnDelay);
				GameObject SpawnedBot = Instantiate(bot, spawnPos, Quaternion.identity, GameObject.Find("BotCollection").transform);
				if (counter == 0 && Manager.Game.General.SelectedGameMode == GameMode.BETHEBIGGEST)
					SpawnedBot.transform.GetChild(0).gameObject.SetActive(true);
				if (randomBotColor)
					SpawnedBot.GetComponent<Renderer>().material.color = Helper.RandomColor();
				SpawnedBot.transform.localScale = new Vector3(random_size, random_size, random_size);
				Bot botModel = new Bot()
				{
					Index = counter,
					Name = NameRandomizer.GetRandomName(minRandomName, maxRandomName),
					Object = SpawnedBot,
					SpawnPosition = SpawnedBot.transform.position,
					IsDead = false
				};
				SpawnedBot.GetComponent<BotController>().Index = counter;
				Bots.Add(botModel);
				lastSize = random_size;
				if (random_size > biggestSize)
					biggestSize = random_size;
				counter++;
				if (maxBotCount > 0 && counter >= maxBotCount)
				{
					Manager.Game.Bots = Bots;
					return;
				}
				xCount++;
				if (maxXCount > 0 && xCount >= maxXCount)
				{
					xCount = 0;
					break;
				}
			}
			atTheStartX = true;
			markedZ -= biggestSize + ZSpacing;
			if (markedZ - biggestSize / 2 < endZ + biggestSize / 2)
				break;
			yCount++;
			if (maxYCount > 0 && yCount >= maxYCount)
				break;
		}
		Manager.Game.Bots = Bots;
	}

	/*
	private List<Vector3> GenerateRandomLoop(List<Vector3> listToShuffle)
	{
		for (int i = listToShuffle.Count - 1; i > 0; i--)
		{
			var k = new System.Random().Next(i + 1);
			var value = listToShuffle[k];
			listToShuffle[k] = listToShuffle[i];
			listToShuffle[i] = value;
		}
		return listToShuffle;
	}
	private  void RandomizeSpawnPositions()
	{
		if (Manager.Game.General.SelectedGameMode == GameMode.BETHEBIGGEST)
		{
			List<Vector3> vector3s = new List<Vector3>();
			foreach (var item in Manager.Game.Bots)
				vector3s.Add(item.Object.transform.position);
			List<Vector3> newVector3s = GenerateRandomLoop(vector3s);
			int i = 0;
			foreach (var item in Manager.Game.Bots)
			{
				var scale = item.Object.transform.localScale.y;
				Vector3 pos = new Vector3(newVector3s[i].x, scale / 2, newVector3s[i].z);
				item.Object.transform.position = pos;
				i++;
			}
		}
	}*/
	public async Task SpawnMyPlayer()
	{
		if (!FindFirstObjectByType<Manager>().spawnMyPlayer)
			return;
		await Task.Delay(10);
		Vector3 spawn_pos = Vector3.zero;
		var random_size = UnityEngine.Random.Range(minPlayerSize, maxPlayerSize);
		if (optimizePlayerForGM && Manager.Game.General.SelectedGameMode == GameMode.BETHEBIGGEST)
		{
			var bot = Manager.Game.Bots[new System.Random().Next(0, Manager.Game.Bots.Count)].Object;
			var pos = bot.transform.position;
			var scale = bot.transform.localScale.y;
			switch (new System.Random().Next(0, 4))
			{
				case 0:
					spawn_pos = new Vector3(pos.x - random_size - (scale / 2), random_size / 2, pos.z + random_size + (scale / 2));
					break;
				case 1:
					spawn_pos = new Vector3(pos.x + random_size + (scale / 2), random_size / 2, pos.z + random_size + (scale / 2));
					break;
				case 2:
					spawn_pos = new Vector3(pos.x + random_size + (scale / 2), random_size / 2, pos.z - random_size - (scale / 2));
					break;
				case 3:
					spawn_pos = new Vector3(pos.x - random_size - (scale / 2), random_size / 2, pos.z - random_size - (scale / 2));
					break;
			}
		}
		else
		{
			if (Manager.Game.Bots.Count > 0)
			{
				var size = Manager.Game.Bots.Count;
				System.Random random = new System.Random();
				var selectedindex = random.Next(0, size);
				Manager.Game.Bots[selectedindex].IsPlayer = true;
				Vector3 selectedPos = Manager.Game.Bots[selectedindex].SpawnPosition;
				GameObject.Destroy(Manager.Game.Bots[selectedindex].Object);
				spawn_pos = new Vector3(selectedPos.x, random_size / 2, selectedPos.z);
			}
			else if (Manager.Game.Foods.Count > 0)
			{
				var size = Manager.Game.Foods.Count;
				System.Random random = new System.Random();
				var selectedindex = random.Next(0, size);
				Vector3 selectedPos = Manager.Game.Foods[selectedindex].Position;
				GameObject.Destroy(Manager.Game.Foods[selectedindex].Object);
				spawn_pos = new Vector3(selectedPos.x, random_size / 2, selectedPos.z);
			}
			else
				spawn_pos = new Vector3(0, random_size / 2, 0);
		}
		var my_player = Instantiate(player, spawn_pos, Quaternion.identity, GameObject.Find("InGameObjects").transform);
		if (randomPlayerColor)
			my_player.GetComponent<Renderer>().material.color = Helper.RandomColor();
		my_player.transform.localScale = new Vector3(random_size, random_size, random_size);
	}

}
