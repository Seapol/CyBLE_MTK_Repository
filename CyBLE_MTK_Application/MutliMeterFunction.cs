using System;
using System.Collections.Generic;
using System.Text;
//using CypressSemiconductor.ChinaManufacturingTest.Work_Loop;
using System.Diagnostics;
using System.Windows.Forms;

namespace CypressSemiconductor.ChinaManufacturingTest
{
    class MutliMeterFunction
    {


        private MultiMeter mm;




        private List<double> current;



        //##################################################################################################//


        public void initialize()
        {
            
            try
            {
                mm = new MultiMeter("U3606A");

                current = new List<double>();


            }
            catch (Exception ex)
            {
                
                MessageBox.Show("MultiMeter U3606A ==> Error: " + ex.Message);

            }
        }
        
        
        public double ReadCurrent()
        {
            double curr = mm.MeasureChannelCurrent().average;
            return curr;
        }
        
        public void idle()
        {
            //Trace.WriteLine("==> In Function idle.");
            //do nothing
        }



    }
}
