using UnityEngine;

namespace Assets.Scripts.Models
{
	public class Calculation
	{
		/// <summary>
		/// When you hit a another player, there is a DESTROY CALCULATION for when you or him will destroy.
		/// Calculation uses a divider. If math:"target/x" bigger than you you will destroy. Or Vice Versa.
		/// </summary>
		public float DestroyDivider = 2;

		/// <summary>
		/// When you hit a another player, if destroy calculations will not be enough,than one player will shrink down LOWEST.
		/// Players will shrink by x%
		/// </summary>
		public float ShrinkFromCollide = 1;

		/// <summary>
		/// When you hit a another player, if destroy calculations will not be enough than, one player will shrink down HIGHEST.
		/// Players will shrink by x%
		/// </summary>
		public float ShrinkFromPlayer = 10;

		/// <summary>
		/// When your player smaller than food, player need to collide with food to shrink it.
		///	So the X is how much x% will shrink.
		/// </summary>
		public float ShrinkFood = 25;

		/// <summary>
		/// Your Player Grows By Foods x%
		/// </summary>
		public float GrowFromFood = 10;

		/// <summary>
		/// Your Player Grows By Players x%
		/// </summary>
		public float GrowFromPlayer = 5;

		/// <summary>
		/// Your player speed over scale will increase if you increase this value.
		/// </summary>
		public float PlayerSpeedFixer = 10;

		public bool Calculation_Destroy(float targetSize, float mySize)
		{
			if (Manager.Game.General.SelectedGameMode == GameMode.BETHEBIGGEST)
			{
				if (targetSize <= mySize)
					return true;
				else return false;
			}
			
			if (targetSize < mySize / DestroyDivider)
				return true;
			else return false;
		}

		public float Calculation_GrowFromFood(float targetSize, float mySize)
		{
			float total = targetSize / (mySize * GrowFromFood);
			return total;
		}

		public float Calculation_GrowFromPlayer(float targetSize, float mySize)
		{
			if (Manager.Game.General.SelectedGameMode == GameMode.BETHEBIGGEST)
			{
				return targetSize + 0.15f;
			}

			//+= CHANGED WILL NEED TO FIX LATER
			float total = targetSize / (mySize * GrowFromPlayer);
			return total;
		}

		public float Calculation_Set_Speed(float maxSpeed, float mySize)
		{
			float total = maxSpeed - (mySize / PlayerSpeedFixer);
			return total;
		}

		/// <summary>
		/// USES NONE VARIABLE
		/// </summary>
		/// <param name="scale"></param>
		/// <returns></returns>
		public void Calculation_Decrease_Scale(Transform transform)
		{
			var scale = transform.localScale.y;
			var newscale = scale;
			if (scale < 1)
			{
				float decreasement = 0.01f;
				newscale -= decreasement;
				Manager.Game.General.Counters.UpdateScale();
				transform.localScale = new Vector3(newscale, newscale, newscale);
				transform.position = new Vector3(transform.position.x, transform.position.y - decreasement / 2, transform.position.z);
			}
			else
			{
				float decreasement = scale / 100f;
				newscale -= decreasement;
				Manager.Game.General.Counters.UpdateScale();
				transform.localScale = new Vector3(newscale, newscale, newscale);
				transform.position = new Vector3(transform.position.x, transform.position.y - decreasement / 2, transform.position.z);
			}
		}
	}
}
