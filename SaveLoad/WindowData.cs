[System.Serializable]
public class WindowData
{
    public bool[] isShown = new bool[10];

    public WindowData(bool[] isShown)
    {
        this.isShown = isShown;
    }
}
