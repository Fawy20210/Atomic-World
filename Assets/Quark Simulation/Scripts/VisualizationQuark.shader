Shader "Custom/VisualizationQuark"
{
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #pragma target 4.5
            StructuredBuffer<float2> _positions;
            StructuredBuffer<float> _sizes;
            StructuredBuffer<float4> _colors;

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 color : COLOR0;
            };

            uniform float4x4 _ObjectToWorld;

            /* uint colorCount;
            uint stride; */
            
            v2f vert(appdata_base v, uint instanceID : SV_InstanceID)
            {
                /* _colors.GetDimensions(colorCount,stride); */
                v2f o;
                
                float4 wpos = float4(v.vertex.xy * _sizes[instanceID % 3] + _positions[instanceID], 0, 1);
                o.pos = mul(UNITY_MATRIX_VP, wpos); //(x,y,z,scale)
                o.color = _colors[instanceID % 3];
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                return i.color;
            }
            ENDCG
        }
    }
}
