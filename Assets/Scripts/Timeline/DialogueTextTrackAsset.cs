using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
[TrackBindingType(typeof(TextMeshPro))]
[TrackClipType(typeof(DialogueTextTimelineClip))]
public class DialogueTextTrackAsset : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<DialogueTextTrackMixer>.Create(graph, inputCount);
    }
}
