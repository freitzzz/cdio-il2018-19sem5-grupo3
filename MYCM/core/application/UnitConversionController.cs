using System;
using System.Collections.Generic;
using core.modelview.unitconversion;
using core.services;

namespace core.application
{
    /// <summary>
    /// Application controller for converting units.
    /// </summary>
    public class UnitConversionController
    {
        public UnitConversionController() { }

        /// <summary>
        /// Retrieves all the available units.
        /// </summary>
        /// <returns>GetUnitsModelView with data from all the available units.</returns>
        public GetUnitsModelView getAllAvailableUnits()
        {
            IEnumerable<string> availableUnits = MeasurementUnitService.getAvailableUnits();
            GetUnitsModelView unitsModelView = new GetUnitsModelView();

            foreach(string unit in availableUnits){
                GetUnitModelView unitModelView = new GetUnitModelView();
                unitModelView.unit = unit;
                unitsModelView.Add(unitModelView);
            }

            return unitsModelView;
        }

        /// <summary>
        /// Converts the value from a unit to another unit.
        /// </summary>
        /// <param name="convertUnitMV">ConvertUnitModelView containing unit conversion data.</param>
        /// <returns>ConvertUnitModelView with converted value.</returns>
        public ConvertUnitModelView convertValue(ConvertUnitModelView convertUnitMV)
        {
            double valueInMM = MeasurementUnitService.convertFromUnit(convertUnitMV.value, convertUnitMV.fromUnit);

            double newValue = MeasurementUnitService.convertToUnit(valueInMM, convertUnitMV.toUnit);

            convertUnitMV.value = newValue;

            return convertUnitMV;
        }
    }
}