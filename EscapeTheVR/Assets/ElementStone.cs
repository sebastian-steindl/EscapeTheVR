using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementStone
{
    public programmingElement elem; // e.g. for/if 
    public int id; // the position in which it should be placed

    public string descriptionText; // text to describe the stone
    public Color color; // color of the VR stone

    public Sprite icon;
    protected string hintSoundPath;

    public ElementStone(Color stoneColor, string description=Constants.descriptionDefault)
    {
        descriptionText = description;
        color = stoneColor; 
    }

    public void PlayHintSound()
    {
        var gameObject = GameObject.FindGameObjectWithTag("HintAudioSource");
        var audioSource = gameObject.GetComponent<AudioSource>();
        var clip = Resources.Load<AudioClip>(hintSoundPath);
        audioSource.clip = clip;

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}

public class VariableStone : ElementStone
{
    public ElementStone filledWith;
    public VariableStone(Color stoneColor, string description = Constants.descriptionDefault) : base(stoneColor, description)
    {
        elem = programmingElement.elemVar;

        this.hintSoundPath = "Audio/success_sound";
    }
}

public class IntervalStone : ElementStone
{
    public ElementStone from;
    public ElementStone to;
    public IntervalStone(Color stoneColor, string description = Constants.descriptionDefault) : base(stoneColor, description)
    {
        elem = programmingElement.elemInterval;
    }
}
public enum programmingElement
{
    // feel free to add stuff
    // use prefix "elem" bc if/while etc are forbidden
    elemVar, // variable
    elemFor,
    elemIf,
    elemWhile,
    elemCompare, // ==
    elemNegation, // !
    elemLogicalAnd, // &&
    elemLogicalOr, // ||
    elemFuncPrint, // print()
    elemText, // text for variables (string)
    elemNumber, // number for variables (int)
    elemBool, // bool for variables
    elemInterval, // interval between two ints, e.g. used in for
    elemEnd,//End of loops/conditions
    elemNotEquals, // !=
    elemMultiplyEquals, // *=
}
