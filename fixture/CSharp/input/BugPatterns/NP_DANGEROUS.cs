using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class NP_DANGEROUS {
    public static void Main() {
        var cl = new CLASS();
        bool boolean = cl.method(null, null);

        if (boolean == true) {
            Console.WriteLine("HELLO");
        }
        else {
            Console.WriteLine("NOOOO");
        }
    }
}

class CLASS {
    public Boolean method(int? n, int? m) {
        if (n + m > 10) {
            return false;
        }
        else {
            return true;
        }
    }
}
