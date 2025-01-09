using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField] private GenericWindow[] windows;
    public GenericWindow[] Windows { get { return windows; } }
    [SerializeField] private Windows defalultWindow = 0;
    public Windows currentWindow { get; private set; }

    private void Start()
    {
        foreach (var window in windows)
        {
            window.Close();
            window.Init(this);
        }
        currentWindow = defalultWindow;
        windows[(int)defalultWindow].Open();
    }

    public void Open(Windows window)
    {
        windows[(int)currentWindow].Close();
        currentWindow = window;
        windows[(int)currentWindow].Open();
    }

    public void Open(int window)
    {
        windows[(int)currentWindow].Close();
        currentWindow = (Windows)window;
        windows[(int)currentWindow].Open();
    }
}
