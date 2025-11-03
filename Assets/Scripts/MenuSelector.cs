using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuSelector : MonoBehaviour
{
    [SerializeField] private AudioClip navigateSound; // ナビゲーション音
    [SerializeField] private AudioClip desideSound; // 決定音
    [SerializeField] private GameObject selectorImage; // 選択インジケーターのイメージ
    [SerializeField] private Button[] buttons; // インスペクターでボタンを割り当て
    [SerializeField] private float inputCooldownTime = 0.5f; // クールダウン時間（秒）

    private bool canReceiveInput = true;
    private int selectedIndex = 0;
    private InputAction _navigate;
    private InputAction _submit;
    private bool isDesided = false;

    void Awake()
    {
        Time.timeScale = 1; // メニューシーンに入るときに時間を正常化
        var inputActionAsset = GetComponent<PlayerInput>().actions;
        _navigate = inputActionAsset.FindAction("Navigate");
        _submit = inputActionAsset.FindAction("Submit");
    }

    private void OnEnable()
    {
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

        isDesided = false;
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

        foreach (var button in buttons)
        {
            if (button != null) button.enabled = false;
        }
    }

    void OnNavigate(InputAction.CallbackContext context)
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            string selectedName = EventSystem.current.currentSelectedGameObject.name;
            Debug.Log("現在選択されているUI: " + selectedName);
        }

        if (buttons.Length <= 1) return;
        if (!canReceiveInput) return;

        Vector2 input = context.ReadValue<Vector2>();
        if (input.y == 0) return; // 上下入力以外は無視

        if(buttons.Length == 2)
        {
            selectedIndex = 1 - selectedIndex;
        }

        UpdateSelection();

        if (navigateSound != null)
        {
            SoundManager.Instance.PlaySound(navigateSound);
        }

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
        if (isDesided) { Debug.Log("Submit ignored: already decided"); return; }
        if (!canReceiveInput) { Debug.Log("Submit ignored: cooldown"); return; }

        buttons[selectedIndex].onClick.Invoke();
        Debug.Log("Button click invoked");

        isDesided = true;
        canReceiveInput = false;
        Invoke(nameof(ResetInput), inputCooldownTime);
    }

    void UpdateSelection()
    {
        isDesided = false;
        var pos = buttons[selectedIndex].transform.position;
        selectorImage.transform.position = pos;
        buttons[selectedIndex].Select(); // UI上で選択状態にする
    }

    public void SelectFirstButton()
    {
        selectedIndex = 0;
        UpdateSelection();
    }

    public void SelectLastButton() 
    {
        selectedIndex = buttons.Length - 1;
        UpdateSelection();
    }
}

