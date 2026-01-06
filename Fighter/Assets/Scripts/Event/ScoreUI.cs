using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private GameObject pointPrefab; // The image/icon prefab
    [SerializeField] private Transform container;    // The Horizontal Layout Group transform

    public void OnScoreIncreased()
    {
        if (pointPrefab != null && container != null)
        {
            // Spawn the icon as a child of the layout group
            Instantiate(pointPrefab, container);
        }
    }

    // Optional: Clear UI on game restart
    public void ClearScore()
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
    }
}