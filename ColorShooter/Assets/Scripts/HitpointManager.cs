using UnityEngine;
using System.Collections;

public class HitpointManager : MonoBehaviour
{
    [SerializeField]
    private int curLife;
    public int CurLife { get { return curLife; } }

    private int maxLife;
    public int MaxLife{ get { return maxLife; } }

    public float PercentLife { get { return (float)curLife / (float)maxLife; } }

    [SerializeField]
    private GameObject RootObject;

    void Start()
    {
        if (RootObject == null)
            RootObject = gameObject;

        if (curLife == 0)
        {
            Debug.Log(gameObject.name + " has 0 life upon construction, destroying it!");
            Destroy(RootObject);
        }

        else
        {
            this.maxLife = curLife;
        }
    }

    public void Damage(int damage)
    {
        if(damage != 0)
        {
            this.curLife -= damage;
            //TODO: maybe call on damaged

            if (curLife < 0)
            {
                curLife = 0;
                Destroy(RootObject);
                //TODO: maybe call on destroyed
            }

            else if (curLife > maxLife)
            {
                curLife = maxLife;
            }
        }
    }


    

}
