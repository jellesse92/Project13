using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerStatusUIScript : MonoBehaviour {

    const float BASE_FILL_AMOUNT = .1f;

    public GameObject[] healthBars;
    public Text[] lifeText;
    public Text[] currencyText;

    private int[] healthLast = new int[4];
    private int[] lastDamage = new int[4];

    void Start()
    {
        for(int i = 0; i < 4; i++)
        {
            healthLast[i] = 100;
            lastDamage[i] = 0;
        }
    }

    //Slowly apply damage to health
    public void ApplyHealthDamage(int index, int damageAmount)
    {
        index -= 1;
        //Jump to last known health amount
        healthBars[index].transform.GetChild(0).GetComponent<Image>().fillAmount = 1.1f - healthLast[index] * .01f;

        StopCoroutine(decreaseHealth(index, lastDamage[index]));

        healthLast[index] -= damageAmount;
        lastDamage[index] = damageAmount;

        StartCoroutine(decreaseHealth(index, damageAmount));
    }

    //Fill bar goes from .1 to .89
    IEnumerator decreaseHealth(int index, int damage)
    {
        for(int i = 0; i < damage; i++)
        {
            healthBars[index].transform.GetChild(0).GetComponent<Image>().fillAmount += .01f;
            yield return new WaitForSeconds(.05f);
        }

    }

    public void SetHealth(int index, int amount)
    {
        healthBars[index].transform.GetChild(index).GetComponent<Image>().fillAmount = .9f - amount * .01f;
    }
}
