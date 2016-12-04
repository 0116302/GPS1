using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GUIManager : MonoBehaviour {

	private static GUIManager _instance;
	public static GUIManager instance {
		get {
			if (_instance == null)
				Debug.LogError ("A script is trying to access the GUIManager which isn't present in this scene!");

			return _instance;
		}
	}

	[Header ("Player")]
	public PlayerController player;
	public Room defaultRoom;

	[Header ("Buttons")]
	public Button startButton;
	public Button shopButton;

	[Header ("UI Sounds")]
	public AudioSource audioSource;
	public AudioClip hoverSound;
	public AudioClip clickSound;
	public AudioClip dingSound;

	[Header ("Shop")]
	public GameObject shopWindow;
	public Text shopPlayerCash;
	public Image shopItemImage;
	public Text shopItemName;
	public Text shopItemDescription;
	public Text shopItemPrice;
	public Text shopItemAOE;
	public Text shopItemDamageType;
	public Text shopItemDamageRate;
	public Text shopItemReusability;
	public Text shopItemCooldown;
	public Text shopItemPlacement;
	GameObject shopSelectedPrefab = null;

	public bool isShopOpen {
		get {
			return shopWindow.activeSelf;
		}
	}

	[Header ("Settings")]
	public GameObject settingsWindow;
	public SegmentedSlider masterVolumeSlider;
	public SegmentedSlider soundVolumeSlider;
	public SegmentedSlider musicVolumeSlider;

	public bool isSettingsOpen {
		get {
			return settingsWindow.activeSelf;
		}
	}

	[Header ("Quit")]
	public GameObject quitWindow;

	public bool isQuitMenuOpen {
		get {
			return quitWindow.activeSelf;
		}
	}

	[Header ("HUD")]
	public Text roomNameDisplay;

	[Header ("Win/Lose")]
	public GameObject winScreen;
	public Text winMessage;
	public Button nextLevelButton;
	public GameObject loseScreen;

	void Awake () {
		if (_instance == null)
			_instance = this;
		else if (instance != this)
			Destroy(gameObject);

		audioSource.ignoreListenerPause = true;
		masterVolumeSlider.onValueChanged.AddListener (OnMasterVolumeChanged);
		soundVolumeSlider.onValueChanged.AddListener (OnSoundVolumeChanged);
		musicVolumeSlider.onValueChanged.AddListener (OnMusicVolumeChanged);
	}

	void Start () {
		masterVolumeSlider.value = SoundManager.instance.GetMasterVolume ();
		soundVolumeSlider.value = SoundManager.instance.GetSoundVolume ();
		musicVolumeSlider.value = SoundManager.instance.GetMusicVolume ();

		shopPlayerCash.text = "$" + LevelManager.instance.cash;
	}

	public void PlayHoverSound () {
		audioSource.PlayOneShot (hoverSound, SoundManager.instance.GetSoundVolume () * SoundManager.instance.GetMasterVolume ());
	}

	public void PlayClickSound () {
		audioSource.PlayOneShot (clickSound, SoundManager.instance.GetSoundVolume () * SoundManager.instance.GetMasterVolume ());
	}

	public void Ding (float volume = 1.0f) {
		audioSource.PlayOneShot (dingSound, volume);
	}

	public void ShopOpen () {
		if (player.playerMode == PlayerMode.Activation)
			return;

		shopPlayerCash.text = "$" + LevelManager.instance.cash;
		shopWindow.SetActive (true);
		player.enabled = false;
	}

	public void ShopClose () {
		shopWindow.SetActive (false);
		player.enabled = true;
	}

	public void ShopToggle () {
		if (isShopOpen) {
			ShopClose ();
		} else {
			ShopOpen ();
		}
	}

	public void ShopSelectItem (ShopEntry entry) {
		shopItemImage.sprite = entry.image;
		shopItemName.text = entry.name;
		shopItemDescription.text = entry.description;
		shopItemPrice.text = "$" + entry.price;
		shopItemAOE.text = entry.areaOfEffect;
		shopItemDamageType.text = entry.damageType;
		shopItemDamageRate.text = entry.damageRate;
		shopItemReusability.text = entry.reusability;
		shopItemCooldown.text = entry.cooldown;
		shopItemPlacement.text = entry.placement;
		shopSelectedPrefab = entry.prefab;
	}

	public void ShopBuy () {
		ShopClose ();

		if (player.cameraMode != CameraMode.RoomView)
			player.SwitchToRoomView (defaultRoom);
		
		player.EnterPlacementMode (shopSelectedPrefab);
	}

	public void ShopSell () {
		ShopClose ();
		player.EnterRemovalMode ();
	}

	public void SettingsOpen () {
		if (isShopOpen) ShopClose ();
		if (isQuitMenuOpen) QuitMenuClose ();

		startButton.interactable = false;
		shopButton.interactable = false;

		settingsWindow.SetActive (true);
		player.enabled = false;
		Game.Pause ();
	}

	public void SettingsClose () {
		startButton.interactable = LevelManager.instance.gamePhase == GamePhase.Setup;
		shopButton.interactable = LevelManager.instance.gamePhase == GamePhase.Setup;

		settingsWindow.SetActive (false);
		player.enabled = true;
		Game.UnPause ();
	}

	public void SettingsToggle () {
		if (isSettingsOpen) {
			SettingsClose ();
		} else {
			SettingsOpen ();
		}
	}

	public void QuitMenuOpen () {
		if (isShopOpen) ShopClose ();
		if (isSettingsOpen) SettingsClose ();

		startButton.interactable = false;
		shopButton.interactable = false;

		quitWindow.SetActive (true);
		player.enabled = false;
		Game.Pause ();
	}

	public void QuitMenuClose () {
		startButton.interactable = LevelManager.instance.gamePhase == GamePhase.Setup;
		shopButton.interactable = LevelManager.instance.gamePhase == GamePhase.Setup;

		quitWindow.SetActive (false);
		player.enabled = true;
		Game.UnPause ();
	}

	public void QuitMenuToggle () {
		if (isQuitMenuOpen) {
			QuitMenuClose ();
		} else {
			QuitMenuOpen ();
		}
	}

	void OnMasterVolumeChanged () {
		SoundManager.instance.SetMasterVolume (masterVolumeSlider.value);
		Ding (masterVolumeSlider.value);
	}

	void OnSoundVolumeChanged () {
		SoundManager.instance.SetSoundVolume (soundVolumeSlider.value);
		Ding (soundVolumeSlider.value * SoundManager.instance.GetMasterVolume ());
	}

	void OnMusicVolumeChanged () {
		SoundManager.instance.SetMusicVolume (musicVolumeSlider.value);
		Ding (musicVolumeSlider.value * SoundManager.instance.GetMasterVolume ());
	}

	public void SwitchToRoom (Room room) {
		// Update the GUI to match the room we are in
		roomNameDisplay.text = room.roomName;
	}

	public void SwitchToOverview () {
		roomNameDisplay.text = "";
	}

	public void StartRaid () {
		ShopClose ();
		shopButton.interactable = false;
		startButton.interactable = false;

		LevelManager.instance.StartRaid ();
	}

	public void Win () {
		winMessage.text = "YOU MADE IT THROUGH DAY " + (LevelLoader.instance.currentLevel + 1).ToString ("D") + " WITH $" + LevelManager.instance.cash.ToString ("D") + " TO SPARE!";

		if (LevelLoader.instance.currentLevel < Game.levelCount - 1) {
			nextLevelButton.interactable = true;
		} else {
			nextLevelButton.interactable = false;
		}

		winScreen.SetActive (true);
		player.enabled = false;
		Game.Pause ();
	}

	public void Lose () {
		loseScreen.SetActive (true);
		player.enabled = false;
		Game.Pause ();
	}

	public void Reload () {
		Game.UnPause ();
		LevelLoader.instance.LoadLevel (LevelLoader.instance.currentLevel);
	}

	public void NextLevel () {
		Game.UnPause ();
		int currentLevel = LevelLoader.instance.currentLevel;
		if (currentLevel < Game.levelCount - 1) {
			LevelLoader.instance.LoadLevel (currentLevel + 1);
		}
	}

	public void Quit () {
		Game.UnPause ();
		SceneManager.LoadScene ("MainMenu");
	}
}
