using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Audio Event")]
public class SimpleAudioEvent: AudioEvent
{
    public AudioClip clip;
    public AudioMixerGroup output;
    
    [Space(10)]
    [Range(0,2)] public float volume = 1f;
    
    [Range(0,2)] public float pitch = 1f;
    [Range(0,1)] public float pitchVariance = 0f; 
    public bool usePitchVariance = true;
    
    public bool loop;

    private float GetPitchWithVariance()
    {
        float randomPitch = Random.Range(-pitchVariance, pitchVariance);
        return pitch + randomPitch;
    }

    public override void Play(AudioSource source)
    {
        if (clip == null)
            return;

        source.clip = clip;
        source.outputAudioMixerGroup = output;
        source.volume = volume;
        // Uses base pitch without variance for the override method
        source.pitch = pitch; 
        source.loop = loop;
        source.Play();
    }
    
    public void Play()
    {
        if (usePitchVariance)
            Play(GetPitchWithVariance());
        else
            Play(pitch);
    }

    private void Play(float targetPitch)
    {
        if (clip == null)
            return;
            
        GameObject audioObject = new GameObject("Audio Event: " + name);
        AudioSource source = audioObject.AddComponent<AudioSource>();

        source.clip = clip;
        source.outputAudioMixerGroup = output;
        source.volume = volume;
        source.pitch = targetPitch; 
        source.loop = loop;
        source.Play();

        audioObject.AddComponent<DestroyAudioSourceOnComplete>();
    }
}