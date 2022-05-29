﻿using Easter.Models.Dyes.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easter.Models.Dyes
{
    public class Dye : IDye
    {
        private int dye;

        public Dye(int power)
        {
            Power = power;
        }

        public int Power
        {
            get
            {
                return dye;
            }
            private set
            {
                if (value < 0)
                {
                    value = 0;
                }
                dye = value;
            }
        }

        public bool IsFinished()
        {
            if (Power == 0)
            {
                return true;
            }
            return false;
        }

        public void Use()
        {
            Power -= 10;
            if (Power < 0)
            {
                Power = 0;
            }
        }
    }
}
