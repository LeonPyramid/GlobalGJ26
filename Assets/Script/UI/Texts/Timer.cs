using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private float initialTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        initialTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        text.SetText($"{Time.time - initialTime}s : {TimeManager.Instance.newTimeType}");
    }
}
