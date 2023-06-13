using System;
using UnityEngine;

namespace Core.SingleService
{
    public abstract class BaseSingleService : MonoBehaviour
    {
        public abstract Type Type { get; }
    }
}