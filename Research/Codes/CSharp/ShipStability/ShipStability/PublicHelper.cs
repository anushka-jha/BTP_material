using System;
using System.Collections.Generic;
using System.Text;

namespace ShipStability
{
    class PublicHelper
    {
        #region Constructor

        public PublicHelper()
        {
        }
        #endregion

        #region public method

        public List<double> convertPointtoDouble(List<Point2D> pt2Dlist, int index)
        {
            List<double> dlist = new List<double>();

            if (index == 1)
            {
                foreach (Point2D pt in pt2Dlist)
                {
                    double d = pt.X;
                    dlist.Add(d);
                }
            }
            else
            {
                foreach (Point2D pt in pt2Dlist)
                {
                    double d = pt.Y;
                    dlist.Add(d);
                }

            }

            return dlist;
        }

        public List<double> convertPointtoDouble(List<Point> pt2Dlist, int index)
        {
            List<double> dlist = new List<double>();

            if (index == 1)
            {
                foreach (Point pt in pt2Dlist)
                {
                    double d = pt.X;
                    dlist.Add(d);
                }
            }
            else if (index == 2)
            {
                foreach (Point pt in pt2Dlist)
                {
                    double d = pt.Y;
                    dlist.Add(d);
                }

            }

            else
            {

                foreach (Point pt in pt2Dlist)
                {
                    double d = pt.Z;
                    dlist.Add(d);
                }

            }

            return dlist;
        }

        public Object GetCentroid ( List<Object> objList)
        {
            Object obj = new Object();
            double sum1 = 0;
            double sum2 = 0;
            double sum3 = 0;
            double sum4 = 0;
            for ( int i = 0; i< objList.Count; i++)
            {
                Point pt = objList[i].CG;
                sum1 = sum1 + objList[i].Weight;
                sum2 = sum2 + objList[i].Weight *pt.X;
                sum3 = sum3 + objList[i].Weight * pt.Y;
                sum4 = sum4 + objList[i].Weight * pt.Z;
            }

            obj.Weight = sum1;
            if(Math.Abs(sum1)<0.000000001)
            {
                obj.CG = new Point(0, 0, 0);
            }
            else
            {
                obj.CG = new Point(sum2 / sum1, sum3 / sum1, sum4 / sum1);
            }
            
            

            return obj;
        }

        public double GetIntValueforOdd(List<Point2D> pt2Dlist)
        {
            if (pt2Dlist.Count < 3)
            {
                return 0;
            }
            List<double> wtList = this.getWtlist(pt2Dlist);
            double sum = 0;
            for(int i =0; i<pt2Dlist.Count;i++)
            {
                sum = sum + pt2Dlist[i].Y * wtList[i];
            }

            return sum;
        }

        public double GetIntValueforEven(List<Point2D> ptList)
        {
            if(ptList.Count<1)
            {
                return 0;
            }
            double sum = 0;
            double sum1 = (0.5) * (ptList[1].X - ptList[0].X) * (ptList[1].Y + ptList[0].Y);
            if (ptList.Count < 3) 
            {
                sum = sum1;
                
            }
            else
            {
                List<Point2D> refList = this.refinePtlist(ptList);
                double sum2 = this.GetIntValueforOdd(refList);
                sum = sum1 + sum2;

            }
            
            
            return sum;
        }

        public double Trapizoidal (List<Point2D> ptList)
        {
            if(ptList.Count == 1)
            {
                return 0;
            }
            double sum = 0;
            for (int i=0; i<ptList.Count-1; i++)
            {
                sum = sum + (0.5) * (ptList[i + 1].X - ptList[i].X) * (ptList[i + 1].Y + ptList[i].Y);
            }

            return sum;
        }

        public double GetShipVol (List<SectionCurve> curveList)
        {
            List<Point2D> SecData = this.getSecAreaData(curveList);
            //if (SecData.Count % 2 == 0)
            //    return GetIntValueforEven(SecData);
            //else
            //    return GetIntValueforOdd(SecData);

                      return this.Trapizoidal(SecData);
        }

        public double GetSecArea ( List<Point2D> ptList)
        {
            double val = 0;
           if (ptList.Count % 2 == 0)
            {
                 val = this.GetIntValueforEven(ptList);
            }
              
            else
            {
                val  = this.GetIntValueforOdd(ptList);
            }
           

            double vl2 = this.Trapizoidal(ptList);

             return vl2;
        }

        public double GetSecAreaTrapz(List<Point2D> ptList)
        {
            double val = 0;
            if (ptList.Count % 2 == 0)
            {
                val = this.GetIntValueforEven(ptList);
            }

            else
            {
                val = this.GetIntValueforOdd(ptList);
            }


            double vl2 = this.Trapizoidal(ptList);

            return vl2;
        }

        public double Getmoment ( List<Section> secList, Point2D stPt, Point2D endpt, double maxdraft)
        {
            
            List<Point2D> ptList = new List<Point2D>();
            SectionCurve curve = new SectionCurve();
            for (int i = 0; i<secList.Count; i++)
            {
                
                List<Point2D> pointList = secList[i].SecXMoment;
                double draft = this.SolveLinearEquation(stPt, endpt, secList[i].Xlocation);
                double zvalue = curve.getUpdatedDrat(maxdraft, draft);
                double yvalue = this.InterpolateData(zvalue, pointList, 2);
                Point2D pt = new Point2D(secList[i].Xlocation, yvalue);
                ptList.Add(pt);
            }

            double value = this.GetSecArea(ptList);

            return value;
        }

        public double Getmoment(List<SectionCurve> curveList, double d)
        {
            List<Point2D> SecData = this.getSecAreaData(curveList);
            // SecData contains x, SecArea at that x;
            List<Point2D> moment = new List<Point2D>();
            for(int i = 0; i < SecData.Count; i++)
            {
                Point2D pt = new Point2D(SecData[i].X, (SecData[i].X -d)* SecData[i].Y);
                moment.Add(pt);
            }
            //moment contains x , Area*(x-xmid) and now we can find the net Lcb.
            //if (moment.Count % 2 == 0)
            //    return GetIntValueforEven(moment);
            //else
            //    return GetIntValueforOdd(moment);

            return this.Trapizoidal(moment);
        }

        public Point2D GetMaxMinElementInArray(List<double> ptList)
        {
            if(ptList.Count < 1)
            {
                return new Point2D(0, 0);
            }

            double[] array = ptList.ToArray();
            //int[] array = { 10, 30, 40, 100, 170, 50, 20, 60 };
            double max = array[0];
            double min = array[0];
            for (int i = 0; i <= array.Length - 1; i++)
            {
                if (array[i] > max)
                {
                    max = array[i];
                }
                if (array[i] < min)
                {
                    min = array[i];
                }
            }

            Point2D pt = new Point2D(min, max);

            return pt;

        }

        public Point2D GetMaxMinElementInArrayWithPointer(List<double> ptList, out int imin, out int imax)
        {
            imin = 0;
            imax = 0;
            if (ptList.Count < 1)
            {
                imin = 0;
                imax = 0;
                return new Point2D(0, 0);
            }

            double[] array = ptList.ToArray();
            //int[] array = { 10, 30, 40, 100, 170, 50, 20, 60 };
            double max = array[0];
            double min = array[0];
            for (int i = 1; i < array.Length ; i++)
            {
                if (array[i] > max)
                {
                    max = array[i];
                    imax = i;
                }
                if (array[i] < min)
                {
                    min = array[i];
                    imin = i;
                }
            }

            Point2D pt = new Point2D(min, max);

            return pt;

        }

        public double InterpolateData(double d, List<Point2D> pt2D, int nIndex)
        {
            double kn = 0;
            int n = this.getIndex(d, pt2D, nIndex);
            if (n == -1)
            {
                return 0;
            }
            if (n>pt2D.Count-2)
            {
                return pt2D[pt2D.Count - 1].Y;

            }

 

            if (nIndex == 2)
            {
                kn = pt2D[n].Y + ((pt2D[n + 1].Y - pt2D[n].Y) / (pt2D[n + 1].X - pt2D[n].X)) * (d - pt2D[n].X);
            }
            else
            {
                kn = pt2D[n].X + ((pt2D[n + 1].X - pt2D[n].X) / (pt2D[n + 1].Y - pt2D[n].Y)) * (d - pt2D[n].Y);
            }
            

            return kn;
        }

        public double InterpolateDataforsub(double d, List<Point2D> pt2D, int nIndex)
        {
            double kn = 0;
            int n = this.getIndex(d, pt2D, nIndex);
            if (n == -1)
            {
                return 0;
            }
            if (n > pt2D.Count - 2)
            {
                return 0;

            }



            if (nIndex == 2)
            {
                kn = pt2D[n].Y + ((pt2D[n + 1].Y - pt2D[n].Y) / (pt2D[n + 1].X - pt2D[n].X)) * (d - pt2D[n].X);
            }
            else
            {
                kn = pt2D[n].X + ((pt2D[n + 1].X - pt2D[n].X) / (pt2D[n + 1].Y - pt2D[n].Y)) * (d - pt2D[n].Y);
            }


            return kn;
        }

        public double SolveLinearEquation ( Point2D p1, Point2D p2, double xvalue)
        {
            if(Math.Abs(p2.X - p1.X)<0.0001)
            {
                return p1.X;
            }
            double slope = (p2.Y - p1.Y) / (p2.X - p1.X);
            double yvalue = p1.Y + slope * (xvalue - p1.X);
            return yvalue;
        }

        public double SolveLinearEquation (double slope, double c, double xvalue)
        {
            double yvalue = slope * xvalue + c;
            return yvalue;
        }
        public List<Point2D> GetDraftData(List<Point2D> ls,List<double> fn)
        {
            List<Point2D> ans=new List<Point2D>();
            for(int i = 0; i < ls.Count; i++)
            {
                Point2D pt = new Point2D(ls[i].X, Getdraftwisedata(ls, fn, i));
                ans.Add(pt);
            }
            return ans;
        }
        /// <summary>
        /// It Solves the equation x = a0 + a1*Y + a2*Y^2
        /// This is to draw a smooth curve between three point
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <param name="pt3"></param>
        /// <returns></returns>
        public Point SolveCoeffofQuadraticEquation(Point2D pt1,Point2D pt2, Point2D pt3)
        {
            
            
            double[,] y = new double[,] { { pt1.X }, { pt2.X }, { pt3.X } };
            double[,] x = new double[,] { { 1, pt1.Y, Math.Pow(pt1.Y, 2) }, { 1, pt2.Y, Math.Pow(pt2.Y, 2) }, { 1, pt2.Y, Math.Pow(pt2.Y, 2) } };
            Matrix mat1 = new Matrix( x);
            Matrix mat2 = new Matrix( y);
            Matrix mat3 = new Matrix();
            mat3.Matmul(mat3.Inverse(mat1),mat2);
            Point pt = new Point(mat3.Element[0, 0], mat3.Element[1, 0], mat3.Element[2, 0]);

            return pt;
            
        }
        /// <summary>
        /// It usees Sridharacharya Formula
        /// </summary>
        /// <param name="coeff"></param>
        /// this is the three co efiicient of the quadratic equation x = a0 + a1*Y + a2*Y^2
        /// <param name="xvalue"></param> 
        /// this is the value x for which I need y
        /// <returns></returns>
        public double QuadraticSolver ( Point coeff, double xvalue)
        {
            try
            {
                double eps = 0.000001;
                double d = coeff.Y * coeff.Y - 4 * coeff.Z * (coeff.X - xvalue);
                double y1 = (-coeff.Y + d) / (2 * coeff.Z);
                double y2 = (-coeff.Y + d) / (2 * coeff.Z);

                if (Math.Abs(y1 - 0) < eps)
                {
                    return 0;
                }
                else if ( y1 > 0)
                {
                    return y1;
                }
                else
                {
                    return y2;
                }

            }
            catch 
            {

                throw new Exception("Roots are not real");
            }
            
        }

        public List<Point2D> GetDensePointSet (SectionCurve curve)
        {
            List<Point2D> densePtset = new List<Point2D>();
            List<Point2D> ptList = curve.SectionPts;
            int kmax = ptList.Count - 3;
            for ( int i = 0; i<kmax;i++)
            {
                if(i ==kmax-1)
                {
                    Point2D p1 = ptList[i];
                    Point2D p2 = ptList[i + 1];
                    Point2D p3 = ptList[i + 2];
                    Point2D pst = p1;
                    Point2D pend = p3;
                    List<Point2D> dataList = this.getDataset(p1, p2, p3, pst, pend, 10, true);
                    densePtset.AddRange(dataList);

                }
                else
                {
                    Point2D p1 = ptList[i];
                    Point2D p2 = ptList[i + 1];
                    Point2D p3 = ptList[i + 2];
                    Point2D pst = p1;
                    Point2D pend = p2;
                    List<Point2D> dataList = this.getDataset(p1, p2, p3, pst, pend, 10, false);
                    densePtset.AddRange(dataList);
                }

            }



            return densePtset;
        }

        public double InverseSolveLinearEquation(Point2D p1, Point2D p2, double yvalue)
        {
            double slope = (p2.Y - p1.Y) / (p2.X - p1.X);
            double xvalue = p1.X + (yvalue - p1.Y) / slope;
            return xvalue;
        }

        public double InverseSolveLinearEquation(double slope, double c, double yvalue)
        {
            double xvalue = (yvalue - c) / slope;
            return xvalue;
        }
        /// <summary>
        /// This Program Solves Two Linear Equation
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public Point2D LinearSolver ( Vector2D v1, Vector2D v2)
        {
            Point2D solutionPt = new Point2D();
            Vector2D v = new Vector2D();

            // double[,] y = new double[,] { { pt1.X }, { pt2.X }, { pt3.X } };
            // double[,] x = new double[,] { { 1, pt1.Y, Math.Pow(pt1.Y, 2) }, { 1, pt2.Y, Math.Pow(pt2.Y, 2) }, { 1, pt2.Y, Math.Pow(pt2.Y, 2) } };

            double[,] c = new double[,] { { v2.StPt.X - v1.StPt.X }, { v2.StPt.Y - v1.StPt.Y } };
            double[,] a = new double[,] { { v1.EndPt.X - v1.StPt.X, -(v2.EndPt.X - v2.StPt.X) }, { v1.EndPt.Y - v1.StPt.Y, -(v2.EndPt.Y - v2.StPt.Y) } };
            Matrix m = new Matrix();
            Matrix m1 = new Matrix(c);
            Matrix m2 = new Matrix(a);
            Matrix m3 = m.Matmul(m.Inverse(m2), m1);
            double[,] d = m3.Element;
            double param = d[1, 0];

            solutionPt = v.GetPoint(v2, param);

            return solutionPt;
        }

        public Point2D GetCgfromPoint2DList(List<Point2D> ptList)
        {
            List<Point2D> momentList = new List<Point2D>();
            foreach(Point2D pt2D in ptList)
            {
                Point2D mpt = new Point2D (pt2D.X, pt2D.X * pt2D.Y);
                momentList.Add(mpt);
            }

            double wt = this.GetSecAreaTrapz(ptList);
            double mom = this.GetSecAreaTrapz(momentList);
            double lcg = mom / wt;
            return new Point2D(wt, lcg);
        }

        public Point2D GetCgfromPoint2DVcg(List<Point2D> ptList)
        {
            List<Point2D> momentList = new List<Point2D>();
            foreach (Point2D pt2D in ptList)
            {
                Point2D mpt = new Point2D(pt2D.X, (0.5)*(pt2D.Y) * pt2D.Y);
                momentList.Add(mpt);
            }

            double wt = this.GetSecAreaTrapz(ptList);
            double mom = this.GetSecAreaTrapz(momentList);
            double lcg = mom / wt;
            return new Point2D(wt, lcg);
        }

        public double GetAreaofPolygon ( List<Point2D> pointList)
        {
            double sum = 0;

            for(int i = 0; i< pointList.Count; i++)
            {
                double x = pointList[(i + 1) % pointList.Count].X - pointList[i].X;
                double y = pointList[(i + 1) % pointList.Count].Y + pointList[i].Y;

                sum = sum + (0.5) * x * y;
            }



            return Math.Abs(sum);
        }

        public Point2D GetCentriodofPolygon( double area, List<Point2D> ptlist)
        {
            Point2D pt = new Point2D();
            double tsum1 = 0;
            double zsum2 = 0;
            double sum = 0;
            for ( int i = 0; i <ptlist.Count; i++)
            {
                double d = ptlist[i].X * ptlist[(i + 1) % (ptlist.Count)].Y - ptlist[i].Y * ptlist[(i + 1) % (ptlist.Count)].X;
                tsum1 = tsum1 + (ptlist[i].X + ptlist[(i + 1) % (ptlist.Count)].X) * d;
                zsum2 = zsum2 + (ptlist[i].Y + ptlist[(i + 1) % (ptlist.Count)].Y) * d;
                sum = sum + ptlist[i].X;
            }

            pt.X = Math.Abs((0.167) * (tsum1 / area));
            pt.Y = Math.Abs((0.167) * (zsum2 / area));


            return pt;
        }

        //public Point2D GetCentriodofPolygon ( List<Point2D> ptList)
        //{
        //    double sum1 = 0;
        //    double sum2 = 0;

        //    for ( int i = 0; i<ptList.Count; i++)
        //    {
        //        sum1 = sum1 + ptList[i].X;
        //        sum2 = sum2 + ptList[i].Y;
        //    }

        //    Point2D pt = new Point2D(sum1 / ptList.Count, sum2 / ptList.Count);


        //    return pt;
        //}

        
       

        #endregion

        #region private method

        #region InterpolateData

        private int getIndex(double d, List<Point2D> dataList, int nIndex)
        {
            if(dataList.Count == 0)
            {
                return -1;
            }
            int key = new int();
            key = 0;

            if( nIndex == 1)
            {
                if(d< dataList[0].Y)
                {
                    return -1;
                }
                for (int i = 0; i < dataList.Count; i++)
                {
                    double dummy = d - dataList[i].Y;

                    if (dummy > 0)
                    {
                        key = i;
                    }
                    else if (dummy <0)
                    {
                        break;
                    }

                }

            }
            else
            {
                if (d < dataList[0].X)
                {
                    return -1;
                }

                for (int i = 0; i < dataList.Count; i++)
                {
                    double dummy = d - dataList[i].X;

                    if (dummy > 0)
                    {
                        key = i;
                    }
                    else if (dummy<0)
                    {
                        break;
                    }

                }

            }

  

            return key;
        }

        #endregion InterpolateData

        #region GetIntValueforOdd

        private List<double> getWtlist (List<Point2D> pt2Dlist)
        {
            List<double> wtList = new List<double>();
            List<double> xvalues = new List<double>();
            for ( int i =0; i<pt2Dlist.Count; i++)
            {
                double y = pt2Dlist[i].X;
                xvalues.Add(y);
            }


            wtList = this.getxWList(xvalues);

            return wtList;
        }

        private double Getdraftwisedata(List<Point2D> ls, List<double> fn,int idx)
        {
            if (idx == 0)
                return 0;
            double ans = 0;
            for(int i = 1; i <= idx; i++)
            {
                ans += 0.5 * (ls[i].X - ls[i - 1].X) * (ls[i].Y +ls[i-1].Y ) * fn[i];
            }
            return ans;
        }
        private List<double> getxWList(List<double> xvallist)
        {
            List<double> xlist = new List<double>();
            List<int> sympwt = this.sympWtList(xvallist.Count);
            double factor = 1/ (3.0);
            xlist.Add(factor* (xvallist[1] - xvallist[0]) * sympwt[0]);
            //xlist.Add(sympwt[0]);
            for ( int i = 1; i<xvallist.Count-1; i++)
            {
                xlist.Add(factor*(xvallist[i] - xvallist[i-1]) * sympwt[i]);
            }

            xlist.Add(factor* (xvallist[xvallist.Count-1] - xvallist[xvallist.Count -2])*sympwt[sympwt.Count-1]);

            return xlist;
        }

        private List<int> sympWtList(int count)
        {
            List<int> sympwt = new List<int>();

            if (count == 3)
            {
                sympwt.Add(1);
                sympwt.Add(4);
                sympwt.Add(1);
            }
            else
            {
                sympwt.Add(1);
                sympwt.Add(4);
                for(int i = 2; i<count-1; i++)
                {
                    if(i%2 == 0)
                    {
                        sympwt.Add(2);
                    }
                    else
                    {
                        sympwt.Add(4);
                    }
                }

                sympwt.Add(1);
            }

            return sympwt;
        }



        #endregion GetIntValueforOdd

        #region GeiIntValueforEven
        private List<Point2D> refinePtlist ( List<Point2D> ptlist)
        {
            List<Point2D> refPtlist = new List<Point2D>();
            for ( int i = 1; i<ptlist.Count; i++)
            {
                refPtlist.Add(ptlist[i]);
            }
            return refPtlist;
        }
        #endregion GeiIntValueforEven

        #region GetDensePtset
        private List<Point2D> getDataset(Point2D p1, Point2D p2, Point2D p3, Point2D fromPt,Point2D toPt, int ndiv, bool addLastPt)
        {

            List<Point2D> ptList = new List<Point2D>();
            double dx = (toPt.X - fromPt.X) / ndiv;
            List<double> xList = this.getxRange(fromPt, toPt, dx, ndiv, addLastPt);

            if (this.checforquadFit(p1, p2, p3))
            {
                
                Point coEff = this.SolveCoeffofQuadraticEquation(p1, p2, p3);
                for (int i = 0; i < xList.Count; i++)
                {
                    double y = this.QuadraticSolver(coEff, xList[i]);
                    ptList.Add(new Point2D(xList[i], y));
                }

            }
            else
            {
                foreach(double draft in xList)
                {
                    double y = this.SolveLinearEquation(fromPt, toPt, draft);
                    ptList.Add(new Point2D(draft, y));
                }

            }
            

            return ptList;
        }

        private bool checforquadFit( Point2D p1, Point2D p2, Point2D p3)
        {
            return false;

        }

        private List<double> getxRange (Point2D stPt, Point2D endPt, double dx, int ndiv, bool value)
        {
            List<double> xList = new List<double>();

            for ( int i = 0; i < ndiv; i++)
            {
                xList.Add(stPt.X + i * dx);
            }

            if(value)
            {
                xList.Add(endPt.X);
            }

            return xList;
        }
        #endregion GetDensePtset

        private List<Point2D> getSecAreaData(List<SectionCurve> curveList)
        {
            List<Point2D> SecData = new List<Point2D>();
            for (int i = 0; i < curveList.Count; i++)
            {
                Point2D secData = new Point2D(curveList[i].Xloc,GetSecArea(curveList[i].SectionPts));
                SecData.Add(secData);

            }

            
            return SecData;
        }
        private double GetArea(List<Point2D> pts)
        {
            double ar1 = 0.5 * (pts[0].Y + pts[1].Y) * (pts[1].X - pts[0].X);
            double ar2 = 0.5 * (pts[1].Y + pts[2].Y) * (pts[2].X - pts[1].X);
            double ar3 = 0.5 * (pts[2].Y + pts[3].Y) * (pts[3].X - pts[2].X);
            double ar4 = 0.5 * (pts[3].Y + pts[0].Y) * (pts[0].X - pts[3].X);

            return Math.Abs(ar1+ar2+ar3+ar4);
        }
            #endregion


    }

}
