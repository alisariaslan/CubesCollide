using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
	[Header("Game Objects")]
	public GameObject musicObject;
	public GameObject soundObject;

	[Header("Music Files")]
	public AudioClip menuMusic;
	public AudioClip ingameMusic;

	[Header("Sound Files")]
	public AudioClip playerHitsWall;
	public AudioClip playerHitsBot;
	public AudioClip eatCube;
	public AudioClip eatFood;
	public AudioClip meDead;
	public AudioClip meWin;

	[Header("Settings")]
	public bool musicEnabled = true;
	public bool soundEnabled = true;

	private AudioSource musicSource;
	private AudioSource soundSource;
	private Animator animator;

	void Start()
	{
		Manager.Game.Audio.Music = this.gameObject;
		Manager.Game.Audio.Sound = transform.GetChild(1).gameObject;

		musicSource = transform.GetChild(0).GetComponent<AudioSource>();
		soundSource = transform.GetChild(1).GetComponent<AudioSource>();
		animator = GetComponent<Animator>();
	}

	private void MuteMusic()
	{
		musicEnabled = false;
		var text = musicObject.GetComponentInChildren<Text>();
		text.text = "Music Off";
		var closed = musicObject.transform.GetChild(1);
		closed.gameObject.SetActive(true);
		animator.Play("MusicOff");
	}
	private void UnMuteMusic()
	{
		musicEnabled = true;
		var text = musicObject.GetComponentInChildren<Text>();
		text.text = "Music On";
		var closed = musicObject.transform.GetChild(1);
		closed.gameObject.SetActive(false);
		animator.Play("MusicOn");
	}
	public void StopMusic()
	{
		if (musicEnabled)
			animator.Play("MusicOff");
	}
	public void PlayMusic()
	{
		if (musicEnabled)
			animator.Play("MusicOn");
	}
	public void ToggleMusic()
	{
		if (musicEnabled)
			MuteMusic();
		else
			UnMuteMusic();
	}
	private void MuteSound()
	{
		soundEnabled = false;
		var text = soundObject.GetComponentInChildren<Text>();
		text.text = "Sound Off";
		var closed = soundObject.transform.GetChild(1);
		closed.gameObject.SetActive(true);
		soundSource.volume = 0;
	}
	private void UnMuteSound()
	{
		soundEnabled = true;
		var text = soundObject.GetComponentInChildren<Text>();
		text.text = "Sound On";
		var closed = soundObject.transform.GetChild(1);
		closed.gameObject.SetActive(false);
		soundSource.volume = 1;
	}
	public void StopSound()
	{
		if (soundEnabled)
			animator.Play("MusicOff");
	}
	public void PlaySound()
	{
		if (soundEnabled)
			animator.Play("MusicOn");
	}
	public void ToggleSound()
	{
		if (soundEnabled)
			MuteSound();
		else
			UnMuteSound();
	}

	public async Task MusicFadeOut()
	{
		animator.Play("MusicFadeOut");
		await Task.Delay(1000);
		musicSource.Stop();
	}

	public async Task MusicFadeIn()
	{
		musicSource.Play();
		animator.Play("MusicFadeIn");
		await Task.Delay(1000);
	}

	public async void SwitchToIngameMusic()
	{
		if (!musicEnabled)
			return;
		await MusicFadeOut();
		musicSource.clip = ingameMusic;
		await MusicFadeIn();
	}

	public async void SwitchToMenuMusic()
	{
		if (!musicEnabled)
			return;
		await MusicFadeOut();
		musicSource.clip = menuMusic;
		await MusicFadeIn();
	}

}
