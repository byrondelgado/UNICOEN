package src;

public class NP_NULL_PARAM_DEREF_ALL_TARGETS_DANGEROUS {
	public static void main(String[] args){
		CLASS cl = new CLASS();
		CLASS2 cl2 = new CLASS2();

		Integer in = null;

		boolean bool;
		bool = cl.method(in, in);	//NP_NULL_PARAM_DEREF_ALL_TARGETS_DANGEROUS	—Dæ“x ’†
		cl2.method2(in, null);

		if(bool == true){
			System.out.println("HELLO");
		}else{
			System.out.println("NOOOO");
		}
	}
}

class CLASS{
	Boolean method(Integer n, Integer m){
		if(n + m > 10){
			return false;
		}else{
			return true;
		}
	}
}

class CLASS2{
	void method2(Integer n, Integer m){
		if(n + m < 10){
			System.out.println("Java");
		}else{
			System.out.println("C");
		}
	}
}
