using UnityEngine;
public class GroundInit : MonoBehaviour
{
	void Start()
	{
		Manager.Game.Ground.Object = this.gameObject;
	}
}