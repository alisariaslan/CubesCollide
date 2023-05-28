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
	public int startRandomMapSize = 2;
	public int endRandomMapSize = 2;

	[Header("Player Settings")]
	public bool randomPlayerSpawn = true;
	public float playerSize = .35f;

	[Header("Bot Settings")]
	public float minBotSize = .35f;
	public float maxBotSize = 15f;
	public int maxBotCount = 1000;
	public int maxXCount = 100;
	public int maxYCount = 100;
	public float XSpacing = 2.5f;
	public float ZSpacing = 2.5f;
	public int botSpawnDelay = -1;
	public int minRandomName = 3;
	public int maxRandomName = 7;

	[Header("Food Settings")]
	public float foodSize = .25f;
	public int foodPass = 2;
	public int foodSpawnDelay = 1;
	public float foodDelayMultiplier = 10;

	private void Start()
	{
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
		do
		{
			if (isFieldsOverLoad)
				chunk--;
			map_border_size = Manager.Game.Ground.Size * chunk;
			map_total_size = Math.Pow(map_border_size, 2);
			map_max_object_count = map_total_size / FindFirstObjectByType<Manager>().maxObjectSize;
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
				float y = (Manager.Game.Ground.Size * 5f - .5f) - rowNow;
				Vector3 pos = new Vector3(x, foodSize / 2, y);
				Points.Add(pos);
				columnNow++;
			}
			rowNow++;
		}
		Manager.Game.Ground.Points = Points;
	}
	public async Task RandomizeGround()
	{
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
		var rnd = new System.Random();
		var randomSize = rnd.Next(startRandomMapSize, endRandomMapSize); Debug.Log("Map Scale: " + randomSize);
		my_ground.transform.localScale = new Vector3(1 * randomSize, 1, 1 * randomSize);
		Manager.Game.Ground.Object = my_ground;
	}

	public async Task GenerateFoods()
	{
		if (!FindFirstObjectByType<Manager>().spawnFoods)
			return;

		await Task.Delay(10);
		float generated = 0;
		List<Food> Foods = new List<Food>();
		int passed = 0;
		foreach (var item in Manager.Game.Ground.Points)
		{
			Food MapFood = new Food();
			if (passed % (foodPass + 1) != 0)
			{
				passed++;
				continue;
			}
			Vector3 spawnPos = new Vector3(item.x, foodSize / 2, item.z);
			if (foodSpawnDelay > 0 && generated > foodDelayMultiplier)
			{
				await Task.Delay(foodSpawnDelay);
				generated = 0;
			}
			var my_food = Instantiate(food, spawnPos, Quaternion.identity, GameObject.Find("FoodCollection").transform);
			if (randomFoodColor)
				my_food.GetComponent<Renderer>().material.color = Helper.RandomColor();
			my_food.transform.localScale = new Vector3(foodSize, foodSize, foodSize);
			MapFood.Object = my_food;
			Foods.Add(MapFood);
			generated++;
			passed++;

		}
		Manager.Game.Foods = Foods;
	}
	public async Task GenerateBots()
	{
		if (!FindFirstObjectByType<Manager>().spawnBots)
			return;

		await Task.Delay(10);
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
		int generated = 0;
		int xCount = 0;
		int yCount = 0;
		while (true)
		{
			while (true)
			{
				float random_size = (float)new System.Random().NextDouble() * (maxBotSize - minBotSize) + minBotSize;
				if (atTheStartX)
				{
					startX = markedX + random_size / 2;
					atTheStartX = false;
				}
				else
				{
					startX += random_size / 2 + lastSize / 2 + XSpacing;
					if (startX + random_size / 2 > endX)
						break;
				}
				startZ = markedZ - random_size / 2;
				Vector3 spawnPos = new Vector3(startX, random_size / 2, startZ);
				if (botSpawnDelay > 0)
					await Task.Delay(botSpawnDelay);
				var my_bot = Instantiate(bot, spawnPos, Quaternion.identity, GameObject.Find("BotCollection").transform);
				if (randomBotColor)
					my_bot.GetComponent<Renderer>().material.color = Helper.RandomColor();
				my_bot.transform.localScale = new Vector3(random_size, random_size, random_size);
				Bot botModel = new Bot() { Name = NameRandomizer.GetRandomName(minRandomName, maxRandomName), Object = my_bot, SpawnPosition = my_bot.transform.position };
				//Debug.Log("Random Name Test:  " + NameRandomizer.GetRandomName(3, 11));
				Bots.Add(botModel);
				lastSize = random_size;
				if (random_size > biggestSize)
					biggestSize = random_size;
				generated++;
				if (maxBotCount < 0 && generated == maxBotCount)
				{
					Manager.Game.Bots = Bots;
					return;
				}
				xCount++;
				if (maxXCount < 0 && xCount == maxXCount)
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
			if (maxYCount < -1 && yCount == maxYCount)
				break;
		}
		Manager.Game.Bots = Bots;

	}

	public async Task SpawnMyPlayer()
	{
		if (!FindFirstObjectByType<Manager>().spawnMyPlayer)
			return;
		await Task.Delay(10);
		var spawn_pos = new Vector3();

		if (Manager.Game.Bots.Count > 0)
		{
			var size = Manager.Game.Bots.Count;
			System.Random random = new System.Random();
			var selectedindex = random.Next(0, size);
			Vector3 selectedPos = Manager.Game.Bots[selectedindex].SpawnPosition;
			GameObject.Destroy(Manager.Game.Bots[selectedindex].Object);
			Manager.Game.Bots.RemoveAt(selectedindex);
			spawn_pos = new Vector3(selectedPos.x, player.transform.localScale.y / 2, selectedPos.z);
		}
		else if (Manager.Game.Foods.Count > 0)
		{
			var size = Manager.Game.Foods.Count;
			System.Random random = new System.Random();
			var selectedindex = random.Next(0, size);
			Vector3 selectedPos = Manager.Game.Foods[selectedindex].Position;
			GameObject.Destroy(Manager.Game.Foods[selectedindex].Object);
			Manager.Game.Foods.RemoveAt(selectedindex);
			spawn_pos = new Vector3(selectedPos.x, player.transform.localScale.y / 2, selectedPos.z);
		}
		else
			spawn_pos = new Vector3(0, player.transform.localScale.y / 2, 0);

		var my_player = Instantiate(player, spawn_pos, Quaternion.identity, GameObject.Find("InGameObjects").transform);
		if (randomPlayerColor)
			my_player.GetComponent<Renderer>().material.color = Helper.RandomColor();
	}

}
