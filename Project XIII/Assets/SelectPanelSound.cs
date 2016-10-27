using UnityEngine;
using System.Collections;

public class SelectPanelSound : MonoBehaviour {
    public AudioClip gunner;
    public AudioClip swordsman;
    public AudioClip mech;
    public AudioClip mage;

    private AudioSource audio;
    // Use this for initialization
    void Start () {
        audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void Gunner()
    {
        audio.clip = gunner;
        audio.Play();
    }
    public void Mage()
    {
        audio.clip = mage;
        audio.Play();
    }
    public void Swordsman()
    {
        audio.clip = swordsman;
        audio.Play();
    }
    public void Mech()
    {
        audio.clip = mech;
        audio.Play();
    }
}
