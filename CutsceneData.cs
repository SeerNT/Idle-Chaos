[System.Serializable]
public class CutsceneData
{
    public bool[] shownCutscenes;
    public bool isShowing;

    public CutsceneData(bool[] shownCutscenes, bool isShowing)
    {
        this.shownCutscenes = shownCutscenes;
        this.isShowing = isShowing;
    }
}

