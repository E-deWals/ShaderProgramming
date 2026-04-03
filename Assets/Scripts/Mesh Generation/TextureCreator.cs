using UnityEditor.Rendering;
using UnityEngine;


public class TextureCreator : MonoBehaviour {
	// Add your own pattern types here:
	public enum PatternType { Noise, None, Mandelbrot, showUV, colum, Checker, sineWave };

	public PatternType patternType;

	const int SIZE = 1024;

	Texture2D texture = null;
	Color[] cols = null;

	[SerializeField] private float degreesToRotate;


    void Start() {
		
		// Create a texture and pass it to the material of this game object's renderer:
		Renderer rend = GetComponent<Renderer>();
		texture = new Texture2D(SIZE, SIZE, TextureFormat.RGBA32, false);
		rend.material.mainTexture = texture;
		texture.wrapMode = TextureWrapMode.Clamp;

		Draw();
	}

	/// <summary>
	/// Returns the pixel color for texture coordinate (u,v), for a given pattern.
	/// </summary>
	Color CalculatePixelColor(float u, float v, PatternType pattern, Vector2 UV) {
		// TODO: insert your own pattern creation code here.
		//  See the slides for details.
		switch (pattern) {
			case PatternType.Noise: // white noise				
									//return Random.value * Color.white;
				return new Color(Random.value, Random.value, Random.value, 1);
			case PatternType.showUV:
				return new Color(UV.x, 0, UV.y, 1);
			case PatternType.colum:
				if (u * 10 % 2 > 1)
				{
					return Color.white;
				}
				else return Color.magenta;
			case PatternType.Checker:
				if (u * 10 % 2 > 1 && v * 10 % 2 < 1)
				{
					return Color.white;
				}
				else if (u * 10 % 2 < 1 && v * 10 % 2 > 1)
				{
					return Color.white;
				}
                else return Color.magenta;
			case PatternType.sineWave:
				return SineWave(new Vector2 (u, v));
			case PatternType.Mandelbrot:
				return Mandelbrot(3 * (u - 0.75f), 3 * (v - 0.5f));
			default:
				return Color.blue;
			
		}
	}

	/// <summary>
	/// Draws a pattern given by the [pattern] number to the [cols] array, which
	/// should have size [width] * [height].
	/// </summary>
	void DrawPattern(Color[] cols, int width, int height, PatternType pattern) {
		for (int index = 0; index < width * height; index++) {
			// TODO: calculate UV coordinates and pass them to CalculatePixelColor:
			int x = index % width;
			int y = index / width;

			float u = (float) x / (width);
			float v = (float) y / (height);
			float rad = Mathf.Deg2Rad * degreesToRotate;
            float newU = u * Mathf.Cos(rad) - v * Mathf.Sin(rad);
            float newV = u * Mathf.Sin(rad) + v * Mathf.Cos(rad);
            cols[index] = CalculatePixelColor(newU + x/2, newV + y/2, pattern, new Vector2(u,v));
		}
	}

	void Draw() {
		if (cols == null) {
			cols = texture.GetPixels();
		}
		DrawPattern(cols, SIZE, SIZE, patternType);

		texture.SetPixels(cols);
		texture.Apply();
	}

	// OnValidate is called whenever an inspector value is changed - even in edit mode!
	void OnValidate() {
		// To prevent calling Draw code in edit mode,
		// we check whether a texture has been created (in Start)
		if (texture == null) return;
		Draw();
	}

	private void Update() {
		// Control + S saves to file:
		if (Input.GetKeyDown(KeyCode.S) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) {
			var exporter = GetComponent<TextureExporter>();
			if (exporter != null) {
				exporter.ExportTexture(texture);
			}
		}
	}


	Color SineWave(Vector2 uv)
	{
		float x = uv.x * 2 * Mathf.PI;
		float y = Mathf.Cos(x) * 0.1f + 0.1f;
		//return uv.y > y	? Color.magenta : Color.white;
		return Color.magenta * y;
	}
	#region Mandelbrot
	// Used for the Mandelbrot fractal:
	const int maxIterations = 30;
	const float escapeLengthSquared = 4;

	Color Mandelbrot(float cReal, float cImaginary) {
		int iteration = 0;

		float zReal = 0;
		float zImaginary = 0;

		while (zReal * zReal + zImaginary * zImaginary < escapeLengthSquared && iteration < maxIterations) {
			// Use Mandelbrot's magic iteration formula: z := z^2 + c 
			// (using complex number multiplication & addition - 
			//   see https://mathbitsnotebook.com/Algebra2/ComplexNumbers/CPArithmeticASM.html)
			float newZr = zReal * zReal - zImaginary * zImaginary + cReal;
			zImaginary = 2 * zReal * zImaginary + cImaginary;
			zReal = newZr;
			iteration++;
		}
		// Return a color value based on the number of iterations that were needed to "escape the circle":
		float grad = 1f * iteration / maxIterations; // between 0 and 1
													 // TODO: use a nicer gradient
		return new Color(grad, grad, grad);
	}
	#endregion
}
