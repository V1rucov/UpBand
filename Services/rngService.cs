using System;

namespace upband{
    public interface IRng{
    public Random r{get;set;}
    public int generateNumber();
}
    public class rng : IRng{
        public Random r{get;set;}
        public rng(){
            r = new Random();
        }
        public int generateNumber(){
            return r.Next(100000,999999);
        }
    }
}