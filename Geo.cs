using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geo : MonoBehaviour {
	LineRenderer liney; // This initializes a LineRenderer that lives outside of the functions defined below
	float pi = Mathf.PI;
	Mesh meshy;// Same as liney, but for mesh
	float norm2 = 1/Mathf.Sqrt(2f);
	Vector3[] memory = new Vector3[26];// So, the linerenderer will have this list of vectors as the positions of its points.
	public Transform p1;// This will be the marker of the first point
	public Transform p2;// Endpoint
	public Vector3 v1;// coordinate of the first point
	public Vector3 v2;// coordinate of the second point
	bool Inv = false;// Wanna invert through a sphere centered at the origin? this'll do it
	float poop;// just the radius of said sphere
	public bool ball = false;// transfers to the ball model
	public float Euc = 0;// How Euclidean are we?
	// Use this for initialization
	void Start () {
		Vector3 A = v1;// setting local variables A and B, kinda useless now but were useful before
		Vector3 B = v2;
		liney = GetComponent<LineRenderer>() ;// assigning liney so that we can set initial characteristics of the curve
		liney.numPositions=26;
		memory = hgeo(Euc,A,B);// setting geodesic nature of curve
		liney.SetPositions (memory);
		liney.startWidth = .005f;// f means float
		p1.position = A;
		p2.position = B;
		liney.receiveShadows = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("q")){
			ball = !ball;// toggle between ball model and halfplane
		}
		float t = pi*Time.time/9f;// a measure of time elapsed
		float sin = Mathf.Sin (t);
		float cos = Mathf.Cos (t);
		float tan = Mathf.Tan (t);
		float exp = Mathf.Exp (t); 
		float cosh = (exp+1/exp)/2;
		float sinh = (exp-1/exp)/2;
		float tanh = cosh / sinh;
		float q = t;
		float p = 2*t;
		float r = 4*t;
		float poop = 1f;
		complex fart = new complex ();// the following lines will create the matrix elements of the mobius transformation
		//complex a = Mathf.Cos(q/2f) * complex.ExpI((r+p)/2f);
		//complex b = Mathf.Sin(q/2f)*complex.ExpI((p-r)/2f);
		//complex c = -Mathf.Sin(q/2f)*complex.ExpI((r-p)/2f);
		//complex d = Mathf.Cos(q/2f)*complex.ExpI(-(r+p)/2f);

		complex a = new complex (1,0);
		complex b = new complex (0,0);
		complex c = new complex (0,0);
		complex d = new complex (1,0);
		for (int i = 0; i < liney.numPositions; i++) {
			Vector3 U = memory [i]; // OG position
			Vector3 V = LFrac (U,a,b,c,d);// new position after transformation
			if (Inv){
				liney.SetPosition (i, Sinversion(poop, fart,V));
				continue;
			}
			if (ball){
				liney.SetPosition (i,Ball(V));
				continue;
			}
			//V = LFrac (V, new complex (1, 0), new complex (0, -1), new complex (1, 0), new complex (0, 1));
			liney.SetPosition (i,V);
		}
		p1.position = LFrac (v1, a, b, c, d);// resetting start point
		p2.position = LFrac (v2, a, b, c, d);// resetting endpoint
		if (Inv){
			p1.position = Sinversion (poop,fart, p1.position);
			p2.position = Sinversion (poop,fart, p2.position);
		}

		if (ball){
			p1.position = Ball (p1.position);
			p2.position = Ball (p2.position);
		}
	}
    // This is the map from the half plane to the ball
	Vector3 Ball(Vector3 V){
		Vector3 s = Vector3.zero;
		float phi = (1f-V.sqrMagnitude/4f)/(1f+V.sqrMagnitude/4f);
		float chi = phi + 1;
		float z = V.y;
		float k = z * chi / 2f;
		k = 1f/(k+1);
		s.x = V.x*k*chi/2f;
		s.y = phi*k;
		s.z = (-V.z) * chi*k / 2f;
		return s;
	}
    // mobius transformation
	Vector3 LFrac(Vector3 U, complex a, complex b, complex c, complex d){
		float x = U.x;
		float y = - U.z;
		float r = U.y;
		complex z = new complex (x,y);
		float el = x*x+y*y+r*r;
		float cl = c.len (2);
		float sc = cl * el + d.len (2) + 2 * (c * z*(~d)).x;
		sc = 1f / sc;
		complex w = new complex ();
		w = el*a*(~c) + a*z*(~d)+ ~(c*z*(~b))+b*(~d);
		w = sc * w;
		Vector3 V = new Vector3 (w.x,r*sc,-w.y);
		return V;
	}
    // reflect a point "Point" through a sphere centered at "center", with radius "radius"
	Vector3 Sinversion(float radius ,complex center,Vector3 Point){
		Vector3 result = Vector3.zero;
		complex x = new complex( Point.x,-Point.z);
		float z = Point.y;
		float scale = radius * radius/(Point.sqrMagnitude +center.len(2)-2*(center*x).x);
		complex w = center - (~center)*scale +x*scale ;
		z = scale *z;
		result.x = w.x;
		result.y = z;
		result.z = -w.y;
		return result;
	}




    // complex numbers are useful!
	public class complex{
		public float x;
		public float y;
		public Vector2 comp; // component vector

        // initialization
		public complex(float r = 0, float c = 0){
			x = r;
			y = c;
			comp = new Vector2(x,y);
		}
			

        // complex multiplication
		public static complex operator *(complex A, complex B){
			complex M = new complex ();
			M.x = A.x*B.x - A.y*B.y;
			M.y = A.x*B.y+A.y*B.x;
			return M;
		}

		public Vector2 ToVec(complex x){
			Vector2 result = new Vector2 (x.x,x.y);
			return result;
		}
        // scaling of number
		public static complex operator *(float t, complex B){
			complex M = new complex ();
			M.x = t * B.x;
			M.y = t * B.y;
			return M;
		}
        // just to define it both ways
		public static complex operator *(complex B, float t){
			complex M = new complex ();
			M.x = t * B.x;
			M.y = t * B.y;
			return M;
		}
        // complex conjugate
		public static complex operator ~(complex A){
			complex M = new complex ();
			M.x = A.x;
			M.y = -A.y;
			return M;
		}
        // addition, duh
		public static complex operator +(complex A, complex B){
			complex M = new complex ();
			M.x = A.x+B.x;
			M.y = A.y + B.y;
			return M;
		}
        // negative
		public static complex operator -(complex x){
			complex M = new complex ();
			M.x = -x.x;
			M.y = -x.y;
			return M;
		}
        // subtraction
		public static complex operator -(complex x, complex y){
			complex M = new complex ();
			M = x + (-y);
			return M;
		}
        // recursive formulation of powers
		public static complex operator ^(complex A, int i){
			complex Z = new complex ();
			int n = i;
			if (n == 1){
				return A;
			}

			else{
				Z = A * (A ^ (n - 1));
				return Z;
			}
		}
		// dum thing, but lets you find the length of the complex vector	
		public float len (int n){
			return Mathf.Pow (this.x*this.x+this.y*this.y,n/2f);
		}
        // mapping the line to the circle
		public static complex ExpI(float s){
			complex M = new complex ();
			M.x = Mathf.Cos (s);
			M.y = Mathf.Sin (s);
			return M;
		}
	}
		
    // Geodesic
	Vector3[] hgeo(float s, Vector3 A, Vector3 B){
		Vector3[] geo = new Vector3[26];
		Vector3 Avg = (A+B)/2;
		Vector3 z = Vector3.up;
		if (Mathf.Abs(A.x-B.x) < .001f && Mathf.Abs(A.z-B.z) < .001f){// If the two points are vertical, than the geo is just the straight line between them
			for (int i = 0; i < 26; i++) {
				float t = i/25f;
				geo [i] = A+t*(B-A);// Check that this makes a path from A to B
			}
		}
		if (A.y == B.y) {// this and the next block are based on a basic geometric construction, using the plane made by the difference vector (A-B) and the z axis
			float c = -A.y;// Using vectors based at the vector Avg
			Vector3 C = Avg + c * z;
			for (int i = 0; i < 26; i++) {// rotate and sweep out the geodesic
				float t = i / 25f;
				float angle = -Vector3.Angle(B-C,A-C) *  t;
				//float angle = 360 *t;
				geo [i] = C + Quaternion.AngleAxis (angle, Vector3.Cross(B-A,z)) * (A-C);
				geo[i] =geo[i]+ s*(A+t*(B-A)-geo[i]);
			}
		} 

		if (A.y != B.y && !(Mathf.Abs(A.x-B.x) < .001f && Mathf.Abs(A.z-B.z) < .001f) ) {// same as before, but transformed because A-B is not horizontal
			Vector3 uv = B - A;
			Vector3 l = uv - (uv.sqrMagnitude / uv.y) * z;
			float c = -Avg.y/(l.y);
			Vector3 C = Avg+c*l;
			for (int i = 0; i < 26; i++) {
				float t = i / 25f;
				float angle = -Vector3.Angle(B-C,A-C) * t;
				//float angle = 360 *t;
				geo [i] =C+ Quaternion.AngleAxis (angle, Vector3.Cross(uv,z)) * (A-C);
				geo[i] =geo[i]+ s*(A+t*(B-A)-geo[i]);
			}
		}
			
		return geo;
	}


}
