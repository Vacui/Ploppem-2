using System;
using UltEvents;
using UnityEngine;

[Serializable]
public sealed class UltEventColor : UltEvent<Color> { }

[Serializable]
public sealed class UltEventString : UltEvent<string> { }

[Serializable]
public sealed class UltEventFloat : UltEvent<float> { }

[Serializable]
public sealed class UltEventVector3 : UltEvent<Vector3> { }