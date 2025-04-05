[System.Serializable]
public class CutsceneData
{
    public bool[] shownCutscenes;
    public bool isShowing;
    public bool isEndingShown;

    public CutsceneData(bool[] shownCutscenes, bool isShowing)
    {
        this.shownCutscenes = shownCutscenes;
        this.isShowing = isShowing;
    }

    public CutsceneData(bool[] shownCutscenes, bool isShowing, bool isEndingShown)
    {
        this.shownCutscenes = shownCutscenes;
        this.isShowing = isShowing;
        this.isEndingShown = isEndingShown;
    }
}

