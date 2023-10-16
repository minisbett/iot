# Si5351 programmable clock generator

## Summary
The Si5351 is a multi-purpose clock generator that is controllable through an I2C interface. It supports non-integer-related frequencies in a range from 8 kHz to 160 MHz
at eachof its outputs. The output frequencies is sythesized in two configurable stages. The first stages uses a PLL to create a high-frequency intermediate clock from the
input stage (which actual clock source depends on the device version A/B/C). The second stage is a high resolution divider that creates the actual output frequency.

## Device Family
The Si5351 device family includes 3 versions and 3 packages. The versions differ on the supported input types for the input stage.
The package differ in the number of supported outputs.

|Version|Inputs                       |10 pin MSOP|16 pin QNF|20 pin QNF|
|-------|-----------------------------|-----------|----------|----------|
|Si5351A|XTAL                         | 3 outputs | 4 outputs| 8 outputs|
|Si5351B|XTAL, voltage control (VCXO) |    n/a    | 4 outputs| 8 outputs|
|Si5351C|XTAL, clock in (10-100 MHz)  |    n/a    | 4 outputs| 8 outputs|
**[Device family overview table]**

16 pin QNF NOT SUPPORTED???

**IMPORTANT**: Two revisions of the device family exist - Si5351 and Si5351-B (don't confuse with version B).
The revision B is the current revision and supports a slightly wider frequency range of 2.5 kHz to 200 MHz.
There might be further differences, too.
However, there is only a limited number of vendors for breakout boards and mostly they use the older revision and
device variant A with 3 outputs.
If you use a custom design you may use either revision with this binding. Though, it has only been tested with
the older revision of Si5351A with 3 outputs.

## Datasheets
**[Si5351]**: [https://www.silabs.com/documents/public/data-sheets/Si5351-B.pdf]
Revision B datasheet
HIER MUSS ICH NOCH EINMAL CHECKEN, OB MAN AUCH NOCH REGULÄR AN DAS ALTE KOMMT ODER WELCHE ALTERNATIVE QUELLE
VERWENDET WERDEN KANN

**[AN619]**: [https://www.silabs.com/documents/public/application-notes/AN619.pdf]

## Binding Notes

**NOTE**: Currently the Si5351 device binding does supports version A (XTAL only input) only.


## References
Provide any references to other tutorials, blogs and hardware related to the component that could help others get started.



THE INTERFACE DOES NOT REFLECT 1:1 THE INTERFACE GIVEN BY THE DATASHEET. IT HAS BEEN DESIGNED TO PROVIDE A HIGHER INTUITIVITY.
E.G. MANY SETTINGS FALSE / TRUE
