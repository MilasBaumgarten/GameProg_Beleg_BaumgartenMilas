Shader "Custom/Drag" {
    Properties {
		_Color("Base Color", Color) = (1,1,1,1)
		_StepCutoff("Step Cutoff", Range(0.01, 0.51)) = 0.5
		_MinAlpha("Minimal Alpha", Float) = 0.01
    }

    SubShader {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        LOD 100
		Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

			float _StepCutoff;
			float4 _Color;
			float _MinAlpha;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

			float2 calcRadialUV(float2 uv, float2 Center, float RadialScale, float LengthScale) {
				// transform to polar coordinates
				float2 delta = uv - Center;
				float radius = length(delta) * 2 * RadialScale;
				float angle = atan2(delta.x, delta.y) * 1.0/6.28 * LengthScale;
				return float2(radius, angle);
			}

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
				float2 uv = calcRadialUV(i.uv, float2(0.5, 0.5), 1, 1);

				fixed4 circle = 1 - step(uv.r, 1);
				fixed4 rad = clamp(uv.g, 0, 1);
				fixed4 radInv = clamp(uv.g * (-1), 0, 1);

				fixed4 mask = step(rad + radInv + circle, _StepCutoff);

				fixed4 col = mask * _Color;

				// reduce inner alpha when player _StepCutoff is high
				col.a = mask * lerp(1, _MinAlpha, _StepCutoff * 2);

                return col;
            }
            ENDCG
        }
    }
}
