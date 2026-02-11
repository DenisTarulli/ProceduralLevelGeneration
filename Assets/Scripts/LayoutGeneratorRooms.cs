using UnityEngine;

public class LayoutGeneratorRooms : MonoBehaviour
{
    [SerializeField] private int levelWidth = 64;
    [SerializeField] private int levelLength = 64;

    [SerializeField, Tooltip("Inclusive")] private int roomWidthMind = 3;
    [SerializeField, Tooltip("Exclusive")] private int roomWidthMax = 5;
    [SerializeField, Tooltip("Inclusive")] private int roomLengthMin = 3;
    [SerializeField, Tooltip("Exclusive")] private int roomLengthMax = 5;

    [SerializeField] private GameObject levelLayoutDisplay;

    System.Random random;

    [ContextMenu("Generate Level Layout")]
    public void GenerateLevel()
    {
        random = new System.Random();
        var roomRect = GetStartRoomRect();
        Debug.Log(roomRect);
        DrawLayout(roomRect);
    }

    /// <summary>
    /// Generates a room (RectInt) of random position X & Y,
    /// with random dimensions of width & length
    /// </summary>
    /// <returns>Room Rectangle</returns>
    private RectInt GetStartRoomRect()
    {
        // The available space rect for the room to be located in 
        // is determined by dividing the levelWidth and levelLength by half
        // and then adding quarter of each value (width and length) to
        // the position of the new room. This way, we assure that the new room
        // will always be located in an inner rect that is centered and is half
        // the level layout size
        // Visual ref: https://imgur.com/a/EpNcr2F

        // Generate random width
        int roomWidth = random.Next(roomWidthMind, roomWidthMax);
        // Width available in the inner rect after substracting the new room width
        int availableWidthX = levelWidth / 2 - roomWidth;
        // Random x position of the new room
        int randomX = random.Next(0, availableWidthX);
        // Add the quarter of level width offset so the new room position
        // is based of the inner rect position (center of the level)
        int roomX = randomX + (levelWidth / 4);

        // We do the same procedure with the new room length
        int roomLength = random.Next(roomLengthMin, roomLengthMax);
        int availableLengthY = levelLength / 2 - roomLength;
        int randomY = random.Next(0, availableLengthY);
        int roomY = randomY + (levelLength / 4);

        return new RectInt(roomX, roomY, roomWidth, roomLength);
    }

    private void DrawLayout(RectInt roomCandidateRect = new RectInt())
    {
        var renderer = levelLayoutDisplay.GetComponent<Renderer>();

        var layoutTexture = (Texture2D)renderer.sharedMaterial.mainTexture;

        layoutTexture.Reinitialize(levelWidth, levelLength);
        levelLayoutDisplay.transform.localScale = new Vector3(levelWidth, levelLength, 1f);
        layoutTexture.FillWithColor(Color.black);
        layoutTexture.DrawRectangle(roomCandidateRect, Color.cyan);
        layoutTexture.SaveAsset();
    }
}
