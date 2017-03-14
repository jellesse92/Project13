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
    public float maxVolume = .1f;
    AudioSource[] aSource;

    int indexUnmute = 0;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);

        //Destroys copy of this on scene
        if (FindObjectsOfType(GetType()).Length > 1)
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
        aSource = new AudioSource[musicArray.Length];

        GameObject child = new GameObject("Death Music");
        child.transform.parent = gameObject.transform;
        aSource[0] = child.AddComponent<AudioSource>() as AudioSource;
        aSource[0].clip = musicArray[0].startClip;
        aSource[0].Stop();

        for (int i = 1; i < musicArray.Length; i++)
        {
            child = new GameObject("Background Music");
            child.transform.parent = gameObject.transform;
            aSource[i] = child.AddComponent<AudioSource>() as AudioSource;
            aSource[i].clip = musicArray[i].startClip;
            aSource[i].loop = true;
            aSource[i].volume = 0;
        }

        for(int i = 1; i < clipsPlayOnStart+1; i++)
            StartCoroutine(StartClips(1));

        nextClip = clipsPlayOnStart + 1;
        ZeroVolumeClips();

	}

    IEnumerator StartClips(int index)
    {

        StartCoroutine(RaiseVolume(index));
        aSource[index].Play();
        yield return new WaitForSeconds(musicArray[index].startClip.length);
        aSource[index].clip = musicArray[index].loopClip;
        aSource[index].Play();
    }

    //Zero the volume for all clips except the first
    void ZeroVolumeClips()
    {
        aSource[0].volume = 0;
        for(int i = clipsPlayOnStart+1; i < aSource.Length; i++)
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
            while(aSource[index].volume < maxVolume)
            {
                aSource[index].volume += INCREASE_VOLUME_RATE;
                yield return new WaitForSeconds(1f);
            }
    }

    public void PlayDeathMusic()
    {
        clipsPlayOnStart = 0;
        ZeroVolumeClips();
        aSource[0].Play();
        StartCoroutine(RaiseVolume(0));
    }

    public void SetMusicVolume(float vol)
    {
        maxVolume = vol;
        if(aSource != null)
            aSource[nextClip - 1].volume = vol;
    }
}
