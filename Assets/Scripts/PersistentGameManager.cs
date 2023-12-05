using UnityEngine;

public class PersistentGameManager : MonoBehaviour
{
    private static PersistentGameManager _instance;

    // Properties for persistent data
    private int _points;
    private int _highScore;

    // Public accessor for the instance
    public static PersistentGameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // If the instance doesn't exist, try to find it in the scene
                _instance = FindObjectOfType<PersistentGameManager>();

                // If it still doesn't exist, create a new GameObject and attach the PersistentGameManager script
                if (_instance == null)
                {
                    GameObject go = new GameObject("PersistentGameManager");
                    _instance = go.AddComponent<PersistentGameManager>();
                }

                // Mark the instance to not be destroyed when loading new scenes
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    // Properties for points and high score
    public int Points
    {
        get { return _points; }
        set { _points = value; }
    }

    public int HighScore
    {
        get { return _highScore; }
        set { _highScore = value; }
    }

    // Reset persistent data (useful for restarting the game)
    public void ResetData()
    {
        _points = 0;
        // You may want to keep high score persistent even after restarting the game
    }
}
