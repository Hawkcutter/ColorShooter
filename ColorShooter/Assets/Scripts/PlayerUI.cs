using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public Player player;
    private Image panel;
    private Image shipImage;
    public Text bulletSpeed;
    public Text power;
    public Text shot;

	// Use this for initialization
	void Start () {

        
        panel = GetComponentInChildren<Image>();

        if (!player)
        {
            if (panel)
                panel.gameObject.SetActive(false);
        }
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (bulletSpeed)
            bulletSpeed.text = "Bulletspeed: " + (player.mainWeapon as Gun).GetUpgrade(Gun.GunUpgradeType.Speed);

        if (power)
            power.text = "Power: " + (player.mainWeapon as Gun).GetUpgrade(Gun.GunUpgradeType.Damage);

        if (shot)
            shot.text = "Shot: " + (player.mainWeapon as Gun).GetUpgrade(Gun.GunUpgradeType.NumShots);
	}
}
