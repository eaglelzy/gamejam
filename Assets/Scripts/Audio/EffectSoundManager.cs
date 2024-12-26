using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

[System.Serializable]
class AudioItem
{
    public string name;
    public AudioClip clip;
}
public class EffectSoundManager : MMSingleton<EffectSoundManager>
{
    [Tooltip("“Ù–ß")]
    [SerializeField]
    private List<AudioItem> effectAudioList;

    private AudioSource audioEffect;

    protected override void Awake()
    {
        base.Awake();
        audioEffect = GetComponent<AudioSource>();
    }

    public void PlayEffectSound(string audioName)
    {
        var item = effectAudioList.Find((AudioItem audioItem) => audioItem.name == audioName);
        if (item != null)
        {
            audioEffect.PlayOneShot(item.clip);
        }
    }
}
