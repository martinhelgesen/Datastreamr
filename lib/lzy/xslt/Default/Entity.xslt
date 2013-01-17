<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="text" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

  <xsl:template match="/TableSettings/DatabaseInfoName" />
  <xsl:template match="/TableSettings/TableName" />
  <xsl:template match="/TableSettings/FilePath" />

  <xsl:template match="/TableSettings/Table">

    Public Class <xsl:value-of select="Name"/>

    
    End Class


  </xsl:template>


</xsl:stylesheet>
