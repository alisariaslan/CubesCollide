using UnityEngine;

public class CanvasController : MonoBehaviour
{
	public GameObject mobileControllers;
	public GameObject pcControllers;
	public GameObject fixedControllers;

	// Start is called before the first frame update
	public void DisplayMobileUI()
    {
		if(mobileControllers.activeSelf)
			mobileControllers.SetActive(false);
		else
			mobileControllers.SetActive(true);
		DisplayFixedControllers();
	}

    // Update is called once per frame
    public void DisplayDesktopUI()
    {
		if (pcControllers.activeSelf)
			pcControllers.SetActive(false);
		else
			pcControllers.SetActive(true);
		DisplayFixedControllers();
	}

	private void DisplayFixedControllers()
	{
		if(fixedControllers.activeSelf)
			fixedControllers.SetActive(false);
		else fixedControllers.SetActive(true);
	}
}
