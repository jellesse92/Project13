using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureScript : ItemHitTrigger {
    public GameObject coins;
    public int coinAmountAtOpen = 10;
    public int coinAmountAtHit = 5;

    public int hitsToOpen = 3;
    
    Vector3 oldPosition;
    // Use this for initialization
    protected override void ClassSpecificStart()
    {
        coins = Instantiate(coins);
        coins.transform.position = transform.position;
        coins.transform.parent = transform;
        coins.transform.localScale = Vector3.one;
    }
	
    public override void ItemHit()
    {
        PlayShake();

        if (hitsToOpen == 0)
            return;
        else if(hitsToOpen > 1)
            coins.GetComponent<ParticleSystem>().Emit(coinAmountAtHit);
        else if (hitsToOpen == 1)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            GetComponent<BoxCollider2D>().enabled = false;
            coins.GetComponent<ParticleSystem>().Emit(coinAmountAtOpen);
            return;
        }
        hitsToOpen--;
    }
}
