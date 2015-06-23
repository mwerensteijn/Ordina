using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {
	
	private Slider slider;
	//private int counter;
	
	private int MaxProgress = 0;
	public Image Fill;  // assign in the editor the "Fill"
	public Color MaxHealthColor = Color.red;
	public Color MinHealthColor = Color.green;
	
	private void Awake() {
		slider = gameObject.GetComponent<Slider>();
		//counter = 0;            // just for testing purposes
	}
	
	private void Start() {
		slider.wholeNumbers = true;        // I dont want 3.543 Health but 3 or 4
		slider.minValue = 0f;
		slider.maxValue = 100;
		slider.value = 0;        // start with full health
	}
	
    //private void Update() {
    //    UpdateProgressBar(counter);        // just for testing purposes
    //    if (counter < MaxProgress)
    //    {
    //        counter++;
    //    }
    //    //counter++;                        // just for testing purposes
    //}
	
	public void UpdateProgressBar(int val) {
        Debug.Log("val: " + val + " MaxProgress: " + MaxProgress);
		slider.value = (float)val / (float)MaxProgress * 100;
        Fill.color = Color.Lerp(MinHealthColor, MaxHealthColor, (float)val / (float)MaxProgress );
	}

    public void SetMaxAwnsers(int totalAwnsers) { MaxProgress = totalAwnsers;}
}
