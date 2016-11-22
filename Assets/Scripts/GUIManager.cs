using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

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

	[Header ("Setting")]
	public GameObject settingWindow;
	public VolumeSetting masterVolume;
	public VolumeSetting soundVolume;
	public VolumeSetting musicVolume;

	public bool isSettingOpen{
		get{
			return settingWindow.activeSelf;
		}
	}

	[Header ("HUD")]
	public Text roomNameDisplay;
	public Text enemyCountDisplay;
	public Text winLoseDisplay;

	void Awake () {
		if (_instance == null)
			_instance = this;
		else if (instance != this)
			Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		shopPlayerCash.text = "$" + GameManager.instance.cash;
		enemyCountDisplay.text = "Enemies: " + GameManager.instance.enemiesLeft;
	}

	public void ShopOpen () {
		if (player.playerMode == PlayerMode.Activation)
			return;
		
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

	#region setting
	public void SettingOpen(){
		settingWindow.SetActive (true);
		player.enabled = false;
		Time.timeScale = 0.0f;
	}
	public void SettingClose(){
		settingWindow.SetActive (false);
		player.enabled = true;
		masterVolume.OnExit ();
		soundVolume.OnExit ();
		musicVolume.OnExit ();
		Time.timeScale = 1.0f;
	}
	public void SettingToggle(){
		AudioSource[] ar = FindObjectsOfType<AudioSource>();
		if (isSettingOpen) {
			SettingClose ();
			foreach (AudioSource a in ar) {
				a.UnPause ();
			}
		} else {
			SettingOpen ();
			foreach (AudioSource a in ar) {
				a.Pause ();
			}
		}
	}
	#endregion
	// Update the GUI to match the room we are in
	public void SwitchToRoom (Room room) {
		roomNameDisplay.text = room.roomName;
	}

	public void SwitchToOverview () {
		roomNameDisplay.text = "";
	}

	public void StartRaid () {
		ShopClose ();
		shopButton.interactable = false;
		startButton.interactable = false;

		GameManager.instance.StartRaid ();
		BGMManager.instance.Play ();
	}

	public void Win () {
		winLoseDisplay.text = "WIN!";
		winLoseDisplay.color = new Color (0, 255, 0);
	}

	public void Lose () {
		winLoseDisplay.text = "LOSE!";
		winLoseDisplay.color = new Color (255, 0, 0);
	}

	public void RestartLevel () {
		GameManager.instance.Initialize ();
		SceneManager.LoadScene ("GameScene");
	}
}
