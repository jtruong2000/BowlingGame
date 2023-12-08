using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pin : MonoBehaviour
{
    private bool _done;

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.collider.CompareTag("Ball") || collision.collider.CompareTag("Pin")) && !_done)
        {
            // get the velocity of the pin after the collision
            float velocity = GetComponent<Rigidbody>().velocity.magnitude;

            // check if the velocity has dropped below the fall threshold
            if (velocity < 10)
            {
                // Update points
                int point = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>().Point;
                point += 1;
                
                // Update UI
                UpdateUI(point);

                // Update high score if necessary
                UpdateHighScore(point);

                // Transfer points to the next stage (you may want to save it in a persistent manager)
                PersistentGameManager.Instance.Points = point;

                _done = true;
            }
        }
    }

    private void UpdateUI(int point)
    {
        // Update the UI text
        GameObject.FindGameObjectWithTag("Point").GetComponent<TextMeshProUGUI>().text = $"Number of fallen pins: {point}";
    }

    private void UpdateHighScore(int point)
    {
        // Check if the current score is higher than the high score
        if (point > PersistentGameManager.Instance.HighScore)
        {
            // Update the high score
            PersistentGameManager.Instance.HighScore = point;

            // You may also want to update the high score UI here
            GameObject.FindGameObjectWithTag("HighScore").GetComponent<TextMeshProUGUI>().text = $"High Score: {point}";
        }
    }
}
