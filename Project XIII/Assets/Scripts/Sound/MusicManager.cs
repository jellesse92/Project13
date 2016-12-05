using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

    const float INCREASE_VOLUME_RATE = .05f;                //Rate at which volume increases

    [System.Serializable]
    public class MusicEntry
    {
        public AudioClip startClip;
        public AudioClip loopClip;
    }

    public MusicEntry[] musicArray;
    public int clipsPlayOnStart;                            //Number of clips to play on start
    private int nextClip;                                   //Next clip to activate
    AudioSource[] aSource;

    int indexUnmute = 0;

    // Use this for initialization
    void Start () {
        aSource = new AudioSource[musicArray.Length];

        for(int i = 0; i < musicArray.Length; i++)
        {
            GameObject child = new GameObject("Background Music");
            child.transform.parent = gameObject.transform;
            aSource[i] = child.AddComponent<AudioSource>() as AudioSource;
            aSource[i].clip = musicArray[i].startClip;
            aSource[i].loop = true;
        }

        for(int i = 0; i < musicArray.Length; i++)
            StartCoroutine(StartClips(i));

        nextClip = clipsPlayOnStart;
        ZeroVolumeClips();

	}

    IEnumerator StartClips(int index)
    {
        aSource[index].Play();
        yield return new WaitForSeconds(musicArray[index].startClip.length);
        aSource[index].clip = musicArray[index].loopClip;
        aSource[index].Play();
    }

    //Zero the volume for all clips except the first
    void ZeroVolumeClips()
    {
        for(int i = clipsPlayOnStart; i < aSource.Length; i++)
        {
            aSource[i].volume = 0;
        }
    }

    public void ActivateNextClip()
    {
        if(nextClip <= aSource.Length)
        {
            StartCoroutine(RaiseVolume(nextClip));
            nextClip++;
        }

    }

    IEnumerator RaiseVolume(int index)
    {
        if(aSource.Length > index)
            while(aSource[index].volume < 1f)
            {
                aSource[index].volume += INCREASE_VOLUME_RATE;
                yield return new WaitForSeconds(1f);
            }

    }

}
