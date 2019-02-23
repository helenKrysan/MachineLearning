namespace MachineWPF.Practice2
{
    class Kernel
    {
        private char type;

        public char Type { get => type; set => type = value; }

        public Kernel(char type)
        {
            this.type = type;
        }

        public double KernelExec(double p)
        {
            if (p >= 1) return 0;
            double weight = 0;
            switch (type)
            {
                //square kernel
                case 'C':
                    {
                        weight = 1 - p * p;
                        break;
                    }

            }
            return weight;
        }


    }
}
