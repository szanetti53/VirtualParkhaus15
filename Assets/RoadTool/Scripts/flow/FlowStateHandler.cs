using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flow
{
	public class FlowStateHandler : Singleton<FlowStateHandler>
	{
		[SerializeField]
		FlowStateData[] stateOrder;

		public FlowStateData CurrentStateData
		{
			get; private set;
		}
		public int CurrentState
		{
			get; private set;
		}

		public bool IsShuttingDown
		{
			get; private set;
		}


		private void Awake()
		{
			IsShuttingDown = false;
		}

		private void Start()
		{
			SetState(0);
		}

		private void OnDestroy()
		{
			IsShuttingDown = true;
		}

		private void OnApplicationQuit()
		{
			IsShuttingDown = true;
		}

		public void NextState(float delay = 1f)
		{
			StartCoroutine(InternalNextState(delay));
		}

		IEnumerator InternalNextState(float delay)
		{
			yield return new WaitForSeconds(delay);
			if (CurrentState + 1 < stateOrder.Length)
			{
				SetState(CurrentState + 1);
			}
			else
			{
				// do something?
			}
		}

		public void BackState(float delay = 1f)
		{
			StartCoroutine(InternalBackState(delay));
		}

		IEnumerator InternalBackState(float delay)
		{
			yield return new WaitForSeconds(delay);
			if (CurrentState + 1 < stateOrder.Length)
			{
				SetState(CurrentState - 1);
			}
			else
			{
				// do something?
			}
		}

		private void SetState(int state)
		{
			if (state == CurrentState && CurrentStateData != null) return;

			if (CurrentStateData != null)
			{
				SetGameObjectsActive(CurrentStateData.deactivateOnStateEnd, false);
			}

			CurrentState = state;
			CurrentStateData = stateOrder[state];

		

			SetGameObjectsActive(CurrentStateData.activateOnStateStart, true);

		}

		private void SetState(FlowStateData state)
		{
			if (state == CurrentStateData) return;

			if (CurrentStateData != null)
			{
				SetGameObjectsActive(CurrentStateData.deactivateOnStateEnd, false);
			}

			CurrentState = -1;
			CurrentStateData = state;

			
			SetGameObjectsActive(CurrentStateData.activateOnStateStart, true);
		}


		private void SetGameObjectsActive(GameObject[] gameObjects, bool active = true)
		{
			foreach (GameObject go in gameObjects)
			{
				go.SetActive(active);

				if (go.GetComponent<IFlowState>() != null)
				{
					if (active)
					{
						go.GetComponent<IFlowState>().StartState();
					}
					else
					{
						go.GetComponent<IFlowState>().EndState();
					}
				}
			}
		}
	}

	[System.Serializable]
	public class FlowStateData
	{
		public string stateName = "";

		[SerializeField]
		public GameObject[] activateOnStateStart;

		[SerializeField]
		public GameObject[] deactivateOnStateEnd;
	}
}

