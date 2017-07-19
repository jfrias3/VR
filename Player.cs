using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public GameObject shape; 
	public GameObject edge;
	public Transform Cam;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Cancel")) {
			Application.Quit();
		}

		if (Input.GetKeyDown(KeyCode.UpArrow)){
			GameObject[] X = iterate (shape);
			//foreach(GameObject x in X){
				//GameObject[] Y = iterate (x);
				//foreach (GameObject y in Y) {
					//iterate (y);
				//}
			//}
		}
			
			

	}


	GameObject[] iterate(GameObject shp){
		GameObject x = create (shp);
		GameObject y = create (shp,2,3);
		GameObject z = create (shp, 4, 5);
		GameObject w = create (shp,5,6);
		GameObject[] List = new GameObject[4];
		List [0] = x;
		List [1] = y;
		List [2] = z;
		List [3] = w;
		return List;
	}


	GameObject create(GameObject shpee, int k = 0, int j = 1){
		GameObject SHAPE = Instantiate (shpee, transform);
		Geo E1 = shpee.transform.GetChild (k).GetComponent<Geo>();
		Geo E2 = shpee.transform.GetChild (j).GetComponent<Geo>();
		for (int i = 0; i < SHAPE.transform.childCount; i++){
			//LineRenderer EDGE = SHAPE.transform.GetChild (i).GetComponent<LineRenderer> ();
			//Vector3[] list = transformation (E1.v1,E1.v2, E2.v2,ref EDGE);
			//SHAPE.transform.GetChild (i).GetComponent<LineRenderer> ().SetPositions(list);
			SHAPE.transform.GetChild(i).GetComponent<Geo>().v1 = SpecificInv(SHAPE.transform.GetChild(i).GetComponent<Geo>().v1,E1.v1,E1.v2, E2.v2); 
			SHAPE.transform.GetChild (i).GetComponent<Geo> ().v2 = SpecificInv (SHAPE.transform.GetChild (i).GetComponent<Geo> ().v2, E1.v1, E1.v2, E2.v2);
		}
		return SHAPE;
	}


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

	Vector3[] transformation( Vector3 Vertex, Vector3 edge1, Vector3 edge2  , ref LineRenderer liney ){
		Vector3[] MEM = new Vector3[liney.numPositions];
		for (int i = 0; i < liney.numPositions; i++){
			Vector3 coordVec = Sphere (Vertex, edge1, edge2);
			MEM[i] = Sinversion(coordVec.z,new complex(coordVec.x, coordVec.y), liney.GetPosition(i));
		}
		liney.SetPositions(MEM);
		return MEM;
	}



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

	Vector3 Sinversion(float radius ,complex center,Vector3 Point){
		Vector3 result = Vector3.zero;
		complex x = new complex( Point.x,-Point.z);
		float z = Point.y;
		float scale = radius * radius/(Point.sqrMagnitude +center.len(2)-2*((~center)*x).x);
		complex w = ~center - (~center)*scale +(~x)*scale ;
		z = scale *z;
		result.x = w.x;
		result.y = z;
		result.z = -w.y;
		return result;
	}


	Vector3 Sphere(Vector3 B, Vector3 A, Vector3 C){
		Vector3 S =  Vector3.zero;
		Vector3 x = new Vector3(A.x,-A.z, A.y);
		Vector3 y = new Vector3 (B.x, -B.z, B.y);
		Vector3 z = new Vector3 (C.x, -C.z, C.y);
		float lxy = (x.sqrMagnitude-y.sqrMagnitude)/2f;
		float lyz = (y.sqrMagnitude-z.sqrMagnitude)/2f;
		matrix D = new matrix (2, 2);
		D.Mat [0, 0] = (x - y).x;
		D.Mat [0, 1] = (x - y).y;
		D.Mat [1, 0] = (y - z).x;
		D.Mat [1, 1] = (y - z).y;
		matrix DI = matrix.Inv (D);
		matrix l = new matrix (2, 1);
		l.Mat[0,0] = lxy;
		l.Mat [1, 0] = lyz;                                                                                                                                                                                                                                                                     
		matrix r = DI*l;
		float ry = r.Mat[1,0];
		float rx = r.Mat[0,0];
		S.x = rx;
		S.y = ry;
		S.z = (S - x).magnitude;
		return S;
	}

	Vector3 SpecificInv(Vector3 Point, Vector3 Vertex, Vector3 edge1, Vector3 edge2){
		Vector3 coordVec = Sphere (Vertex, edge1, edge2);
		Point = Sinversion(coordVec.z,new complex(coordVec.x, coordVec.y), Point);
		return Point;
	}



	public class complex{
		public float x;
		public float y;
		public Vector2 comp;


		public complex(float r = 0, float c = 0){
			x = r;
			y = c;
			comp = new Vector2(x,y);
		}



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

		public static complex operator *(float t, complex B){
			complex M = new complex ();
			M.x = t * B.x;
			M.y = t * B.y;
			return M;
		}

		public static complex operator *(complex B, float t){
			complex M = new complex ();
			M.x = t * B.x;
			M.y = t * B.y;
			return M;
		}

		public static complex operator ~(complex A){
			complex M = new complex ();
			M.x = A.x;
			M.y = -A.y;
			return M;
		}

		public static complex operator +(complex A, complex B){
			complex M = new complex ();
			M.x = A.x+B.x;
			M.y = A.y + B.y;
			return M;
		}

		public static complex operator -(complex x){
			complex M = new complex ();
			M.x = -x.x;
			M.y = -x.y;
			return M;
		}

		public static complex operator -(complex x, complex y){
			complex M = new complex ();
			M = x + (-y);
			return M;
		}

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

		public float len (int n){
			return Mathf.Pow (this.x*this.x+this.y*this.y,n/2f);
		}

		public static complex ExpI(float s){
			complex M = new complex ();
			M.x = Mathf.Cos (s);
			M.y = Mathf.Sin (s);
			return M;
		}

		public static complex operator /(complex a, complex b){
			complex result = new complex ();
			complex invb = ~b;
			invb = (1/(b.len(2)))*invb;
			result = a * invb;
			return result;
		}
	}




	public class matrix{
		public int R;
		public int C;
		public float[,] Mat;

		public matrix(int r, int c){
			R = r;
			C = c;
			Mat = new float[R,C];
		}

		public float m(int r, int c){
			return Mat [r, c];
		}
		public static matrix tran(matrix A){
			matrix M = new matrix (A.C,A.R);
			for (int i = 0; i < A.C; i++ ){
				for (int j = 0; j < A.R; j++) {
					M.Mat [i, j] = A.Mat [j, i];
				}
			}
			return M;
		}

		public matrix del(int r, int c){
			matrix COF = new matrix (R - 1, C - 1);
			for (int i = 0; i <R ; i++){
				for (int j = 0; j < C ; j++) {
					if (i < r && j < c){
						COF.Mat [i, j] = this.Mat [i, j];
					}

					if(i < r && j > c){
						COF.Mat [i, j - 1] = this.Mat [i, j];
					}

					if(i>r && j<c ){
						COF.Mat [i - 1, j] = this.Mat [i, j];
					}

					if(i>r && j>c){
						COF.Mat [i - 1, j - 1] = this.Mat [i, j];
					}
				}
			}
			return COF;
		}

		public static float det(matrix A){
			float Det = 0;
			if(A.R == A.C && A.R == 1){
				Det = A.Mat [0, 0];
			}
			if (A.R == A.C && A.R ==2) {
				Det = A.m (0, 0) * A.m (1, 1) - A.m (0, 1) * A.m (1, 0);
				return Det;
			}
			if (A.R == A.C && A.R > 2) {
				for (int c = 0; c < A.C; c++) {
					Det += Mathf.Pow (-1, c) * A.m (0, c) * det(A.del(0,c));
				}
			}
			return Det;
		}

		public static matrix operator *(matrix A, matrix B){
			matrix M = new matrix (A.R,B.C);
			if (A.C == B.R){
				for(int i = 0; i < A.R; i++){
					for (int j = 0; j < B.C; j++ ){
						for(int k = 0; k < A.C; k++){
							M.Mat [i, j] += A.m (i, k) * B.m (k, j);
						}
					}
				}
			}
			return M;
		}

		public static matrix operator *(float t, matrix B){
			matrix M = new matrix (B.R,B.C);
			for(int i = 0; i < B.R; i++){
				for (int j = 0; j < B.C; j++ ){
					M.Mat [i, j] = t * B.Mat [i, j];
				}
			}
			return M;
		}

		public static matrix operator *(matrix B, float t){
			matrix M = new matrix (B.R,B.C);
			for(int i = 0; i < B.R; i++){
				for (int j = 0; j < B.C; j++ ){
					M.Mat [i, j] = t * B.Mat [i, j];
				}
			}
			return M;
		}

		public static matrix operator +(matrix A, matrix B){
			matrix M = new matrix (A.R,B.C);
			if (A.C == B.C && A.R == B.R){
				for(int i = 0; i < A.R; i++){
					for (int j = 0; j < B.C; j++ ){
						M.Mat [i, j] = A.m (i, j) + B.m (i, j);
					}
				}
			}
			return M;
		}


		public static float trace (matrix A){
			float tr = 0;
			for (int i = 0; i < A.R; i++) {
				tr += A.m (i, i);
			}
			return tr;
		}

		public static matrix I (int n){
			matrix I = new matrix(n,n);
			for (int i=0; i<n;i++){
				I.Mat [i, i] = 1;
			}
			return I;
		}

		public void Print(){
			string M = "";
			for (int i =0; i <this.R;i++){
				string entry = "|";
				for (int j = 0; j<this.C; j++){
					entry += this.Mat[i,j].ToString("R") + ", ";

				}
				entry += "|" + System.Environment.NewLine;
				M += entry;
			}
			print (M);
		}

		public float cof(int r, int c) {
			matrix COF = this.del (r, c);
			float cof = Mathf.Pow (-1, r + c)*det(COF);
			return cof;
		}

		public static matrix Inv(matrix A){
			matrix M = new matrix(A.R,A.R);
			if (det(A) != 0){
				float sc = 1f / det (A);
				for (int i=0; i <A.R;i++){
					for(int j = 0; j < A.R; j++) {
						M.Mat [i, j] = sc * A.cof (j, i);
					}
				}
			}
			return M;
		}
	}
}
