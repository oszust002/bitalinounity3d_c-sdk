using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class CSV_Parser
{
    /// <summary>
    /// Convert the frame text into CSV format
    /// </summary>
    /// <param name="line">Text who need to be convert</param>
    /// <param name="analogChannelSize">Number of elements read</param>
    /// <returns>Return the result of the convertion</returns>
    internal static string ToCSV ( string line, int analogChannelSize )
    {
        int i = 0;

        if ( line.Contains ( "CRC" ) )
        {
            string result = "";

            string[] cutLine = line.Split ( ' ' );

            result = cutLine [ 0 ];

            foreach ( string part in cutLine )
            {
                if ( part.Contains ( ';' ) )
                {
                    string[] cutData = part.Split ( ';' );

                    foreach ( string data in cutData )
                    {
                        if ( i < analogChannelSize )
                        {
                            result += ";" + data;

                            i++;
                        }
                    }
                }
            }

            result += ";" + cutLine [ cutLine.Length - 1 ];

            result = result.Replace ( ".", "," );

            return result;
        }

        return null;
    }
}

