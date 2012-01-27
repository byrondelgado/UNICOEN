package src;

public class GraphSample {
	public static void main(String[] args){
		Integer a = null;
		Integer b = 1;
		int c = 100;
		b = a;
		if(b == null){
			c = 11;
			b = 22;
		}else{
			c = 5;
			b = 25;
		}
		while(c < 10){
			c++;
			if(c = 9){
				b = 100;
			}else{
				b = 10;
			}
		}
		for(int i = 0; i < 20; i++){
			a = 10;
			b++;
		}
		b = null;
		c = 10;
	}
}

