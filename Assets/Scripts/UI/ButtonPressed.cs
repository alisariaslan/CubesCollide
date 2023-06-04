using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonPressed : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public List<GameObject> gameObjects;
	public List<Image> images;
	public bool isCamera;
	public bool isMovement;
	public bool isOto;
	public bool isPause;
	public Vector2 direction = Vector2.zero;

	void Start()
	{
		if(isOto)
		{
			Manager.Game.General.OtoButton = this.gameObject;
		}
		if (isPause)
		{
			Manager.Game.General.PauseButton = this.gameObject;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (Manager.Game.Player.Object == null)
		{
			Debug.Log("Player NULL!");
			return;
		}

		if (isCamera)
		{
			if (direction == Vector2.zero)
			{
				Manager.Game.Player.Controller.SetCompassFromHere(KeyCode.R);
			}
			if (direction == Vector2.up)
			{
				Manager.Game.Player.Controller.SetCompassFromHere(KeyCode.UpArrow);
			}
			if (direction == Vector2.left)
			{
				Manager.Game.Player.Controller.SetCompassFromHere(KeyCode.LeftArrow);
			}
			if (direction == Vector2.right)
			{
				Manager.Game.Player.Controller.SetCompassFromHere(KeyCode.RightArrow);
			}
		}

		if (isMovement)
		{
			if (direction == Vector2.zero)
			{
				Manager.Game.Player.Controller.SetDirectionFromHere(KeyCode.Space);
			}
			if (direction == Vector2.up)
			{
				Manager.Game.Player.Controller.SetDirectionFromHere(KeyCode.W);
			}
			if (direction == Vector2.left)
			{
				Manager.Game.Player.Controller.SetDirectionFromHere(KeyCode.A);
			}
			if (direction == Vector2.down)
			{
				Manager.Game.Player.Controller.SetDirectionFromHere(KeyCode.S);
			}
			if (direction == Vector2.right)
			{
				Manager.Game.Player.Controller.SetDirectionFromHere(KeyCode.D);
			}
		}

		if (isOto)
			ToggleOto();

		if (isPause)
			TogglePause();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		//EMPTY
	}

	public void CheckOto()
	{
		if (Manager.Game.Player.IsOtoEnabled)
		{
			foreach (var item in gameObjects)
				item.SetActive(false);
			transform.GetChild(0).GetComponent<Image>().color = Color.green;
		}
		else
		{
			foreach (var item in gameObjects)
				item.SetActive(true);
			transform.GetChild(0).GetComponent<Image>().color = Color.white;
		}
	}

	public void ToggleOto()
	{
		if (Manager.Game.Player.Object == null)
		{
			Debug.Log("Player NULL!");
			return;
		}

		if (Manager.Game.Player.IsOtoEnabled)
		{
			_ = Manager.Game.Chat.Controller.Text_Line("OTO Camera Disabled.");
			Manager.Game.Player.IsOtoEnabled = false;
		}
		else
		{
			_=Manager.Game.Chat.Controller.Text_Line("OTO Camera Enabled.");
			Manager.Game.Player.IsOtoEnabled = true;
		}

		CheckOto();
	}

	public void TogglePause()
	{
		FindFirstObjectByType<Manager>().Pause();
	}


}
