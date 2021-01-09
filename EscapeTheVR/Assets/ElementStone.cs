using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementStone
{
    public programmingElement elem; // e.g. for/if 
    public int id; // the position in which it should be placed

    public string descriptionText; // text to describe the stone

    protected List<string> hintSounds;
    protected int currentHintSoundIndex;

    public ElementStone(programmingElement type, string description=Constants.descriptionDefault)
    {
        this.descriptionText = description;
        this.elem = type;
        this.InitHintSounds();
    }

    public void PlayHintSound()
    {
        var gameObject = GameObject.FindGameObjectWithTag("HintAudioSource");
        var audioSource = gameObject.GetComponent<AudioSource>();
        
        if (!audioSource.isPlaying)
        {
            var clip = Resources.Load<AudioClip>(this.hintSounds[this.currentHintSoundIndex]);
            audioSource.clip = clip;
            audioSource.Play();
            this.currentHintSoundIndex = ++this.currentHintSoundIndex % this.hintSounds.Count; // Loop hintsoundindex
        }
    }

    private void InitHintSounds()
    {
        switch (this.elem)
        {
            case programmingElement.elemVar:
                this.hintSounds = new List<string>()
                {
                    "Audio/Elementstonehints/variable_1",
                    "Audio/Elementstonehints/variable_2"
                };

                break;
            case programmingElement.elemFor:
                this.hintSounds = new List<string>()
                {
                    "Audio/Elementstonehints/for",
                };

                break;
            case programmingElement.elemIf:
                this.hintSounds = new List<string>()
                {
                    "Audio/Elementstonehints/if_1",
                    "Audio/Elementstonehints/if_2",
                    "Audio/Elementstonehints/ifschleife"
                };

                break;
            case programmingElement.elemWhile:
                this.hintSounds = new List<string>()
                {
                    "Audio/Elementstonehints/while_1",
                    "Audio/Elementstonehints/while_2"
                };

                break;
            case programmingElement.elemCompare:
                this.hintSounds = new List<string>()
                {
                    "Audio/Elementstonehints/equals_1",
                    "Audio/Elementstonehints/equals_2"
                };

                break;
            case programmingElement.elemLogicalAnd:
                this.hintSounds = new List<string>()
                {
                    "Audio/Elementstonehints/and",
                    "Audio/Elementstonehints/and_zeichen"
                };

                break;
            case programmingElement.elemLogicalOr:
                this.hintSounds = new List<string>()
                {
                    "Audio/Elementstonehints/or_1",
                    "Audio/Elementstonehints/or_2",
                    "Audio/Elementstonehints/or_zeichen"
                };

                break;
            case programmingElement.elemFuncPrint:
                this.hintSounds = new List<string>()
                {
                    "Audio/Elementstonehints/print"
                };

                break;
            case programmingElement.elemText:
                this.hintSounds = new List<string>()
                {
                    "Audio/Elementstonehints/text"
                };

                break;
            case programmingElement.elemNumber:
                this.hintSounds = new List<string>()
                {
                    "Audio/Elementstonehints/number"
                };

                break;
            case programmingElement.elemBool:
                this.hintSounds = new List<string>()
                {
                    "Audio/Elementstonehints/bool"
                };

                break;
            case programmingElement.elemInterval:
                this.hintSounds = new List<string>()
                {
                    "Audio/Elementstonehints/interval"
                };

                break;
            case programmingElement.elemEnd:
                this.hintSounds = new List<string>()
                {
                    "Audio/Elementstonehints/end"
                };

                break;
            case programmingElement.elemNotEquals:
                this.hintSounds = new List<string>()
                {
                    "Audio/Elementstonehints/notequals"
                };

                break;
            case programmingElement.elemMultiplyEquals:
            case programmingElement.elemNegation:
                this.hintSounds = new List<string>()
                {
                    "Audio/luja_soge"
                };

                break;
            default:
                this.hintSounds = new List<string>();
                break;
        }
    }
}

public class VariableStone : ElementStone
{
    public ElementStone filledWith;
    public string uuid { get; }
    public VariableStone(string description = Constants.descriptionDefault) 
        : base(programmingElement.elemVar, description)
    {
        uuid = System.Guid.NewGuid().ToString();
    }
}

public class IntervalStone : ElementStone
{
    public ElementStone from;
    public ElementStone to;
    public string uuid { get; }
    public IntervalStone(string description = Constants.descriptionDefault) 
        : base(programmingElement.elemInterval, description)
    {
        uuid = System.Guid.NewGuid().ToString();
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
