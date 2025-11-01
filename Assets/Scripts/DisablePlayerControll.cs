using UnityEngine;

public class DisablePlayerControll : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;

    private void OnEnable()
    {
        _playerController.GetComponent<Animator>().SetFloat("MoveSpeed", 0);
        _playerController.enabled = false;
    }
}
