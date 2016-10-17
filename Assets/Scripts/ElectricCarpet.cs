using UnityEngine;
using System.Collections;

public class ElectricCarpet : Defense
{
	public GameObject projectile;
	public Transform projectileSpawnPoint;

	private float cooldown = 0.0f;
	public float cooldownDuration = 10.0f;

	GUIManager guiManager;

	void Start ()
	{
		guiManager = GameObject.FindObjectOfType<GUIManager> ();
	}

	void Update ()
	{
		if (cooldown > 0.0f)
		{
			cooldown -= Time.deltaTime;
		}
		else
		{
			cooldown = 0.0f;
		}
	}

	public override void OnHoverEnter ()
	{

	}

	public override void OnHoverStay ()
	{
		if (cooldown > 0.0f)
		{
			guiManager.cooldownDisplay.text = "Cooldown: " + Mathf.CeilToInt (cooldown) + "s";

		}
		else
		{
			guiManager.cooldownDisplay.text = "";
		}
	}

	public override void OnHoverExit ()
	{
		guiManager.cooldownDisplay.text = "";
	}

	public override void OnTrigger ()
	{
		if (cooldown <= 0.0f)
		{
			Instantiate (projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
			cooldown = cooldownDuration;
		}
	}
}