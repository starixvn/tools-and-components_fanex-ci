<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:template match="/BuildSummary/Projects">
    <section name="Build summary" fontcolor="blue">
      <table>
        <tr>
          <td value="Project" fontattribute="bold"/>
          <td value="Error" fontattribute="bold"/>
          <td value="Warning" fontattribute="bold"/>          
        </tr>
        <xsl:apply-templates select="Project"/>
      </table>
    </section>
  </xsl:template>
  <xsl:template match="Project">
    <tr>
      <xsl:element name="td">
        <xsl:attribute name="value">
          <xsl:value-of select="Name"/>
        </xsl:attribute>
        <xsl:attribute name="fontcolor">black</xsl:attribute>
      </xsl:element>
      <xsl:element name="td">
        <xsl:attribute name="value">
          <xsl:value-of select="Error"/>
        </xsl:attribute>
        <xsl:if test="Error > 0">
          <xsl:attribute name="fontcolor">red</xsl:attribute>
        </xsl:if>
        <xsl:if test="Error = 0">
          <xsl:attribute name="fontcolor">blue</xsl:attribute>
        </xsl:if>
      </xsl:element>
      <xsl:element name="td">
        <xsl:attribute name="value">
          <xsl:value-of select="Warning"/>
        </xsl:attribute>
        <xsl:if test="Warning > 0">
          <xsl:attribute name="fontcolor">red</xsl:attribute>
        </xsl:if>
        <xsl:if test="Warning = 0">
          <xsl:attribute name="fontcolor">blue</xsl:attribute>
        </xsl:if>
      </xsl:element>      
    </tr>
  </xsl:template>
</xsl:stylesheet>