using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour {
	LineRenderer liney;
	Mesh meshy;
	float norm2 = 1/Mathf.Sqrt(2f);
	Vector3[] memory = new Vector3[101];
	// Use this for initialization
	void Start () {
		Vector3 A =new  Vector3(3,1,2);
		Vector3 B =new Vector3(2,3,3);
		liney = GetComponent<LineRenderer>() ;

		liney.numPositions=101;
		memory = geo(A,B);
		liney.SetPositions (memory);
		matrix example = matrix.I (5);
	}
	
	// Update is called once per frame
	void Update () {
		float dt = Time.deltaTime;
		float t = Time.time;
		/*for (int i = 0; i < 101; i++) {
			Quaternion q = new Quaternion (0, 0, 0, 0);
			q.w = liney.GetPosition(i).x;
			q.x = -liney.GetPosition (i).z;
			q.y = liney.GetPosition (i).y;
			Quaternion a = new Quaternion (0,0,0,1);
			Quaternion b = new Quaternion (0,0,0,0);
			Quaternion c = new Quaternion (dt,dt,0,-dt);
			Quaternion d = new Quaternion (0,0,0,1);
			Quaternion first = a * q;
			Quaternion Numer =new Quaternion (first.x+b.x,first.y+b.y,first.z+b.z,first.w+b.w);
			Quaternion Denom = c*q;
			Quaternion Denomer = new Quaternion(Denom.x+d.x,Denom.y+d.y,Denom.z+d.z,Denom.w+d.w);
			Denomer = invert (Denomer);
			q = Numer*Denomer;
			Vector3 Vec = new Vector3 ( q.w , q.y , q.x);
			liney.SetPosition(i,Vec );
			if (counter < 1) {
				print ("pos");
				print (q);
				print ("Numer");
				print (Numer);
				print ("Denomer");
				print (Denomer);
				counter++;
			}
			*/

		for (int i = 0; i < 101; i++) {
			Quaternion q = new Quaternion (0, 0, 0, 0);
			float sin = Mathf.Sin (t/2f);
			float cos = Mathf.Cos (t/2f);
			float tan = sin / cos;
			float x = memory[i].x;
			float y = -memory[i].z;
			float r = memory[i].y;
			q.Set(y,r,0,x);
			Quaternion a = new Quaternion (0,0,0,1);
			Quaternion b = new Quaternion (0,0,0,0);
			Quaternion c = new Quaternion (0,0,0,sin);
			Quaternion d = new Quaternion (0,0,0,1);
			float sc = sqlen (c) * sqlen (q) + sqlen (d) + 2 * Re (cmult(c,q,conj(d)));
			sc = 1f / sc;
			Quaternion Q = new Quaternion (0,0,0,0);
			Quaternion compp = scale(  sc,( ad(    scale(sqlen(q), cmult(a, conj(c)))    ,    cmult(b,conj(q),conj(c))   ,new Quaternion(0,r,0,0),   cmult(a,q,conj(d))   ,   cmult(b,conj(d)) ) ) );
			Q.w = Re(compp);
			Q.x = Im (compp);
			Q.y = r * sc;
			Vector3 Vec = new Vector3 (Q.w,Q.y,-Q.x);
			/*Quaternion first = a * q;
			Quaternion Numer =new Quaternion (first.x+b.x,first.y+b.y,first.z+b.z,first.w+b.w);
			Quaternion Denom = c*q;
			Quaternion Denomer = new Quaternion(Denom.x+d.x,Denom.y+d.y,Denom.z+d.z,Denom.w+d.w);
			Denomer = invert (Denomer);
			q = Numer*Denomer;
			Vector3 Vec = new Vector3 ( q.w , q.y , -q.x);
			liney.SetPosition(i,Vec );
			if (counter < 1) {
				print ("pos");
				print (q);
				print ("Numer");
				print (Numer);
				print ("Denomer");
				print (Denomer);
				counter++;
			}*/
			liney.SetPosition (i, Vec);
		}

		
	}

	Quaternion ad (Quaternion a, Quaternion b)
	{
		Quaternion ad = new Quaternion (a.x+b.x,a.y+b.y,a.z+b.z,a.w+b.w);
		return ad;
	}

	Quaternion ad (Quaternion a, Quaternion b, Quaternion c )
	{
		Quaternion ad = new Quaternion (a.x+b.x+c.x, a.y+b.y+c.y, a.z+b.z+c.z ,a.w+b.w+c.w);
		return ad;
	}

	Quaternion ad (Quaternion a, Quaternion b, Quaternion c, Quaternion d )
	{
		Quaternion ad = new Quaternion (a.x+b.x+c.x+d.x,a.y+b.y+c.y+d.y,a.z+b.z+c.z+d.z,a.w+b.w+c.w+d.w);
		return ad;

	}

	Quaternion ad (Quaternion a, Quaternion b, Quaternion c, Quaternion d, Quaternion e )
	{
		Quaternion ad = new Quaternion (a.x+b.x+c.x+d.x+e.x,a.y+b.y+c.y+d.y+e.y,a.z+b.z+c.z+d.z+e.z,a.w+b.w+c.w+d.w+e.w);
		return ad;

	}

	Quaternion cmult(Quaternion a, Quaternion b){
		float a1 = a.w;
		float a2 = a.x;
		float b1 = b.w;
		float b2 = b.x;
		float py = a1*b2+a2*b1;
		float px = a1 * b1 - a2 * b2;
		Quaternion prod = new Quaternion (py,0,0,px);
		return prod;
	}

	Quaternion cmult(Quaternion a, Quaternion b, Quaternion c){
		float a1 = a.w;
		float a2 = a.x;
		float b1 = b.w;
		float b2 = b.x;
		float c1 = c.w;
		float c2 = c.x;
		float py = a1*b2+a2*b1;
		float px = a1 * b1 - a2 * b2;
		float p2 = px * c2 + py * c1;
		float p1 = px * c1 - py * c2;
		Quaternion prod = new Quaternion (p2,0,0,p1);
		return prod;
	}

	float Re(Quaternion q){
		Quaternion cq = conj (q);
		Quaternion Req = ad(cq,q);
		float Re = Req.w/2f;
		return Re;
	}

	float Im(Quaternion q){
		Quaternion cq = conj(q);
		Quaternion Imq = ad(q,scale(-1f,cq));
		float Im = Imq.x / 2f;
		return Im;
	}

	float sqlen (Quaternion Q){
		float qs = Mathf.Pow(Q.x,2)+Mathf.Pow(Q.y,2)+Mathf.Pow(Q.z,2)+Mathf.Pow(Q.w,2);
		return qs;
	}

	Quaternion conj (Quaternion Q){
		Quaternion W = new Quaternion (-1*Q.x,-1*Q.y,-1*Q.z,Q.w);
		return W;
	}

	Quaternion scale(float t, Quaternion q){
		Quaternion p = new Quaternion (t*q.x,t*q.y,t*q.z,t*q.w);
		return p;
	}

	Quaternion invert(Quaternion q){
		float ql = sqlen (q);
		Quaternion cq = new Quaternion (0,0,0,0);
		cq = conj (q);
		Quaternion Q = new Quaternion(cq.x/ql,cq.y/ql,cq.z/ql,cq.w/ql);
		return Q;
	
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
						M.Mat [i, j] = sc * A.cof (i, j);
					}
				}
			}
			return M;
		}
	}

	Vector3[] geo(Vector3 p1, Vector3 p2){
		Vector3[] vert = new Vector3[101];
		float x1 = 0;
		float x2 = p2.x-p1.x;
		float y1 = 0;
		float y2 = -p2.z+p1.z;
		float z1 = p1.y;
		float z2 = p2.y;
		float thet = Mathf.Acos ((x2)/(Mathf.Pow(x2*x2+y2*y2,.5f)));
		matrix so = new matrix(3,3);
		so.Mat[0,0] = Mathf.Cos(thet);
		so.Mat [0, 1] = -Mathf.Sin (thet);
		so.Mat [1, 0] = Mathf.Sin (thet);
		so.Mat [1, 1] = Mathf.Cos (thet);
		so.Mat [2, 2] = 1;
		matrix P1 = new matrix (3, 1);
		P1.Mat [0, 0] = x1;
		P1.Mat [1, 0] = y1;
		P1.Mat [2, 0] = z1;
		matrix P2 = new matrix (3, 1);
		P2.Mat [0, 0] = x2;
		P2.Mat [1, 0] = y2;
		P2.Mat [2, 0] = z2;
		P2 = so * P2;
		float cent = Mathf.Pow(P2.m(2,0),2) - Mathf.Pow(P1.m(2,0),2);
		cent = cent/(2*(P2.m(0,0) - P1.m(0,0)));
		cent += (P2.m(0,0)+P1.m(0,0))/2;
		float r = cent*cent + P1.m(2,0)*P1.m(2,0);
		r = Mathf.Pow (r, .5f);
		float t0 = Mathf.Acos((P1.m(0,0)-cent)/r);
		float t1 = Mathf.Acos((P2.m(0,0)-cent)/r);
		for (int i =0; i< 101; i++){
			float t = t0 + 2 * Mathf.PI * i / 100f;
			float x = cent + r * Mathf.Cos (t);
			float z = r * Mathf.Sin (t);
			matrix P = new matrix(3,1);
			P.Mat [0, 0] = x;
			P.Mat [1, 0] = 0;
			P.Mat [2, 0] = z;
			P = matrix.Inv (so) * P;
			P.Mat [0, 0] += p1.x;
			P.Mat [2, 0] += -p1.z;
			//vert[i] =new  Vector3(P.m(0,0),P.m(2,0),-P.m(1,0));
			vert[i] = Vector3.zero;
			vert [i].x = P.m (0, 0);
			vert [i].y = P.m (2, 0);
			vert [i].z = -P.m (1, 0);
		}
		return vert;
	}



}
