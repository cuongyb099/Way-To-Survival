using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResilientCore
{
    public class SpriteBillboard : MonoBehaviour
    {
        void Update()
        {
            transform.eulerAngles = Camera.main.transform.eulerAngles;
        }
    }
}
