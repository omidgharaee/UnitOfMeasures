using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfMeasures.Core.Domain.Services;

namespace UnitOfMeasures.Core.Application.Services
{
    public class Converter : IConverter
    {
        private readonly IMathParser _mathParser;
        public Converter(IMathParser mathParser)
        {
            _mathParser = mathParser;
        }

        private decimal[,] daDataMatrix = null;
        public string Convert(string convertValue, Domain.Unit.Entities.Unit convertUnit, Domain.Unit.Entities.Unit intoUnit, List<Domain.Unit.Entities.Unit> units)
        {
            if (string.IsNullOrWhiteSpace(convertValue) || convertUnit == null && intoUnit == null)
            {
                return "";
            }

            if (intoUnit.Type == "Formula" || convertUnit.Type == "Formula")
            {
                double result = 0;
                if (intoUnit.Type == "Formula")
                {
                    var formula = intoUnit.FormulaFromBase;
                    formula = formula.Replace("a", convertValue);
                    result = _mathParser.Parse(formula);
                }
                else if (convertUnit.Type == "Formula")
                {
                    var formula = convertUnit.FormulaToBase;
                    formula = formula.Replace("a", convertValue);
                    result = _mathParser.Parse(formula);
                }

                return result + "  " + intoUnit.Abbreviation;
            }
            else
            {

                daDataMatrix = CreateDataMatrix(units);

                decimal dResult = 0.0m;
                int iIndex1 = 0, iIndex2 = 0;
                string sResult = "";

                try
                {
                    iIndex1 = units.FindIndex(u => u.Id == convertUnit.Id);
                    iIndex2 = units.FindIndex(u => u.Id == intoUnit.Id);
                    dResult = decimal.Parse(convertValue) * this.daDataMatrix[iIndex2, iIndex1];
                    sResult = dResult.ToString();

                    if (sResult.Length > 25)
                        sResult = sResult.Substring(0, 25);


                    sResult = sResult + "  " + intoUnit.Abbreviation;
                }
                catch (Exception ex)
                {
                    sResult = "";
                }

                return sResult;
            }
        }

        private decimal[,] CreateDataMatrix(List<Core.Domain.Unit.Entities.Unit> aalConversionObjects)
        {
            int iConvUnitCount = aalConversionObjects.Count;
            decimal[,] daMatrix = new decimal[iConvUnitCount, iConvUnitCount];
            string sCode = "";
            decimal dData = 0.0m, dInternalData = 0.0m;
            Core.Domain.Unit.Entities.Unit udUnitCode = null, udInternalCode = null;

            for (int i = 0; i < iConvUnitCount; i++)
            {
                udUnitCode = (Core.Domain.Unit.Entities.Unit)aalConversionObjects[i];
                sCode = udUnitCode.Abbreviation;
                dData = (decimal)udUnitCode.ConversionFactor;
                if (udUnitCode.ConversionFactor == 0)
                    break;

                for (int j = 0; j < iConvUnitCount; j++)
                {
                    udInternalCode = (Core.Domain.Unit.Entities.Unit)aalConversionObjects[j];
                    dInternalData = (decimal)udInternalCode.ConversionFactor;
                    daMatrix[i, j] = dInternalData / dData;
                }
            }

            return daMatrix;
        }

    }
}
