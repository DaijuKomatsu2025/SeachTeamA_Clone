using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuSelector : MonoBehaviour
{
    [SerializeField] private GameObject selectorImage; // 選択インジケーターのイメージ
    public Button[] buttons; // インスペクターで2つのボタンを割り当て
    private int selectedIndex = 0;

    public float inputCooldownTime = 0.5f; // クールダウン時間（秒）
    private bool canReceiveInput = true;

    private InputAction _navigate;
    private InputAction _submit;

    void Awake()
    {
        Time.timeScale = 1; // メニューシーンに入るときに時間を正常化
        var inputActionAsset = GetComponent<PlayerInput>().actions;
        _navigate = inputActionAsset.FindAction("Navigate");
        _submit = inputActionAsset.FindAction("Submit");

        if (_navigate != null)
        {
            _navigate.performed += OnNavigate;
            _navigate.Enable();
        }
        if (_submit != null)
        {
            _submit.performed += OnSubmit;
            _submit.Enable();
        }
    }

    void Start()
    {
        UpdateSelection();
    }

    void OnDisable()
    {
        if (_navigate != null)
        {
            _navigate.performed -= OnNavigate;
            _navigate.Disable();
        }
        if (_submit != null)
        {
            _submit.performed -= OnSubmit;
            _submit.Disable();
        }
    }

    void OnNavigate(InputAction.CallbackContext context)
    {
        if (!canReceiveInput) return;

        Vector2 input = context.ReadValue<Vector2>();
        if (input.y == 0) return; // 上下入力以外は無視

        selectedIndex = 1 - selectedIndex;
        UpdateSelection();

        // 入力受付を一時停止
        canReceiveInput = false;
        Invoke(nameof(ResetInput), inputCooldownTime);

        //Debug.Log("OnNavigate called :" + selectedIndex);
    }

    void ResetInput()
    {
        canReceiveInput = true;
    }

    void OnSubmit(InputAction.CallbackContext context)
    {
        if (!canReceiveInput) return;

        Debug.Log("OnSubmit called");
        buttons[selectedIndex].onClick.Invoke();

        // 入力受付を一時停止
        canReceiveInput = false;
        Invoke(nameof(ResetInput), inputCooldownTime);
    }

    void UpdateSelection()
    {
        var pos = buttons[selectedIndex].transform.position;
        selectorImage.transform.position = pos;
        buttons[selectedIndex].Select(); // UI上で選択状態にする
    }

    public void SelectStartButton()
    {
        selectedIndex = 0;
        UpdateSelection();
    }

    public void SelectExitButton() 
    {
        selectedIndex = 1;
        UpdateSelection();
    }
}

