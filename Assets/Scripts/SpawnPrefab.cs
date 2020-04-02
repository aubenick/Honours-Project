//======= Copyright (c) Valve Corporation, All rights reserved. ===============

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Valve.VR.InteractionSystem.Sample
{
    public class SpawnPrefab : MonoBehaviour
    {
        public SteamVR_Action_Boolean spawnAction;
        public SteamVR_Action_Boolean cycleAction;
        

        public Hand hand;
        public Hand otherHand;


        // Prefabs
        public List<GameObject> prefabs;

        private int maxPrefabs;
        private int currentPrefabIndex = 0;

        private void OnEnable()
        {
            if (hand == null)
                hand = this.GetComponent<Hand>();

            if (spawnAction == null)
            {
                Debug.LogError("<b>[SteamVR Interaction]</b> No CreateCube action assigned", this);
                return;
            }

            if (cycleAction == null)
            {
                Debug.LogError("<b>[SteamVR Interaction]</b> No Cycle action assigned", this);
                return;
            }

            // Add all prefabs to a list
            maxPrefabs = prefabs.Count;
            spawnAction.AddOnChangeListener(OnCubeActionChange, hand.handType); 
            cycleAction.AddOnChangeListener(OnCycleActionChange, hand.handType);
        }

        private void OnDisable()
        {
            if (spawnAction != null)
                spawnAction.RemoveOnChangeListener(OnCubeActionChange, hand.handType);
                spawnAction.RemoveOnChangeListener(OnCycleActionChange, hand.handType);

        }

        private void OnCubeActionChange(SteamVR_Action_Boolean actionIn, SteamVR_Input_Sources inputSource, bool newValue)
        {
            if (newValue)
            {
                CreateNewPrefab();
            }
        }

        private void OnCycleActionChange(SteamVR_Action_Boolean actionIn, SteamVR_Input_Sources inputSource, bool newValue)
        {
            if (newValue)
            {
                CyclePrefab();
            }
        }

        //Create Prefab Functions
        public void CreateNewPrefab()
        {
            StartCoroutine(SpawnNewPrefab());
        }

        private IEnumerator SpawnNewPrefab()
        {
            

            GameObject newPrefab = GameObject.Instantiate<GameObject>(prefabs[currentPrefabIndex]);
            newPrefab.transform.position = (hand.transform.position + otherHand.transform.position) /2;
            //newPrefab.transform.rotation = Quaternion.Euler(0, Random.value * 360f, 0);


            Rigidbody rigidbody = newPrefab.GetComponent<Rigidbody>();

            

            float x = Mathf.Abs(hand.transform.position.x - otherHand.transform.position.x);
            float y = Mathf.Abs(hand.transform.position.y - otherHand.transform.position.y);
            float z = Mathf.Abs(hand.transform.position.z - otherHand.transform.position.z);

           
            //Debug.Log("size " +  " " + x  + " " + y + " " + z);
            Vector3 newCubeScale = new Vector3(x, y, z);

            newPrefab.transform.localScale = newCubeScale*10;

            if (newPrefab.GetComponent<MeshCollider>() != null)
            {
                newPrefab.GetComponent<MeshCollider>().sharedMesh = newPrefab.GetComponent<MeshFilter>().mesh;
            }


            if (rigidbody != null)
                rigidbody.isKinematic = false;
            
            yield return null;
        }

        //Cycle Prefab functions
        public void CyclePrefab()
        {
            currentPrefabIndex++;
            if (currentPrefabIndex == maxPrefabs)
                currentPrefabIndex = 0;
        }



        //End of class
    }
}