[System.Serializable]
public class ClipSettings {
    public int columns;
    public int rows;
    public int height;
    public int width;
    public int frameRate;
    public bool canLoop;

    public ClipSettings(int columns, int rows, int height, int width, int frameRate, bool canLoop) {
        this.columns = columns;
        this.rows = rows;
        this.height = height;
        this.width = width;
        this.frameRate = frameRate;
        this.canLoop = canLoop;
    }
}