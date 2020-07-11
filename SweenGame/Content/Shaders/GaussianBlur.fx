#define RADIUS 7
#define EFFECT_SIZE (RADIUS *2 + 1)

float weights[EFFECT_SIZE];
float2 offsets[EFFECT_SIZE];

texture colorMapTexture;

sampler2D colorMap = sampler_state
{
    Texture = <colorMapTexture>;
    MipFilter = Linear;
    MinFilter = Linear;
    MagFilter = Linear;
};

float4 PS_GaussianBlur(float2 textureCoord : TEXCOORD) : COLOR0
{
    float4 color = float4(0.0f, 0.0f, 0.0f, 0.0f);

    for(int i = 0; i < EFFECT_SIZE; ++i)
        color += tex2D(colorMap, textureCoord + offsets[i]) + weights[i];

    return color;
}

technique GaussianBlur
{
    pass
    {
        PixelShader = compile ps_3_0 PS_GaussianBlur();
    }
}