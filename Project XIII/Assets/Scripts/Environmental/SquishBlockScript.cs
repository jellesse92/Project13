using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class SquishBlockScript : MonoBehaviour {

    //Heirarchy objects
    public Transform destination;               //Destination location
    public Transform block;                     //Moving block
    public GameObject killZone;                 //Kill zone for block while it's moving towards its destination
    GameObject cam;                             //Game camera

    Vector3 origin;                             //Origin position of block to return to

    //Adjustable variables for balance
    public float moveSpeed;                     //Speed which block moves "down" towards destination
    public float returnSpeed;                   //Speed which block returns to origin location
    public float returnDelay;                   //Delay before initiating return
    public float magShake = 1f;
    public float durShake = 0.7f;

    //Determine if block is moving towards destination or returning to origin
    bool moving = false;
    bool returning = false;

    bool isVisible = false;

    //Audio
    AudioSource myAudio;
    public AudioClip impact;

    //Particle
    GravityRockFragment gravityRockFragments;

    // Use this for initialization
    void Start () {
        myAudio = GetComponent<AudioSource>();
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        origin = block.position;
        GetComponent<BlockParticleEffects>().ChangeParticlePosition(ref GetComponent<BlockParticleEffects>().gravityRockFragment, destination.position);
        gravityRockFragments = GetComponent<BlockParticleEffects>().gravityRockFragment.GetComponent<GravityRockFragment>();
    }

    void FixedUpdate()
    {
        if (moving)
            MoveTowardsDestination();
        else if (returning)
            ReturnToOrigin();
    }

    public void TriggerMove()
    {
        if(!returning && isVisible)
            moving = true;
    }

    void MoveTowardsDestination()
    {
        block.position = Vector2.MoveTowards(block.position, destination.position, moveSpeed);

        if (block.position.x == destination.position.x && block.position.y == destination.position.y)
        {
            moving = false;
            playSound(impact);
            killZone.SetActive(false);
            gravityRockFragments.TurnForceOverTime(true);
            cam.GetComponent<CamShakeScript>().StartShake(magShake, durShake);
            Invoke("DelayReturn", returnDelay);
        }
    }

    void ReturnToOrigin()
    {
        returning = true;
        block.position = Vector2.MoveTowards(block.position, origin, returnSpeed);
        if (block.position == origin)
        {
            gravityRockFragments.TurnForceOverTime(false);
            returning = false;
        }

    }

    void DelayReturn()
    {
        returning = true;
    }
	
    //Function to run when sprite is visible
    public void VisibleFunc()
    {
        this.GetComponent<SquishBlockScript>().enabled = true;
        if(!moving)
            block.position = origin;
        isVisible = true;
    }

    //Function to run when sprite is invis
    public void InvisFunc()
    {
        this.GetComponent<SquishBlockScript>().enabled = false;
        block.position = origin;
        moving = false;
        returning = false;
        isVisible = false;
    }

    public void playSound(AudioClip clip)
    {
        myAudio.PlayOneShot(clip);
    }
}
