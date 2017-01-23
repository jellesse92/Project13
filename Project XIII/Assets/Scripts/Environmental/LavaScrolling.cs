using UnityEngine;
using System.Collections;

public class LavaScrolling : MonoBehaviour 
{
	public Vector2 animationRate = new Vector2( 1.0f, 0.0f );
	public string textureName = "_MainTex";
	
	Vector2 offset = Vector2.zero;
	
	void LateUpdate() 
	{
        offset += (animationRate * Time.deltaTime );
		if( GetComponent<Renderer>().enabled )
		{
			GetComponent<Renderer>().sharedMaterial.SetTextureOffset( textureName, offset);
		}
	}
}