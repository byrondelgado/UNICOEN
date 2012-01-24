using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class NP_SOMEPATH {
    public static void Main() {
        int n = 11;
        bool? s = false;
        if (n > 10) {
            s = null;
        }
        else if (n < 10) {
            s = null;
        }
        else {
            s = true;
        }

        if (s == true) {
            Console.WriteLine("STRING");
        }
    }
}