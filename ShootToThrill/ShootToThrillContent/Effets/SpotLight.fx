
//--------------------------- EFFET SPOTLIGHT ----------------------------


//-------------------------------- SEMANTICS --------------------------------

#define MaxLumieres 10

// Semantic - générales
float4x4 Monde : WORLD;
float4x4 MondeVueProjection : WORLDVIEWPROJECTION;
float4x3 MondeTransposeeInverse : WORLDINVERSETRANSPOSE;


float3 PositionLumieres[MaxLumieres];
float4 CouleursLumieres[MaxLumieres];
float RayonsLumieres[MaxLumieres];
int NombreLumieres;
float3 PositionObjet;

// Semantic - Matériau
float4 CouleurLumiereDiffuse : DIFFUSE;

// Semantic - Texture
bool TextureActive;
texture Texture : TEXTURE0;


//-------------------------------- STRUCTURES --------------------------------

// Déclaration de la structure du format de la texture, aucun filtre dans le cas présent
sampler FormatTexture = sampler_state
{
	Texture = (Texture);
};

struct VertexShaderInput 
{
	float4 Position : POSITION0;    
	float4 PositionOut : TEXCOORD2;// Position du sommet dans l'espace 3D
	float3 Normale : NORMAL;                 // Normale du sommet dans l'espace 3D
	float2 CoordonneesTexture : TEXCOORD;   // Coordonnées de texture (0..1, 0..1) liées au sommet
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;              // Position du sommet en fonction de la matrice MondeVueProjection (Clip Space)	
	float4 PositionOut : TEXCOORD2;
	float2 CoordonneesTexture : TEXCOORD0;    // Coordonnées de texture (0..1, 0..1) liées au sommet
	float3 Normale : TEXCOORD1;                // Vecteur normal du pixel
	//float Distance : FOG;
};

VertexShaderOutput VertexShaderSpotLight(VertexShaderInput EntreeVS)
{
	VertexShaderOutput SortieVS;
	
	//SortieVS.Distance = distance(PositionLumiere, mul(EntreeVS.Position, Monde));
	// Transformation des sommets en fonction de la matrice MondeVueProjection
	SortieVS.Position = mul(EntreeVS.Position, MondeVueProjection);
	// Affectation (sans transformation) des coordonnées de texture qui seront interpolées par le GPU
	SortieVS.CoordonneesTexture = EntreeVS.CoordonneesTexture;
 
	SortieVS.Normale = mul(float4(EntreeVS.Normale, 0), Monde);
	SortieVS.PositionOut = mul(EntreeVS.Position, MondeVueProjection);

	return SortieVS;
}

float CalculerNorme(float3 vecteur)
{
	return sqrt(pow(vecteur.x, 2) + pow(vecteur.y, 2) + pow(vecteur.z, 2));
}

float4 PixelShaderSpotLight(VertexShaderOutput EntreePS) : COLOR0
{
	float3 positionRelative = float3(0, 0, 0);
	float4 couleurTexture;
	float4 accumulationCouleur = CouleurLumiereDiffuse;
	float attenuation = pow(1 - EntreePS.Normale, 2);

	if (TextureActive)
	{
		couleurTexture = tex2D(FormatTexture, EntreePS.CoordonneesTexture);
	}
	else
	{
		couleurTexture = CouleurLumiereDiffuse;
	}

	couleurTexture /= 10;
	float distance = 0;

	for (int i = 0; i < NombreLumieres; i++)
	{
		positionRelative = PositionObjet - PositionLumieres[i];
		distance = length(positionRelative) / 2;

		if (RayonsLumieres[i] > length(positionRelative))
		{
			couleurTexture += attenuation * max(dot(EntreePS.Normale, -positionRelative), 0) * CouleursLumieres[i] / distance;
		}
	}

	//couleurTexture.rgb *= accumulationCouleur.rgb;

	return couleurTexture;
}

technique Technique_SpotAVincent
{
    pass SpotLight
    {
        // TODO: set renderstates here.
		
		VertexShader = compile vs_3_0 VertexShaderSpotLight();
		PixelShader = compile ps_3_0 PixelShaderSpotLight();
    }
}
