using System;
using System.Collections.Generic;
using Ukiyo.UI;
using UnityEngine;
using UnityEngine.UI;

public class InGamePopupMsg : MonoBehaviour
{
    public static InGamePopupMsg Instance;
    
    public Text msgText;
    public UIAnimation msgAnimation;
    public List<PopupMsg> cachedPopupMessages = new List<PopupMsg>();

    void Start()
    {
        if (Instance == null)
            Instance = this;
        msgText.gameObject.SetActive(false);
    }

    #region CRUD

    public void AddText(string text, float time)
    {
        AddText(new PopupMsg(text, time));
    }

    public void AddText(PopupMsg popupMsg)
    {
        cachedPopupMessages.Add(popupMsg);
    }
    
    public void AddUniqueText(string text, float time)
    {
        AddUniqueText(new PopupMsg(text, time));
    }

    public void AddUniqueText(PopupMsg popupMsg)
    {
        if (!cachedPopupMessages.Contains(popupMsg))
            cachedPopupMessages.Add(popupMsg);
    }

    public void RemoveText(string text)
    {
        RemoveText(new PopupMsg(text, 0));
    }

    public void RemoveText(PopupMsg popupMsg)
    {
        cachedPopupMessages.Remove(popupMsg);
    }
    
    public float CheckRemainingTime(string text)
    {
        return CheckRemainingTime(new PopupMsg(text, 0));
    }
    
    public float CheckRemainingTime(PopupMsg popupMsg)
    {
        if (cachedPopupMessages.Contains(popupMsg))
        {
            return cachedPopupMessages.Find(x => x.text.Equals(popupMsg.text)).time;
        }
        return 0;
    }

    #endregion

    void Update()
    {
        // When there is no msg, hide the actor
        if (cachedPopupMessages.Count == 0)
        {
            msgText.text = "";
            msgText.gameObject.SetActive(false);
            return;
        }
        
        // Update popup msg as a queue, only show first element for certain of time
        var popupMsg = cachedPopupMessages[0];
        var text = popupMsg.text;
        var time = popupMsg.time;

        // If the time is greater than 0, it means the msg has a time limit and only displays for a certain time
        // If the time is less than -1 means it has no time limit, only hides until it is deleted
        if (time > 0 || time <= -1)
        {
            // Activate and set the text
            if (!msgText.gameObject.activeSelf)
            {
                msgText.gameObject.SetActive(true);
                msgAnimation.PlayOpenAnimation();
            }
            msgText.text = text;
            
            // Update time
            time -= Time.deltaTime;
            // Clamp the value in case overflow
            if (time < -10)
                time = -1;
            cachedPopupMessages[0].time = time;
        }

        // When the time reaches 0 and is still greater than -1, it indicates a timeout and deletes the message
        if (time <= 0 && time > -1)
        {
            msgText.text = "";
            msgText.gameObject.SetActive(false);
            
            cachedPopupMessages.RemoveAt(0);
        }
    }

    [Serializable]
    public class PopupMsg
    {
        public string text = "";
        public float time = 0.0f;

        public PopupMsg() { }

        public PopupMsg(string str, float t)
        {
            text = str;
            time = t;
        }
        
        // Only compare text
        #region Override Comparison
        
        public static bool operator ==(PopupMsg a, PopupMsg b)
        {
            return a.text == b.text;
        }

        public static bool operator !=(PopupMsg a, PopupMsg b)
        {
            return a.text != b.text;
        }
        
        protected bool Equals(PopupMsg other)
        {
            return text == other.text;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((PopupMsg) obj);
        }

        public override int GetHashCode()
        {
            return text != null ? text.GetHashCode() : 0;
        }

        #endregion
    }
}
