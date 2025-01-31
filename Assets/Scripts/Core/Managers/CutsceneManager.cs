using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Serialization;

namespace Skullknight.Core
{
    public class CutsceneManager: MonoBehaviour
    {
        
        [SerializeField] private PlayableDirector director;
        public PlayableDirector Director => director;
        
        [SerializeField] private CutsceneInstance[] cutscenes;
        public CutsceneInstance[] Cutscenes => cutscenes;
        [SerializeField] private int selectedIndex = 0;
        public UnityEvent OnCutsceneFinished;
        
        void Awake()
        {
            //if(playables == null) playables = new PlayableAsset[1];
            director.stopped += (x) =>
            {
                OnCutsceneFinished?.Invoke();
            };
        }

        public void PlayCutscene(int index)
        {
            if (index > -1 && index < cutscenes.Length)
            {
                if (cutscenes[index] != null)
                {
                    director.playableAsset = cutscenes[index].asset;
                    director.Play();
                    selectedIndex = index;
                }
            }
        }
        public void PlayNextCutscene()
        {
            PlayCutscene(selectedIndex + 1);
        }

        public void EndCutscene()
        {
            if (director.time < director.duration)
            {
                director.time = director.duration;
                director.Evaluate();
            }
            director.Stop();
        }

        public CutsceneInstance GetFirstCutscene()
        {
            return cutscenes[0];
        }

        public void SkipCutscene()
        {
            
        }
    }
    [System.Serializable]
    public class CutsceneInstance
    {
        public PlayableAsset asset;
        public bool playOnStart = false;
        public bool isBossCutscene = false;
    }
}