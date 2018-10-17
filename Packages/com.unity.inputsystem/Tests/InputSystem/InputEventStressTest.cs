#if UNITY_EDITOR || DEVELOPMENT_BUILD
using NUnit.Framework;
using System;
using System.Collections;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.Controls;
using UnityEngine.Experimental.Input.Layouts;
using UnityEngine.Experimental.Input.LowLevel;
using UnityEngine.Experimental.Input.Utilities;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.TestTools;

public class InputEventStressTest
{
    [UnityTest]
    public IEnumerator GameObject_WithRigidBody_WillBeAffectedByPhysics()
    {
        InputDevice[] deviceArray = new InputDevice[2];
 
        for (int i = 0; i < 2; i++)
        {
            deviceArray[i] = InputSystem.AddDevice<Gamepad>();
        }

        // run 500 frames
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                var value = (i + j + 190) / 1000f;
                InputSystem.QueueStateEvent(deviceArray[j], new GamepadState { rightTrigger = value }, InputRuntime.s_Instance.currentTime);
            }
            yield return new WaitForEndOfFrame();

            for (int j = 0; j < 2; j++)
            {
                var value = (i + j + 190) / 1000f;
                Assert.That(((Gamepad)deviceArray[j]).rightTrigger.ReadValue(), Is.EqualTo(value).Within(0.000001));
            }
        }
    }
}
#endif // UNITY_EDITOR || DEVELOPMENT_BUILD
