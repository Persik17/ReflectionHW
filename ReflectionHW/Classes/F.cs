namespace ReflectionHW.Classes
{
    public class F
    {
        public int I1 { get; set; }
        public int I2 { get; set; }
        public int I3 { get; set; }
        public int I4 { get; set; }
        public int I5 { get; set; }
        public int I6 { get; set; }

        public F() { }

        public static F Get() => new() { I1 = 1, I2 = 2, I3 = 3, I4 = 4, I5 = 5, I6 = 6 };
    }

}
