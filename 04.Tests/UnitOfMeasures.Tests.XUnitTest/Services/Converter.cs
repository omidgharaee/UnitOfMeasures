using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitOfMeasures.Tests.XUnitTest.Services
{
    public class Converter
    {
        [Theory]
        [InlineData("10", "10000  m")]
        [InlineData("87", "87000  m")]
        public void ConvertLenght(string convertValue, string expected)
        {
            Core.Application.Services.MathParser mathParser = new Core.Application.Services.MathParser();
            Core.Application.Services.Converter converter = new Core.Application.Services.Converter(mathParser);


            var lenghtUnits = new List<Core.Domain.Unit.Entities.Unit>()
            {
                new Core.Domain.Unit.Entities.Unit(Guid.NewGuid())
                {
                    Name = "Metre",
                    Abbreviation = "m",
                    ConversionFactor = 1,
                },
                new Core.Domain.Unit.Entities.Unit(Guid.NewGuid())
                {
                    Name = "Kilometre",
                    Abbreviation = "km",
                    ConversionFactor = 1000,
                },
            };




            var resultLenght = converter.Convert(convertValue, lenghtUnits[1], lenghtUnits[0], lenghtUnits);


            Assert.Equal(expected, resultLenght);

        }


        [Theory]
        [InlineData("25", "77  f", true)]
        [InlineData("200", "392  f", true)]
        [InlineData("64", "17.77777777777778  c", false)]
        public void ConvertTemperature(string convertValue, string expected, bool fromBase)
        {
            Core.Application.Services.MathParser mathParser = new Core.Application.Services.MathParser();
            Core.Application.Services.Converter converter = new Core.Application.Services.Converter(mathParser);

            var temperatureUnits = new List<Core.Domain.Unit.Entities.Unit>()
            {
                new Core.Domain.Unit.Entities.Unit(Guid.NewGuid())
                {
                    Name = "Celsius",
                    Abbreviation = "c",
                    ConversionFactor = 1,
                },
                new Core.Domain.Unit.Entities.Unit(Guid.NewGuid())
                {
                    Name = "Fahrenheit",
                    Abbreviation = "f",
                    Type  = "Formula",
                    FormulaFromBase = "(a*9/5)+32",
                    FormulaToBase = "(a-32)*5/9"
                },
            };

            string resultTemperature = string.Empty;

            if (fromBase)
            {
                resultTemperature = converter.Convert(convertValue, temperatureUnits[0], temperatureUnits[1], temperatureUnits);
            }
            else
            {
                resultTemperature = converter.Convert(convertValue, temperatureUnits[1], temperatureUnits[0], temperatureUnits);
            }

            Assert.Equal(expected, resultTemperature);
        }

    }
}
