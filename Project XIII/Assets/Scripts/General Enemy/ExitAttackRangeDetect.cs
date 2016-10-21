using UnityEngine;
using System.Collections;

public class ExitAttackRangeDetect : MonoBehaviour {

	void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
            transform.parent.GetComponent<Enemy>().SetAttackInRange(false);
    }
}
