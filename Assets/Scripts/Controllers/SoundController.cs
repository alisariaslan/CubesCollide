using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
	[Header("Game Objects")]
	public GameObject musicObject;
	public GameObject audioObject;

	[Header("Music Files")]
	public AudioClip menuMusic;
	public AudioClip ingameMusic;

	[Header("Sound Settings")]
	public bool music = true;
	public bool audiom = true;

	private AudioSource musicSource;
	private AudioSource audioSource;
	private Animator soundAnimator;

	private void Start()
	{
		Manager.Game.Sound.Object = this.gameObject;
		musicSource = transform.GetChild(0).GetComponent<AudioSource>();
		audioSource = transform.GetChild(1).GetComponent<AudioSource>();
		soundAnimator = GetComponent<Animator>();
	}

	// Start is called before the first frame update
	public void ToggleAudio()
	{
		if (audiom)
		{
			audiom = false;
			var text = audioObject.GetComponentInChildren<Text>();
			text.text = "Audio Off";
			var closed = audioObject.transform.GetChild(1);
			closed.gameObject.SetActive(true);
			audioSource.volume = 0;
		}
		else
		{
			audiom = true;
			var text = audioObject.GetComponentInChildren<Text>();
			text.text = "Audio On";
			var closed = audioObject.transform.GetChild(1);
			closed.gameObject.SetActive(false);
			audioSource.volume = 1;
		}
	}

	// Update is called once per frame
	public void ToggleMusic()
	{
		if (music)
		{
			music = false;
			var text = musicObject.GetComponentInChildren<Text>();
			text.text = "Music Off";
			var closed = musicObject.transform.GetChild(1);
			closed.gameObject.SetActive(true);
			soundAnimator.Play("MusicOff");
		}
		else
		{
			music = true;
			var text = musicObject.GetComponentInChildren<Text>();
			text.text = "Music On";
			var closed = musicObject.transform.GetChild(1);
			closed.gameObject.SetActive(false);
			soundAnimator.Play("MusicOn");
		}
	}

	private async Task MusicFadeOut()
	{
		soundAnimator.Play("MusicFadeOut");
		await Task.Delay(1000);
	}

	private async Task MusicFadeIn()
	{
		musicSource.Play();
		soundAnimator.Play("MusicFadeIn");
		await Task.Delay(1000);
	}

	public async void SwitchToIngameMusic()
	{
		await MusicFadeOut();
		musicSource.clip = ingameMusic;
		await MusicFadeIn();
	}

	public async void SwitchToMenuMusic()
	{
		await MusicFadeOut();
		musicSource.clip = menuMusic;
		await MusicFadeIn();
	}

}
