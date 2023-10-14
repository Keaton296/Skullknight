using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueTextTrackMixer : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        TextMeshPro textMeshPro = playerData as TextMeshPro;
        string currentText = "";
        if (!textMeshPro) return;
        int count = playable.GetInputCount();
        for (int i = 0; i < count; i++)
        {
            if(playable.GetInputWeight(i) > 0)
            {
                ScriptPlayable<DialogueTextTimelineBehaviour> inputPlayable = (ScriptPlayable<DialogueTextTimelineBehaviour>) playable.GetInput(i);
                DialogueTextTimelineBehaviour behaviour = inputPlayable.GetBehaviour();
                currentText = behaviour.dialogueText;
            }
        }
        textMeshPro.text = currentText;
    }
}
