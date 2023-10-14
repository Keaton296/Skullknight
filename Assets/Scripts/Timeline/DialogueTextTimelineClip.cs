using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueTextTimelineClip : PlayableAsset
{
    public string text;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DialogueTextTimelineBehaviour>.Create(graph);
        playable.GetBehaviour().dialogueText = text;

        return playable;
    }
}
