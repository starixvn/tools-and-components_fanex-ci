<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslHelper="urn:XslTransformHelper" xmlns:fn="http://www.w3.org/2005/xpath-functions" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata" xmlns:msprop="urn:schemas-microsoft-com:xml-msprop" xmlns:mstns="http://stylecopcmd.sourceforge.net/StyleCopReport.xsd" xmlns:n1="urn:schemas-microsoft-com:xml-msdatasource" xmlns:xdt="http://www.w3.org/2005/xpath-datatypes" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:altova="http://www.altova.com">
  <xsl:output version="4.0" method="html" indent="no" encoding="UTF-8" />
  <xsl:param name="SV_OutputFormat" select="'HTML'" />
  <xsl:variable name="XML" select="/" />
  <xsl:template match="/">

    <xsl:for-each select="$XML">

      <xsl:choose>
        <xsl:when test="count(  //mstns:Violations  )">
          <h4>
            StyleCop Report: <b style="color: red">Violated</b>
          </h4>
          <div style="margin-left:10px;">
            <details>
              <summary style="cursor:pointer">
                <span>
                  Total Source Files
                </span>
                :
                <span style="color:#0080ff;">
                  <xsl:value-of select="count(  //mstns:SourceCodeFiles  )" />
                </span>
                <br />
                <span>
                  Total Number of Violations
                </span>
                :
                <span style="color:#0080ff;">
                  <xsl:value-of select="count(  //mstns:Violations  )" />
                  <xsl:value-of select="xslHelper:IncreaseStyleCopViolation(count(  //mstns:Violations  ))"/>
                </span>
              </summary>

              <xsl:for-each select="mstns:StyleCopReport">
                <xsl:for-each select="mstns:Solutions">
                  <xsl:for-each select="mstns:Projects">

                    <div style="font-size: 11px">
                      <table style="cursor:pointer; width:100%;" border="0">
                        <tbody>
                          <tr>
                            <td style="text-align:left; width:80px;" height="20">
                              <span>
                                <xsl:text>Violations</xsl:text>
                              </span>
                            </td>
                            <td style="text-align:left; width:250px;" height="20">
                              <span>
                                <xsl:text>Name</xsl:text>
                              </span>
                            </td>
                            <td style="min-width:500px; text-align:left;" height="20">
                              <span>
                                <xsl:text>Path</xsl:text>
                              </span>
                            </td>
                          </tr>
                        </tbody>
                      </table>
                      <xsl:for-each select="mstns:SourceCodeFiles">

                        <xsl:choose>
                          <xsl:when test="count(  mstns:Violations  )">

                            <details>
                              <summary>
                                <table style="cursor:pointer; width:100%;" border="0">
                                  <tbody>
                                    <tr style="background-color:#d2d2d2">
                                      <td style="text-align:left; width:80px">
                                        <span style="color:#0080ff">
                                          <xsl:value-of select="count( mstns:Violations )" />
                                        </span>
                                      </td>
                                      <td style="text-align:left; width:250px">
                                        <xsl:for-each select="mstns:Name">
                                          <span style="cursor:pointertext-decoration:none">
                                            <xsl:apply-templates />
                                          </span>
                                        </xsl:for-each>
                                      </td>
                                      <td style="min-width:500px; text-align:left">
                                        <xsl:for-each select="mstns:Path">
                                          <xsl:apply-templates />
                                        </xsl:for-each>
                                      </td>
                                    </tr>
                                  </tbody>
                                </table>
                              </summary>

                              <div style="margin-left:10px;">
                                <table style="width:100%;" border="0">
                                  <tbody>
                                    <tr>
                                      <td style="width:80px">
                                        <span style="">
                                          <xsl:text>Rule</xsl:text>
                                        </span>
                                      </td>
                                      <td style="min-width:780px">
                                        <span>
                                          <xsl:text>Description</xsl:text>
                                        </span>
                                      </td>
                                      <td style="width:50px">
                                        <span>
                                          <xsl:text>Line</xsl:text>
                                        </span>
                                      </td>
                                    </tr>
                                  </tbody>
                                </table>
                                <xsl:for-each select="mstns:Violations">
                                  <xsl:for-each select="mstns:Rules">
                                    <details>
                                      <summary>
                                        <table style="border:0; cursor:pointer; width:100%;" border="0">
                                          <tbody>
                                            <tr style="background-color:silver">
                                              <td style="color:blue; width:80px">
                                                <xsl:for-each select="mstns:CheckId">
                                                  <xsl:apply-templates />
                                                </xsl:for-each>
                                              </td>
                                              <td style="min-width:780px">
                                                <span style="color:black">
                                                  <xsl:value-of select="../mstns:Message" />
                                                </span>
                                              </td>
                                              <td style="width:50px">
                                                <span>
                                                  <xsl:value-of select="../mstns:Line" />
                                                </span>
                                              </td>
                                            </tr>
                                          </tbody>
                                        </table>
                                      </summary>

                                      <div style="background-color:#ffffe9; margin-left:10px; padding:3px; width:80%;">
                                        <span>
                                          <xsl:value-of select="../mstns:SourceCodeLine" />
                                        </span>
                                      </div>
                                      <table style="background-color:#dee7ec; border:0; margin-left:12px; width:80%;" border="0">
                                        <tbody>
                                          <tr>
                                            <td style="width:80px">
                                              <span style="font-weight:bold">
                                                <xsl:text>Name</xsl:text>
                                              </span>
                                              <span>
                                                <xsl:text>:</xsl:text>
                                              </span>
                                            </td>
                                            <td style="min-width:620px">
                                              <xsl:for-each select="mstns:Name">
                                                <xsl:apply-templates />
                                              </xsl:for-each>
                                            </td>
                                          </tr>
                                          <tr>
                                            <td style="width:80px">
                                              <span style="font-weight:bold">
                                                <xsl:text>Namespace</xsl:text>
                                              </span>
                                              <span>
                                                <xsl:text>:</xsl:text>
                                              </span>
                                            </td>
                                            <td style="min-width:620px">
                                              <xsl:for-each select="mstns:Namespace">
                                                <xsl:apply-templates />
                                              </xsl:for-each>
                                            </td>
                                          </tr>
                                          <tr>
                                            <td style="width:80px">
                                              <span style="font-weight:bold">
                                                <xsl:text>Description</xsl:text>
                                              </span>
                                              <span>
                                                <xsl:text>:</xsl:text>
                                              </span>
                                            </td>
                                            <td style="min-width:620px">
                                              <xsl:for-each select="mstns:Description">
                                                <xsl:apply-templates />
                                              </xsl:for-each>
                                            </td>
                                          </tr>
                                        </tbody>
                                      </table>

                                    </details>
                                  </xsl:for-each>
                                </xsl:for-each>
                              </div>

                            </details>

                          </xsl:when>
                        </xsl:choose>

                      </xsl:for-each>
                    </div>

                  </xsl:for-each>
                </xsl:for-each>
              </xsl:for-each>

            </details>
          </div>
        </xsl:when>
        <xsl:otherwise>
          <h4>
            StyleCop Report: <b style="color: blue">Passed</b>
          </h4>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:for-each>


  </xsl:template>
</xsl:stylesheet>
