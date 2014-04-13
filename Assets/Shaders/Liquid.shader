// Created by inigo quilez - iq/2013
// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.

// I've not seen anybody out there computing correct cell interior distances for Voronoi
// patterns yet. That's why they cannot shade the cell interior correctly, and why you've
// never seen cell boundaries rendered correctly. 
//
// However, here's how you do mathematically correct distances (note the equidistant and non
// degenerated grey isolines inside the cells) and hence edges (in yellow):
//
// http://www.iquilezles.org/www/articles/voronoilines/voronoilines.htm

Shader "Custom/Liquid" {
	Properties {
		_NoiseTex("Noise (RGB)", 2D) = "white" {}
		_CellColor("Cell Color", Color) = (0, 0.4, 0.7, 1)
		_EdgeColor("Edge Color", Color) = (1, 1, 1, 1)
		_CellSize("Cell Size", Range (0.05, 1)) = 0.3
		_Noise("Noise Influence", Range (0, 4)) = 1.5
		_RampPower("Ramp Power", Range (0, 4)) = 1
		_ColorSteps("Color Steps", Float) = 4
	}
	SubShader {
        Tags { "RenderType" = "Opaque" }
        
        Pass {
        CGPROGRAM
        	#pragma glsl
        	#pragma only_renderers opengl
            #pragma vertex vert
            #pragma fragment frag

    		#include "UnityCG.cginc"
			
			float2 hash(float2 p) // Generate pseudorandom-looking values
			{
				p = float2(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5, 183.3)));
				return frac(sin(p) * 43758.5453);
			}

			float3 voronoi(in float2 x)
			{
				// Calculated with coordinates relative to x, so except for using x as random seed it can be viewed as the origin (0, 0). 
				
			    float2 grid_pos = floor(x); // Used as random seed.
			    float2 pos_offset = frac(x);
			
				float2 towards_center, cell_center;
			    float min_distance_sq = 8.0;
			    
			    // Find the cell in which x is located (i.e. the closest cell origin).
			    for (int j = -1; j <= 1; j++) {
				    for (int i = -1; i <= 1; i++)
				    {
				        float2 direction = float2(i, j); 					 					// Non random direction to cell in grid.
				        float2 random_offset = hash(grid_pos + direction);   					// Random offset vector to cell.
				        random_offset = 0.5 + 0.5 * sin(_Time.y + 6.2831 * random_offset); 		// Animate random offset with sin() over time, so that it looks like the cells are moving.
						float2 pos = direction + random_offset - pos_offset;					// Canditate position of the cell containing x.
				        
				        float d_sq = dot(pos, pos);
				        if (d_sq < min_distance_sq)
				        {
				            min_distance_sq = d_sq;
				            cell_center = pos;
				            towards_center = direction;
				        }
					}
				}
			
			    min_distance_sq = 8.0f; // Reset the min distance check to the maximum distance.
			    for(int j2 = -2; j2 <= 2; j2++) {
				    for(int i2 = -2; i2 <= 2; i2++)
				    {
				        float2 direction = towards_center + float2(i2, j2); 					// The direction from the cell center towards the neighbor.
						float2 random_offset = hash(grid_pos + direction);						// Random offset vector to neighbor.
				       	random_offset = 0.5 + 0.5 * sin(_Time.y + 6.2831 * random_offset); 		// Animate random offset with sin() over time, so that it looks like the cells are moving.
				        float2 neighbor_pos = direction + random_offset - pos_offset;			// The position of the neighbor.
						float2 to_neighbor = neighbor_pos - cell_center;						// Vector from neighbor to cell center.
				        if (dot(to_neighbor, to_neighbor) > 0.000001f) // Ignore current neighbor ?
						{
							// Project x (0, 0) onto the border. This is the distance of x along neighbor_pos to closest neighbor.
					        // float d = dot((cell_center+neighbor_pos)*0.5f, normalize(neighbor_pos-cell_center));
					        float2 border_center = cell_center + 0.5f * to_neighbor;
					        float d = dot(border_center, normalize(to_neighbor));
					        min_distance_sq = min(min_distance_sq, d);
						}
				    }
				}
			
			    return float3(min_distance_sq, cell_center);
			}

    		struct appdata_t {
            	float4 vertex : POSITION;
            };

            struct v2f {
                float4 vertex : POSITION;
              	float2 vpos : VPOS;
            };
            
            sampler2D _NoiseTex;
            float4 _CellColor;
            float4 _EdgeColor;
            float _CellSize;
            float _Noise;
            float _RampPower;
            float _ColorSteps;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            
            half4 frag(v2f i) : COLOR
            {
				float2 p = i.vpos.xy / (_ScreenParams.xx*_CellSize);
			   	float3 c = voronoi(8.0f * p);
			   	
			   	float4 c2 = tex2D(_NoiseTex, p);

				// float3 col = lerp(_EdgeColor, _CellColor, smoothstep(0, 0.06, c.x*c2.r));
//			    float dd = length(c.yz); // Distance to cell center.
//			    if (dd < 0.4f)
//			    	dd = 0;
//			    else if (dd < 0.6f)
//			    	dd = 0.4f;
//			    else if (dd < 0.8f)
//			    	dd = 0.6f;
//			    else
//			    	dd = 1;
//			    
//				float3 col = lerp(_CellColor, _EdgeColor, dd);//lerp(dd, c2.r, _NoiseBlend)));

//			    float dd = sqrt(c.x)*2; // Distance to cell center.
//			    if (dd < 0.4f)
//			    	dd = 0;
//			    else if (dd < 0.6f)
//			    	dd = 0.4f;
//			    else if (dd < 0.8f)
//			    	dd = 0.6f;
//			    else
//			    	dd = 1;
//			    
//				float3 col = lerp(_CellColor, _EdgeColor, 1-dd);//lerp(dd, c2.r, _NoiseBlend)));

			    float dd = length(c.yz); // Distance to cell center.
			    float t = dd * (1-c.x) * c2.r * _Noise;
			    
//			    if (t < 0.2f)
//			    	t = 0;
//			    if (t < 0.4f)
//			    	t = 0.2f;
//			    else if (t < 0.6f)
//			    	t = 0.4f;
//			    else if (dd < 0.8f)
//			    	t = 0.6f;
//			    else
//			    	t = 1;

				t = saturate(floor(t * _ColorSteps) / _ColorSteps);
				float3 col = lerp(_CellColor, _EdgeColor, smoothstep(0, 1, pow(t, _RampPower)));
				return half4(col, 1);
            }

		ENDCG
		}
    }
	FallBack "Diffuse"
}
