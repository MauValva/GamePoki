Shader "Custom/DotsBackgroundShader"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _DotsColor ("Dots Color", Color) = (0,1,1,1)
        _AlphaControl ("Alpha Control", Float) = 1
        _Size ("Size", Float) = 10
        _RotationAngle ("Rotation Angle", Float) = 0
        _DotsAngle ("Dots Angle", Float) = 0
        _MovingSpeed ("Moving Speed", Vector) = (0, 1, 0, 0)
        _MinDotSize ("Min Dot Size", Float) = 0.1
        _MaxDotSize ("Max Dot Size", Float) = 0.4
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            #define HALF_PI 1.5707963

            float4 _BaseColor;
            float4 _DotsColor;
            float  _Size;
            float  _RotationAngle;
            float  _DotsAngle;
            float2 _MovingSpeed;
            float  _MinDotSize;
            float  _MaxDotSize;
            float  _AlphaControl;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex       : SV_POSITION;
                float2 uv           : TEXCOORD0;
                float2 origUV       : TEXCOORD1; 
                float  angleRad     : TEXCOORD2; 
                float  dotsAngleRad : TEXCOORD3; 
            };

            // -----------------------------------------------------------
            // Helpers
            // -----------------------------------------------------------

            float2 RotateUV(float2 uv, float2 pivot, float angle)
            {
                float s = sin(angle);
                float c = cos(angle);
                uv -= pivot;
                return float2(uv.x * c - uv.y * s,
                              uv.x * s + uv.y * c) + pivot;
            }

            float circle(float2 uv, float2 center, float radius)
            {
                float2 diff = uv - center;
                return saturate(1.0 - dot(diff, diff) / (radius * radius));
            }

            // -----------------------------------------------------------
            // Vertex
            // -----------------------------------------------------------

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex       = UnityObjectToClipPos(v.vertex);
                o.uv           = v.uv;
                o.origUV       = v.uv;
                o.angleRad     = radians(_RotationAngle);
                o.dotsAngleRad = radians(_DotsAngle);
                return o;
            }

            // -----------------------------------------------------------
            // Fragment
            // -----------------------------------------------------------

            float4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                float aspect = _ScreenParams.x / _ScreenParams.y;
                uv.x *= aspect;

                float2 rotatedSpeed = float2(
                    _MovingSpeed.x * cos(i.angleRad) - _MovingSpeed.y * sin(i.angleRad),
                    _MovingSpeed.x * sin(i.angleRad) + _MovingSpeed.y * cos(i.angleRad)
                );

                rotatedSpeed.x *= aspect;

                uv += _Time.y * rotatedSpeed;

                uv = RotateUV(uv, float2(0.5 * aspect, 0.5), i.angleRad);

                uv = frac(uv * _Size);

                uv = RotateUV(uv, float2(0.5, 0.5), i.dotsAngleRad);

                float t        = saturate(i.origUV.y);
                float curvedT  = 1.0 - cos(t * HALF_PI);
                float radius   = lerp(_MinDotSize, _MaxDotSize, curvedT);

                float mask = circle(uv, float2(0.5, 0.5), radius);

                float3 finalColor = lerp(_BaseColor.rgb, _DotsColor.rgb, mask);

                float alpha = mask * _AlphaControl * _BaseColor.a;

                return float4(finalColor, alpha);
            }
            ENDCG
        }
    }
}
