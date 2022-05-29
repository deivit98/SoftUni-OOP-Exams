using Formula1.Models.Contracts;
using Formula1.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Formula1.Models
{
    internal abstract class FormulaOneCar : IFormulaOneCar
    {
        private string model;
        private int horsepower;
        private double engineDisplacement;
        protected FormulaOneCar(string model, int horsepower, double engineDisplacement)
        {
            Model = model;
            Horsepower = horsepower;
            EngineDisplacement = engineDisplacement;
        }
        public double Points = 0;
        public string Model 
        {
            get
            {
                return this.model;
            }
            private set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 3)
                {
                    throw new ArgumentException(ExceptionMessages.InvalidF1CarModel,value);
                }
                this.model = value;
            } 
        }

        public int Horsepower 
        {
            get
            {
                return this.horsepower;
            }
            private set
            {
                if (value < 900 || value > 1050)
                {
                    throw new ArgumentException(ExceptionMessages.InvalidF1HorsePower, value.ToString());
                }
                this.horsepower = value;
            }
        }

        public double EngineDisplacement 
        {
            get
            {
                return this.engineDisplacement;
            }
            private set
            {
                if (value < 1.6 || value > 2.00)
                {
                    throw new ArgumentException(string.Format(ExceptionMessages.InvalidF1EngineDisplacement, value));
                }
                this.engineDisplacement = value;
            }
        }

        public double RaceScoreCalculator(int laps)
        {
            return this.EngineDisplacement / this.Horsepower * laps;
        }
    }
}
