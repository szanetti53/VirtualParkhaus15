using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    private AudioSource voice, effects, music;

	[SerializeField]
	private AudioSource positiveTone, negativeTone, hoverClick;

	Coroutine activeCallbackDelay;

	public void PlayVoice(AudioClip clip, System.Action callback = null)
    {
		if(voice == null)
        {
			return;
        }

		StopVoice();

		voice.clip = clip;

		activeCallbackDelay = StartCoroutine(CallbackDelay(clip.length, callback));
		voice.Play();
    }

	public void StopVoice()
    {
		if(voice == null)
        {
			return;
        }
        if (voice.isPlaying)
        {
			voice.Stop();
        }
		if(activeCallbackDelay != null)
        {
			StopCoroutine(activeCallbackDelay);
        }
    }

	public void ClearVoice()
    {
		if(voice.clip == null)
        {
			return;
        }
		voice.Stop();
		voice.clip = null;
    }

	IEnumerator CallbackDelay(float delay, System.Action callback)
	{
		yield return new WaitForSeconds(delay);

		callback?.Invoke();
		activeCallbackDelay = null;
	}

	public void PlaySoundEffect(SoundEffect type = SoundEffect.HoverClick)
	{
		switch (type)
		{
			case SoundEffect.HoverClick:
				if (hoverClick == null) return;
				hoverClick.Play();
				break;
			case SoundEffect.PositiveTone:
				if (positiveTone == null) return;
				positiveTone.Play();
				break;
			case SoundEffect.NegativeTone:
				if (negativeTone == null) return;
				negativeTone.Play();
				break;
		}
	}
}

public enum SoundEffect
{
	HoverClick,
	PositiveTone,
	NegativeTone
}
