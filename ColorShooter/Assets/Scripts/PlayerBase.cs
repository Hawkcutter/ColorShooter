using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerBase : MonoBehaviour 
{


    [SerializeField]
    private Image baseDamagedEffect;

    [SerializeField]
    private int lifes;
    public int Lifes { get { return lifes; } }

    [SerializeField]
    private float damagedEffectDuration;
    private float curDamagedEffectDuration;

    public bool Indestructible = false;

    public void SetLifes(int amount)
    {
        this.lifes = amount;

        GameManager.Instance.OnBaseLifeChanged();
    }

    void Update()
    {
        if (curDamagedEffectDuration > 0.0f)
        {
            OnEffectUpdate();
        }

    }

    void OnEffectUpdate()
    {
        curDamagedEffectDuration -= Time.deltaTime;

        float t = curDamagedEffectDuration / damagedEffectDuration;

        t = Mathf.Sin(t * Mathf.PI);

        t = Mathf.Clamp(t, 0.0f, 1.0f);

        baseDamagedEffect.color = new Color(1.0f, 1.0f, 1.0f, t);

        if (curDamagedEffectDuration <= 0.0f)
            OnEffectEnd();
    }

    void OnEffectEnd()
    {
        baseDamagedEffect.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    void OnEffectStart()
    {
     
        curDamagedEffectDuration = damagedEffectDuration;
    }



    void OnTriggerEnter2D(Collider2D collider)
    {
        Enemy enemy = collider.GetComponent<Enemy>();

        if (enemy && enemy.BaseDmg != 0)
        {
            if (!Indestructible)
            {
                lifes -= enemy.BaseDmg;

                GameManager.Instance.OnBaseLifeChanged();

                OnEffectStart();
            }

            if (lifes <= 0)
            {
                lifes = 0;
                GameManager.Instance.Reset();
            }

            Destroy(enemy.gameObject);
        }
    }
}
