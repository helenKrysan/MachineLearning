namespace MachineWPF.Practice2
{
    class Core
    {
        private char type;

        public char Type { get => type; set => type = value; }

        public Core(char type)
        {
            this.type = type;
        }

        public double CoreExec(double p)
        {
            if (p >= 1) return 0;
            double weight = 0;
            switch (type)
            {
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
