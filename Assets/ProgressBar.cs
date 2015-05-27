using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {
	
	private Slider slider;
	private int counter;
	
	public int MaxProgress = 100;
	public Image Fill;  // assign in the editor the "Fill"
	public Color MaxHealthColor = Color.red;
	public Color MinHealthColor = Color.green;
	
	private void Awake() {
		slider = gameObject.GetComponent<Slider>();
		counter = 0;            // just for testing purposes
	}
	
	private void Start() {
		slider.wholeNumbers = true;        // I dont want 3.543 Health but 3 or 4
		slider.minValue = 0f;
		slider.maxValue = MaxProgress;
		slider.value = 0;        // start with full health
	}
	
	private void Update() {
		UpdateProgressBar(counter);        // just for testing purposes
        if (counter < MaxProgress)
        {
            counter++;
        }
		//counter++;                        // just for testing purposes
	}
	
	public void UpdateProgressBar(int val) {
		slider.value = val;
        Fill.color = Color.Lerp(MinHealthColor, MaxHealthColor, (float)val / MaxProgress);
	}
}
