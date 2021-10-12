using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Flow
{
    public class FlowBaseState : MonoBehaviour, IFlowState
    {
        [SerializeField]
        public UnityEvent OnStartState;

        [SerializeField]
        public UnityEvent OnEndState;

        [SerializeField]
        private AudioClip audioClip;

        [SerializeField]
        private float audioDelay = 3f;

        [SerializeField]
        private float nextStateDelay = 1f;

        public bool movingToNextState = false;

        public bool movingToBackState = false;

        public virtual void NextState()
        {
            if (!movingToNextState)
            {
                FlowStateHandler.Instance.NextState(nextStateDelay);
            }

            movingToNextState = true;
        }

        public virtual void BackState()
        {
            if (!movingToBackState)
            {
                FlowStateHandler.Instance.BackState(nextStateDelay);
            }

            movingToBackState = true;
        }

        public virtual void StartState(bool force = false)
        {
            OnStartState?.Invoke();

            if (audioClip)
            {
                StartCoroutine(PlayAudioDelay(audioClip, audioDelay));
            }
            else
            {
                AudioManager.Instance.ClearVoice();
            }
            
        }

        private IEnumerator PlayAudioDelay(AudioClip clip, float delay)
        {
            yield return new WaitForSeconds(delay);

            AudioManager.Instance.PlayVoice(clip);
        }

        public virtual void EndState(bool force = false)
        {
            OnEndState?.Invoke();
            StopAllCoroutines();
            if (audioClip)
            {
                AudioManager.Instance.StopVoice();
            }

            movingToNextState = false;
            movingToBackState = false;
        }
    }
}

