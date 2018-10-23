#if UNITY_EDITOR || DEVELOPMENT_BUILD
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.LowLevel;
using UnityEngine.TestTools;

public class InputEventStressTests
{
    [UnityTest]
    public IEnumerator InputEventStressTests_RunOneGamePadOneEventType195Frames()
    {
        yield return new MonoBehaviourTest<RunOneGamePadOneEventType195Frames>();
    }

    public class RunOneGamePadOneEventType195Frames : MonoBehaviour, IMonoBehaviourTest
    {
        private int frameCount;
        private float sentInputTriggerValue;
        private Gamepad gamePad = null;

        public bool IsTestFinished
        {
            get { return frameCount > 195; }
        }

        void OnEnable()
        {
            gamePad = InputSystem.AddDevice<Gamepad>();
        }

        void Update()
        {
            frameCount++;

            if (frameCount % 2 == 1)
                GenerateInput();
            else
                ReadInput();
        }

        void OnDisable()
        {
            InputSystem.RemoveDevice(gamePad);
        }

        private void GenerateInput()
        {
            if (gamePad != null)
            {
                sentInputTriggerValue = frameCount / 1000f;
                InputSystem.QueueStateEvent(gamePad, new GamepadState {rightTrigger = sentInputTriggerValue }, InputRuntime.s_Instance.currentTime);
            }
        }

        private void ReadInput()
        {
            if (gamePad != null)
            {
                Assert.That(gamePad.rightTrigger.ReadValue(), Is.EqualTo(sentInputTriggerValue).Within(0.000001));
            }
        }
    }
}
#endif // UNITY_EDITOR || DEVELOPMENT_BUILD
