Public Class HREmployment
    Property PersonIdentifier As String
    Property CompanyIdentifier As String
    Property EmployeeNumber As String
    Property EmployeeCategory As String
    Property Position As String
    Property FromDate As String
    Property EndDate As String
    Property PositionPercent As String
    Property DepartmentIdentifier As String
End Class

'1)
'299;299;Fast stilling;Teknisk asnv.;2012-04-18;;100.0;105

'2)
'299;299;Fast stilling;Teknisk asnv.;2012-04-18;2013-06-26;100.0;105
'299;299;Fast stilling;Teknisk asnv.;2013-06-27;;70.0;105
'299;299;Fast stilling;Teknisk asnv.;2013-06-27;;30.0;412

'3)
'299;299;Fast stilling;Teknisk asnv.;2013-06-27;;70.0;105
'299;299;Fast stilling;Teknisk asnv.;2013-06-27;;30.0;412
'150;150;Fast stilling;Teknisk asnv.;2009-01-20;;100.0;105



'1 employment, 2 distributions
'299;299;Fast stilling;Teknisk asnv.;2013-06-27;;70.0;105
'299;299;Fast stilling;Teknisk asnv.;2013-06-27;;30.0;412

'1 employment, 1 distribution
'150;150;Fast stilling;Teknisk asnv.;2009-01-20;;100.0;105

'Position opprettes dersom det ikke benyttes SSB stillingskoder
'EmployeeCategory må være satt fra før av