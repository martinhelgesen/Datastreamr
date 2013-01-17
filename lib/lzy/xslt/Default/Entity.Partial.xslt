<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="text" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

  <xsl:template match="/TableSettings/DatabaseInfoName" />
  <xsl:template match="/TableSettings/TableName" />
  <xsl:template match="/TableSettings/FilePath" />

  <xsl:template match="/TableSettings/Table">

    Public Partial Class <xsl:value-of select="Name"/>

    <xsl:for-each select="Columns/ColumnInfo" >
      Privat _<xsl:value-of select="Name"></xsl:value-of> as <xsl:value-of select="NetRuntimeType"/>
    </xsl:for-each>

    Public ReadOnly Property Fields as string() = {<xsl:for-each select="Columns/ColumnInfo" ><xsl:if test="position()&gt;1">,</xsl:if>"<xsl:value-of select="Name" />"</xsl:for-each>}
        
    <xsl:for-each select="Columns/ColumnInfo" >
      Public Property <xsl:value-of select="Name"/> as <xsl:value-of select="NetRuntimeType"/>
        Get
          return _<xsl:value-of select="Name"/>
        End Get
        Set(value as <xsl:value-of select="NetRuntimeType"/>)
          _<xsl:value-of select="Name" /> = value
        End Set
      End Property
    </xsl:for-each>
    
    End Class


  </xsl:template>


</xsl:stylesheet>
