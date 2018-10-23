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

    [UnityTest]
    public IEnumerator InputEventStressTests_RunOneGamePadThousandEventType195Frames()
    {
        yield return new MonoBehaviourTest<RunOneGamePadThousandEventType195Frames>();
    }

    public class RunOneGamePadThousandEventType195Frames : MonoBehaviour, IMonoBehaviourTest
    {
        private int frameCount;
        private float sentInputTriggerValue;
        private Gamepad gamePad = null;
        private static int numEvents = 1000;

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
                for (int i = 0; i < numEvents; i++)
                {
                    sentInputTriggerValue = (frameCount + i) / 1000f;
                    InputSystem.QueueStateEvent(gamePad, new GamepadState {rightTrigger = sentInputTriggerValue},
                        InputRuntime.s_Instance.currentTime);
                }
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

    [UnityTest]
    public IEnumerator InputEventStressTests_RunFiveHundredGamePadsThousandEventTypes195Frames()
    {
        yield return new MonoBehaviourTest<RunFiveHundredGamePadsThousandEventTypes195Frames>();
    }

    public class RunFiveHundredGamePadsThousandEventTypes195Frames : MonoBehaviour, IMonoBehaviourTest
    {
        private static int numDevices = 500;
        private static int numEvents = 1000;
        private int frameCount;
        private Gamepad gamePad = null;

        private float[] sentTriggerValues = new float[numDevices];
        private Gamepad[] gamePadDevices = new Gamepad[numDevices];

        public bool IsTestFinished
        {
            get { return frameCount > 195; }
        }

        void OnEnable()
        {
            for (int i = 0; i < numDevices; i++)
            {
                gamePadDevices[i] = InputSystem.AddDevice<Gamepad>();
            }
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
            for (int i = 0; i < numDevices; i++)
            {
                InputSystem.RemoveDevice(gamePadDevices[i]);
            }
        }

        private void GenerateInput()
        {
            if (gamePad != null)
            {
                for (int deviceIndex = 0; deviceIndex < numDevices; deviceIndex++)
                {
                    for (int eventIndex = 0; eventIndex < 1000; eventIndex++)
                    {
                        sentTriggerValues[deviceIndex] = (frameCount + eventIndex + deviceIndex) / 1000f;
                        InputSystem.QueueStateEvent(gamePad, new GamepadState {rightTrigger = sentTriggerValues[deviceIndex]},
                            InputRuntime.s_Instance.currentTime);
                    }
                }
            }
        }

        private void ReadInput()
        {
            if (gamePad != null)
            {
                for (int deviceIndex = 0; deviceIndex < numDevices; deviceIndex++)
                {
                    Assert.That(gamePad.rightTrigger.ReadValue(), Is.EqualTo(sentTriggerValues[deviceIndex]).Within(0.000001));
                }
            }
        }
    }
}
#endif // UNITY_EDITOR || DEVELOPMENT_BUILD
