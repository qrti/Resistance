using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Resistance
{
    class Program
    {
        public static double[] e3 = {1.0, 2.2, 4.7};
        public static double[] e6 = {1.0, 1.5, 2.2, 3.3, 4.7, 6.8};
        public static double[] e12 = {1.0, 1.2, 1.5, 1.8, 2.2, 2.7, 3.3, 3.9, 4.7, 5.6, 6.8, 8.2};
        public static double[] e24 = {1.0, 1.1, 1.2, 1.3, 1.5, 1.6, 1.8, 2.0, 2.2, 2.4, 2.7, 3.0,
                                      3.3, 3.6, 3.9, 4.3, 4.7, 5.1, 5.6, 6.2, 6.8, 7.5, 8.2, 9.1};
        
        public static string[] id = {"Ohm", "KOhm", "MOhm"};

        static void Main(string[] args)
        {
            int e = 12;             // E12
            int pot = 8;            // 10^0 10^(pot+1) - 1
            int im, jm, spm;
            double v, vs, vp, pi, pj, pim, pjm, vmin;
            bool erow;
            double[] ex = new double[e];

            for(int i=0; i<e; i++)
                ex[i] = e12[i];

            do{
                im=jm=spm=0;
                v=vs=vp=pi=pj=pim=pjm=0;
                vmin=double.MaxValue;
                erow=false;

                Console.Write("target R in Ohm : ");
                string s = Console.ReadLine();

                if(!double.TryParse(s, out v))
                    Environment.Exit(0);

                for(int i=0; i<e; i++){
                    for(int ppi=0; ppi<pot; ppi++){          
                        pi = Math.Pow(10, ppi);

                        if(ex[i]*pi == v){
                            erow = true;
                             Console.WriteLine("                  R from normal row");
                        }
                    }
                }

                if(!erow){
                    for(int ppi=0; ppi<pot; ppi++){          
                        pi = Math.Pow(10, ppi);
                
                        for(int i=0; i<e; i++){
                            for(int ppj=0; ppj<pot; ppj++){
                                pj = Math.Pow(10, ppj);

                                for(int j=0; j<e; j++){
                                    vs = ex[i]*pi + ex[j]*pj;
                                    vp = (ex[i]*pi * ex[j]*pj) / (ex[i]*pi + ex[j]*pj);
                            
                                    if(Math.Abs(v-vs) < Math.Abs(vmin)){
                                        spm = 0;
                                        vmin = Math.Abs(v-vs);
                                        im=i; jm=j; pim=pi; pjm=pj;
                                    }

                                    if(Math.Abs(v-vp) < Math.Abs(vmin)){
                                        spm = 1;
                                        vmin = Math.Abs(v-vp);
                                        im=i; jm=j; pim=pi; pjm=pj;
                                    }
                                }
                            }
                        }
                    }
                    
                    double ra = ex[im] * pim;
                    double rb = ex[jm] * pjm;
                    int aa = 0;
                    int bb = 0;

                    while(ra>=1000 && aa<3){
                        ra /= 1000;
                        aa++;
                    }

                    while(rb>=1000 && bb<3){
                        rb /= 1000;
                        bb++;
                    }

                    Console.WriteLine("best combination: {0} {1} {2} {3} {4}", ra, id[aa], spm==0?"+":"|", rb, id[bb]);
                    Console.WriteLine("deviance        : {0} Ohm = {1} %", Math.Round(vmin, 0), Math.Round((vmin*100)/v, 1));
                }

                Console.WriteLine("------------------------------------");
            }while(true);
        }
    }
}
