using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    CinemachineThirdPersonFollow _thirdPerson;

    void Awake()
    {
        _thirdPerson = GetComponent<CinemachineCamera>().GetComponent<CinemachineThirdPersonFollow>();
    }

    // Lock camera to always stay directly behind player
    void Update()
    {
        if (_thirdPerson != null)
        {
            _thirdPerson.CameraDistance = 5f;
        }
    }
}
