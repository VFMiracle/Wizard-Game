using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    [SerializeField]
    private Sprite[] enmElmtalSprites;
    [SerializeField]
    private AudioClip[] castSounds, specialSounds;
    [SerializeField]
    private GameObject[] deathParticles;
    private static Dictionary<string, Color> elementColor = new Dictionary<string, Color>();

    private void Awake()
    {
        elementColor = new Dictionary<string, Color>
        {
            {"Fire", new Color(0.877f, 0.186f, 0.095f)},
            {"Water", new Color(0.126f, 0.449f, 0.811f)},
            {"Nature", new Color(0.051f, 0.575f, 0.135f)}
        };
    }

    public AudioClip GetCastSoundByElement(ElementType element)
    {
        AudioClip ret;
        switch (element)
        {
            case ElementType.Fire:
                ret = castSounds[0];
                break;
            case ElementType.Water:
                ret = castSounds[1];
                break;
            case ElementType.Nature:
                ret = castSounds[2];
                break;
            default:
                ret = castSounds[0];
                break;
        }
        return ret;
    }

    public AudioClip GetSpecialSoundByElement(ElementType element)
    {
        AudioClip ret;
        switch (element)
        {
            case ElementType.Fire:
                ret = specialSounds[0];
                break;
            case ElementType.Water:
                ret = specialSounds[1];
                break;
            case ElementType.Nature:
                ret = specialSounds[2];
                break;
            default:
                ret = specialSounds[0];
                break;
        }
        return ret;
    }

    public GameObject SpawnDeathParticle(ElementType element, Vector2 position)
    {
        GameObject deathPrtcl;
        switch (element)
        {
            case ElementType.Fire:
                deathPrtcl = deathParticles[0];
                break;
            case ElementType.Water:
                deathPrtcl = deathParticles[1];
                break;
            case ElementType.Nature:
                deathPrtcl = deathParticles[2];
                break;
            default:
                deathPrtcl = deathParticles[0];
                break;
        }
        return Instantiate(deathPrtcl, position, Quaternion.identity);
    }

    //Returns a specific RGB color from the 'elementColor' dictionary based on the 'id' parameter, which will be used as
    //  an index.
    public static Color getElementColor(string id)
    {
        return elementColor[id];
    }

    public static ElementType RandomElement()
    {
        ElementType ret = ElementType.Fire;
        int val = Random.Range(0, 3);
        switch (val)
        {
            case 0:
                ret = ElementType.Fire;
                break;
            case 1:
                ret = ElementType.Water;
                break;
            case 2:
                ret = ElementType.Nature;
                break;
        }
        return ret;
    }

    public static bool IsWeakness(ElementType element, ElementType possWeakness)
    {
        bool ret = false;
        if (element == ElementType.Fire && possWeakness == ElementType.Water) ret = true;
        else if (element == ElementType.Water && possWeakness == ElementType.Nature) ret = true;
        else if (element == ElementType.Nature && possWeakness == ElementType.Fire) ret = true;
        return ret;
    }

    public enum ElementType
    {
        Fire,
        Water,
        Nature,
    }
}
