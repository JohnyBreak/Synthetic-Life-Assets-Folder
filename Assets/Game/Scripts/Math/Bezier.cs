using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Bezier
{

    public static Vector3 GetPoint3(Vector3 p0, Vector3 p1, Vector3 p2, float _time)
    {

        _time = Mathf.Clamp01(_time);
        float _oneMinusT = 1f - _time;
        return
             _oneMinusT * _oneMinusT * p0 +
            2f * _oneMinusT * _time * p1 +
              _time * _time * p2;
    }

    public static Vector3 GetPoint4(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float _time)
    {
       
        _time = Mathf.Clamp01(_time);
        float _oneMinusT = 1f - _time;
        return
            _oneMinusT * _oneMinusT * _oneMinusT * p0 +
            3f * _oneMinusT * _oneMinusT * _time * p1 +
            3f * _oneMinusT * _time * _time * p2 +
            _time * _time * _time * p3;
    }
   
}
