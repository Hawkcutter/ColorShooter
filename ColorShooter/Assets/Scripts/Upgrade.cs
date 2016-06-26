using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class Upgrade : MonoBehaviour 
{
    public enum UpgradeType { AttackSpeed, AttackPower, AttackNumber,       Count }

    public UpgradeType Type;

    public bool Randomize;

    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite attackDamageSprite;
    [SerializeField]
    private Sprite attackSpeedSprite;
    [SerializeField]
    private Sprite attackNumberSprite;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (Randomize)
            Type = (UpgradeType)(Random.Range(0, (int)UpgradeType.Count));

        if (Type == UpgradeType.AttackSpeed)
        {
            spriteRenderer.sprite = attackSpeedSprite;
        }

        else if (Type == UpgradeType.AttackPower)
        {
            spriteRenderer.sprite = attackDamageSprite;
        }

        else if (Type == UpgradeType.AttackNumber)
        {
            spriteRenderer.sprite = attackNumberSprite;
        }

        else
        {
            Debug.Log("unknwon upgrade type, destroying: " + gameObject.name);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();

        if(player)
        {
            if (Type == UpgradeType.AttackSpeed)
            {
                player.mainWeapon.UpgradeAttackSpeed();
            }

            else if (Type == UpgradeType.AttackPower)
            {
                player.mainWeapon.UpgradeAttackDamage();
            }

            else if (Type == UpgradeType.AttackNumber)
            {
                player.mainWeapon.UpgradeNumAttacks();
            }


            Destroy(gameObject);
        }
    }
}
