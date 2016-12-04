using UnityEngine;
using System.Collections;

public class ShopEntry : MonoBehaviour {

	public bool selectedByDefault = false;

	public Sprite image;
	public new string name;
	[TextArea (3, 5)]
	public string description;
	public int price;
	public string areaOfEffect;
	public string damageType;
	public string damageRate;
	public string reusability;
	public string cooldown;
	public string placement;

	public GameObject prefab;

	public void Start () {
		if (selectedByDefault) {
			GUIManager.instance.ShopSelectItem (this);
		}
	}

}
