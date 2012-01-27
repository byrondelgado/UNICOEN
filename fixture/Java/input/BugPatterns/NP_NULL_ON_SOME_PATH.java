package src;

public class NP_NULL_ON_SOME_PATH {
	public static void main(String[] args){
		int n = 11;
		Boolean s = false;

		if(n < 10){
			s = null;
		}else{
			s = true;
		}

		if(s){	//NP_NULL_ON_SOME_PATH	優先度 髁E
			System.out.println("STRING");
		}
	}
}
