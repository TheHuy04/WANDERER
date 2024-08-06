using UnityEngine;

public class InfiniteScrollBackground : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public GameObject layerObject;
        public float scrollSpeed;
    }

    public ParallaxLayer[] layers;
    public float overallSpeed = 1f;

    void Update()
    {
        foreach (ParallaxLayer layer in layers)
        {
            // Move the layer
            float movement = layer.scrollSpeed * overallSpeed * Time.deltaTime;
            layer.layerObject.transform.Translate(Vector3.left * movement);

            // Check if the layer has moved completely off-screen
            if (layer.layerObject.transform.position.x <= -GetLayerWidth(layer.layerObject))
            {
                // Reset the layer's position
                Vector3 resetPosition = layer.layerObject.transform.position;
                resetPosition.x += GetLayerWidth(layer.layerObject) * 2; // Move it to the right of the screen
                layer.layerObject.transform.position = resetPosition;
            }
        }
    }

    float GetLayerWidth(GameObject layerObject)
    {
        SpriteRenderer spriteRenderer = layerObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            return spriteRenderer.bounds.size.x;
        }
        return 0;
    }
}