using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CyBLE_MTK_Application
{
    class MFGTestSanityCheck
    {
        public static string SerialNumberUniqueCheck (string[] DUTserialnumbers, string[] DUTserialportsName)
        {
            
            for (int i= 0; i<DUTserialnumbers.Length; i++)
            {
                int SerialNumberUnique = 0;
                foreach (var val in DUTserialnumbers)
                {
                    
                    if (DUTserialnumbers[i] == val)
                    {
                        SerialNumberUnique++;
                    }
                    if (SerialNumberUnique > 1 && DUTserialportsName[i].ToLower() != "configure...")
                    {
                        MessageBox.Show("SN : "+ val.ToString() + " SerialNumberUniqueCheck FAIL (" + SerialNumberUnique.ToString() + ") ", "Warning: SerialNumberUniqueCheck FAIL", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        return val.ToString();
                    }
                    
                }
            }

            return "SerialNumberUniqueCheck: PASS!!!";

        }

        public static string SerialPortPredefineMappingCheck (string[] DUTserialportsName, string[] PredefineserialportsName)
        {
            for (int i = 0; i < DUTserialportsName.Length; i++)
            {
                if (DUTserialportsName[i].ToLower()!= PredefineserialportsName[i].ToLower()&& DUTserialportsName[i].ToLower() != "configure...")
                {
                    MessageBox.Show("DUTserialports #" + (i + 1).ToString() + " is not aligned with predefined. Configured Port name is : " + DUTserialportsName[i].ToLower() + "(Predefined:" + PredefineserialportsName[i].ToLower() + ")" , "Warning: SerialPortPredefineMappingCheck FAIL!" , MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return "SerialPortPredefineMappingCheck : FAIL !!!";
                }
            }

            return "SerialPortPredefineMappingCheck : PASS !!!";

        }


    }
}
