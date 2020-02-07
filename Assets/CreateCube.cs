//======= Copyright (c) Valve Corporation, All rights reserved. ===============

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Valve.VR.InteractionSystem.Sample
{
    public class CreateCube : MonoBehaviour
    {
        public SteamVR_Action_Boolean cubeAction;

        public Hand hand;
        public Hand otherHand;

        public GameObject cubePrefab;


        private void OnEnable()
        {
            if (hand == null)
                hand = this.GetComponent<Hand>();

            if (cubeAction == null)
            {
                Debug.LogError("<b>[SteamVR Interaction]</b> No CreateCube action assigned", this);
                return;
            }

            cubeAction.AddOnChangeListener(OnCubeActionChange, hand.handType);
        }

        private void OnDisable()
        {
            if (cubeAction != null)
                cubeAction.RemoveOnChangeListener(OnCubeActionChange, hand.handType);
        }

        private void OnCubeActionChange(SteamVR_Action_Boolean actionIn, SteamVR_Input_Sources inputSource, bool newValue)
        {
            if (newValue)
            {
                CreateNewCube();
            }
        }

        public void CreateNewCube()
        {
            StartCoroutine(GenerateCube());
        }

        private IEnumerator GenerateCube()
        {
            

            GameObject newCube = GameObject.Instantiate<GameObject>(cubePrefab);
            newCube.transform.position = (hand.transform.position + otherHand.transform.position) /2;
            newCube.transform.rotation = Quaternion.Euler(0, Random.value * 360f, 0);

            //newCube.GetComponentInChildren<MeshRenderer>().material.SetColor("_TintColor", Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));

            Rigidbody rigidbody = newCube.GetComponent<Rigidbody>();

            

            float x = Mathf.Abs(hand.transform.position.x - otherHand.transform.position.x);
            float y = Mathf.Abs(hand.transform.position.y - otherHand.transform.position.y);
            float z = Mathf.Abs(hand.transform.position.z - otherHand.transform.position.z);

           
            Debug.Log("size " +  " " + x  + " " + y + " " + z);
            Vector3 newCubeScale = new Vector3(x, y, z);

            newCube.transform.localScale = newCubeScale*10;

            if (rigidbody != null)
                rigidbody.isKinematic = false;
            
            yield return null;
        }
    }
}