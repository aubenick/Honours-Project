//Nicholas Aubé
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Valve.VR.InteractionSystem.Sample
{
    public class DeleteHeldItem : MonoBehaviour
    {
        public SteamVR_Action_Boolean deleteAction;

        public Hand hand;

        private void OnEnable()
        {
            if (hand == null)
                hand = this.GetComponent<Hand>();

            if (deleteAction == null)
            {
                Debug.LogError("<b>[SteamVR Interaction]</b> No deleteHeldItem action assigned", this);
                return;
            }

            deleteAction.AddOnChangeListener(OnDeleteActionChange, hand.handType);
        }

        private void OnDisable()
        {
            if (deleteAction != null)
                deleteAction.RemoveOnChangeListener(OnDeleteActionChange, hand.handType);
        }

        private void OnDeleteActionChange(SteamVR_Action_Boolean actionIn, SteamVR_Input_Sources inputSource, bool newValue)
        {
            if (newValue)
            {
                DelItem();
            }
        }

        public void DelItem()
        {
            StartCoroutine(DeleteItem());
        }

        private IEnumerator DeleteItem()
        {

            //MyScript sn = GetComponent<Hand>();
            GameObject heldItem = GetComponent<Hand>().currentAttachedObject;
            
            //If not null, detech and destroy object
            if(heldItem != null)
            {
                GetComponent<Hand>().DetachObject(heldItem);
                Destroy(heldItem);
            }

            yield return null;
        }
    }
}
