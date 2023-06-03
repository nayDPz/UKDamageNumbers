Shader "Custom/DamageNumber"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _FontTex("FontTexture", 2D) = "white" {}
        _FontTextColumns("FontTexture Columns", Int) = 3
        _FontTextRows("FontTexture Rows", Int) = 3
        _StringCharacterCount("Length of String", Int) = 3
        _StringOffset("String offset", Vector) = (0.5,0.5,0,0)
        _StringScale("String scale", Vector) = (0.25,0.25,0,0)
        _CharWidth("Character width", Float) = 1.0
        _Color("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags {"Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off

        ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha


        Pass
        {
            CGPROGRAM

            #pragma vertex vert alpha
            #pragma fragment frag alpha

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 custom1 : TEXCOORD1;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 custom1 : TEXCOORD1;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _FontTex;
            float4 _FontTex_ST;

            float4 _Color;

            // font texture information
            int _FontTextColumns;
            int _FontTextRows;

            // string length
            static int _StringCharacterCount = 4;

            // float array because there's no SetIntArray in c#
            float _String_Chars[4];

            // string placement & scaling
            float4 _StringOffset;
            float4 _StringScale;

            // Character width - combine with StringScale to change character spacing
            float _CharWidth;

            static int digits;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.custom1 = v.custom1;
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col;
                float dmg = i.custom1.x + .00001;
                int charCount = 1;  

                if(dmg >= 1)
                {
                    
                    digits = log10(dmg);
                                
                    int dmgint = floor(dmg);
                    [unroll(_StringCharacterCount)] for (int j = _StringCharacterCount - 1; j >= 0; j--)
                    {                  
                        _String_Chars[j] = (uint)dmgint % 10;
                        dmgint = floor((uint)dmgint / 10);
                    
                        if(dmgint != 0)
                        {
                            charCount = charCount + 1;
                        }
                    }
                }

                if(dmg < 1 && dmg > 0)
                {
                    charCount = 2;
                    _String_Chars[3] = dmg * 10;
                    _String_Chars[2] = 10;
                    _String_Chars[1] = 0; 
                }
                
                
                // Determine what character in the string this pixel is in
                // And what UV of that character we are in
                float charIndex = 0;
                float2 inCharUV = float2(0,0);

                // Avoid i.uv.x = 1 and indexing charIndex[_StringCharacterCount] 
                i.uv.x = clamp(i.uv.x, 0.0, 0.99999);

                // Scale and offset uv
                i.uv = clamp((i.uv - _StringOffset.xy) / _StringScale.xy + 0.5, 0, 1);

                // Find where in the char to sample
                inCharUV = float2(
                    modf(i.uv.x * charCount, charIndex),
                    i.uv.y);

                // Scale inCharUV.x based on charWidth factor
                inCharUV.x = (inCharUV.x-0.5f)/_CharWidth + 0.5f;

                // Clamp char uv
                // alternatively you could clip if outside (0,0)-(1,1) rect
                inCharUV = clamp(inCharUV, 0, 1);

                // Get char uv in font texture space

                charIndex = charIndex + (_String_Chars.Length - charCount);

                float fontIndex = _String_Chars[charIndex];
                float fontRow = floor(fontIndex / _FontTextColumns);
                float fontColumn = floor(fontIndex % _FontTextColumns);

                float2 fontUV = float2(
                        (fontColumn + inCharUV.x) / _FontTextColumns,
                        1.0 - (fontRow + 1.0 - inCharUV.y) / _FontTextRows);

                // Sample the font texture at that uv
                col = tex2D(_FontTex, fontUV);

                // Modify by color:
                col = col * _Color;

                col *= i.color;

                return col;
            }
           ENDCG
        }
    }
}
