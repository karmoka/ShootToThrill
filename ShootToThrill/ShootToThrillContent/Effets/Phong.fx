
//------------------------------- EFFET PHONG ------------------------------

//-------------------------------- SEMANTICS --------------------------------

// Semantic - g�n�rales
float4x4 Monde : WORLD;
float4x4 MondeVueProjection : WORLDVIEWPROJECTION;
float3 PositionCamera : VIEWPOSITION;

// Semantic - Sources lumineuses
float3 PositionLumiere : LIGHTDIR0_POSITION;
float3 DirectionLumiere : LIGHTDIR0_DIRECTION;
float CarreDistanceLumiere;

// Semantic - Mat�riau
float3 CouleurLumiereAmbiante : AMBIENT;
float4 CouleurLumiereDiffuse : DIFFUSE;
float3 CouleurLumiereSpeculaire : SPECULAR;
float PuissanceSpeculaire : SPECULARPOWER;
float4 IntensiteLumiereDiffuse;    // Multiplicateur d'intensit� de la lumi�re diffuse (canal par canal) 
float3 IntensiteLumiereSpeculaire; // Multiplicateur d'intensit� de la lumi�re sp�culaire (canal par canal) 

// Semantic - Texture
bool TextureActive;
texture Texture : TEXTURE0;

//-------------------------------- STRUCTURES --------------------------------

// D�claration de la structure du format de la texture, aucun filtre dans le cas pr�sent
sampler FormatTexture = sampler_state
{
	Texture = (Texture);
};
struct VertexShaderInput
{ 
	float4 Position : POSITION0; 
	float3 Normale : NORMAL0; 
	float2 CoordonneesTexture : TEXCOORD0; 
}; 
  
struct VertexShaderOutput 
{ 
	float4 Position : POSITION0; 
	float2 CoordonneesTexture : TEXCOORD0; 
	float3 Normale : TEXCOORD1; 
	float3 PosMonde : TEXCOORD2; 
}; 
  
VertexShaderOutput VertexShaderPhong(VertexShaderInput EntreeVS)
{ 
	VertexShaderOutput SortieVS; 
  
	// Transformation des sommets en fonction de la matrice MondeVueProjection
	SortieVS.Position = mul(EntreeVS.Position, MondeVueProjection); 

	// Affectation (sans transformation) des coordonn�es de texture qui seront interpol�es par le GPU
	SortieVS.CoordonneesTexture = EntreeVS.CoordonneesTexture;
  
	// Passe l'information pour les calculs de sp�cularit� et de diffusion au pixel shader
	SortieVS.Normale = mul(EntreeVS.Normale, Monde); 
	
	// Transformation des sommets en fonction de la matrice Monde
	float3 posMonde = mul(EntreeVS.Position, Monde).xyz;
	SortieVS.PosMonde = posMonde; 
  
	return SortieVS; 
} 
  
float4 PixelShaderPhong(VertexShaderOutput EntreePS) : COLOR0
{ 
	float4 couleurTexture;
	if (TextureActive)
	{
		couleurTexture = tex2D(FormatTexture, EntreePS.CoordonneesTexture);
	}
	else
	{
		couleurTexture = CouleurLumiereDiffuse;
	}
	// R�flexion Phong = lumi�re ambiante + lumi�re diffuse + reflets sp�culaires 
	// Article original: HLSL per-pixel point light using phong-blinn lighting model (https://brooknovak.wordpress.com/2008/11/13/hlsl-per-pixel-point-light-using-phong-blinn-lighting-model/)  
  
	// Donne la direction de la lumi�re pour ce fragment
	float3 directionLumiere = normalize(EntreePS.PosMonde - PositionLumiere);

	// �clairage diffus par pixel
	float eclairageDiffus = saturate(dot(EntreePS.Normale, -directionLumiere));   
  
	// Introduit la chute de l'intensit� lumineuse
	eclairageDiffus *= (CarreDistanceLumiere / dot(PositionLumiere - EntreePS.PosMonde, PositionLumiere - EntreePS.PosMonde)); 
  
	// En utilisant la modification Blinn demi-angle pour des raisons de performance malgr� l'impact sur la pr�cision
	float3 h = normalize(normalize(PositionCamera - EntreePS.PosMonde) - directionLumiere); 
	float illuminationSpeculaire = pow(saturate(dot(h, EntreePS.Normale)), PuissanceSpeculaire); 
  
	return float4(saturate((couleurTexture.xyz * CouleurLumiereAmbiante) + 
                           (couleurTexture.xyz * CouleurLumiereDiffuse * IntensiteLumiereDiffuse * eclairageDiffus) + 
                           (CouleurLumiereSpeculaire * IntensiteLumiereSpeculaire * illuminationSpeculaire)), couleurTexture.w); 
} 
  
technique TechniqueWithTexture 
{ 
	pass Phong 
	{ 
		VertexShader = compile vs_2_0 VertexShaderPhong();
		PixelShader = compile ps_2_0 PixelShaderPhong();
	} 
}
