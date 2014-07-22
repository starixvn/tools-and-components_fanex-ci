<?xml version="1.0"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns="http://www.w3.org/TR/xhtml1/strict" xmlns:xslHelper="urn:XslTransformHelper">

<xsl:template match="/FxCopReport">
  <xsl:value-of select="xslHelper:IncreaseFxCopViolation(count(//Message[@Status='Active']))"/>
  <xsl:choose>
    <xsl:when test="count(//Message[@Status='Active'])">
      <h4>
        FxCop Analysis Report: <font color="red">Failed</font>
      </h4>
    </xsl:when>
    <xsl:otherwise>
      <h4>
        FxCop Analysis Report: <font color="blue">Passed</font>
      </h4>
    </xsl:otherwise>
  </xsl:choose>
  
  <xsl:apply-templates select="Namespaces"/>
    <xsl:choose>
        <xsl:when test="Namespaces">
        </xsl:when>
    </xsl:choose>
    <xsl:apply-templates select="Targets"/>
</xsl:template>
  
<xsl:template match="*">
<xsl:choose>
    <xsl:when test="@Name or name()='Resources'">
        <xsl:variable name="MessageCount" select="count(.//Message[@Status='Active'])"/>
        <xsl:choose>
            <xsl:when test="$MessageCount > 0">
                <xsl:variable name="nodeId" select="xslHelper:GenerateGuid()"/>
                <div class="NodeLine">
                    <xsl:attribute name="onClick">
                        javascript:ViewState('<xsl:value-of select="$nodeId"/>');
                    </xsl:attribute>
                    
                    <!-- Display Icon -->
                    <a href="javascript:DoNothing()">
                    <xsl:choose>
                        <xsl:when test="name()='Member' and @Kind='Method'">
                            <nobr class="NodeIcon">&#x004C;</nobr>    
                        </xsl:when>
                        <xsl:when test="name()='Member' and @Kind='Constructor'">
                            <nobr class="NodeIcon">&#x003D;</nobr>    
                        </xsl:when>
                        <xsl:when test="name()='Member' and @Kind='Property'">
                            <nobr class="NodeIcon">&#x0098;</nobr>    
                        </xsl:when>
                        <xsl:when test="name()='Member' and @Kind='Event'">
                            <nobr class="NodeIcon">&#x007E;</nobr>    
                        </xsl:when>
                        <xsl:when test="name()='Member' and @Kind='Field'">
                            <nobr class="NodeIcon">&#x00EB;</nobr>    
                        </xsl:when>
                        <xsl:when test="name()='Type' and @Kind='Class'">
                            <nobr class="NodeIcon">&#x003C;</nobr>    
                        </xsl:when>
                        <xsl:when test="name()='Type' and @Kind='Interface'">
                            <nobr class="NodeIcon">&#x003C;</nobr>    
                        </xsl:when>
                        <xsl:when test="name()='Type' and @Kind='Delegate'">
                            <nobr class="NodeIcon">&#x003C;</nobr>    
                        </xsl:when>
                        <xsl:when test="name()='Type' and @Kind='Enum'">
                            <nobr class="NodeIcon">&#x003C;</nobr>    
                        </xsl:when>
                        <xsl:when test="name()='Type' and @Kind='Struct'">
                            <nobr class="NodeIcon">&#x003C;</nobr>    
                        </xsl:when>
                        <xsl:when test="name()='Namespace'">
                            <nobr style="color: navy;">{} </nobr>    
                        </xsl:when>
                        <xsl:when test="name()='Target'">
                            <nobr class="NodeIcon">&#x0032;</nobr>    
                        </xsl:when>
                        <xsl:when test="name()='Module'">
                            <nobr class="NodeIcon">&#x0031;</nobr>    
                        </xsl:when>
                        <xsl:when test="name()='Resource'">
                            <nobr class="NodeIcon">&#x009D;</nobr>    
                        </xsl:when>
                        <xsl:when test="name()='Resources'">
                            <nobr class="NodeIcon">&#x00CC;</nobr>    
                        </xsl:when>
                        <xsl:otherwise>
                            [<xsl:value-of select="name()"/>]    
                        </xsl:otherwise>
                    </xsl:choose>
                    </a>
                    <xsl:choose>
                        <xsl:when test="name()='Resources'">
                            <xsl:value-of select="name()"/>
                        </xsl:when>
                        <xsl:otherwise>
                            <xsl:value-of select="@Name"/>
                        </xsl:otherwise>
                    </xsl:choose>
                    <nobr class="MessageNumber">
                        (<xsl:value-of select="$MessageCount"/>)
                    </nobr>
                </div>
                
                <div class="NodeDiv" style="display: none; position: relative; padding-left: 11;">
                    <xsl:attribute name="id">
                        <xsl:value-of select="$nodeId"/>
                    </xsl:attribute>

                    <xsl:apply-templates />
                </div>

            </xsl:when>
        </xsl:choose>
    </xsl:when>
    <xsl:otherwise>
        <xsl:apply-templates />
    </xsl:otherwise>
</xsl:choose>
</xsl:template>

<xsl:template match="Messages">
    <xsl:variable name="MessageBlockId" select="xslHelper:GenerateGuid()"/>
    <div class="MessageBlock">
        <xsl:attribute name="onClick">
            javascript:ViewState('<xsl:value-of select="$MessageBlockId"/>');
        </xsl:attribute>
        <a href="javascript:DoNothing()"><nobr class="MessagesIcon">&#x005D;</nobr></a>
        <xsl:variable name="MessageCount" select="count(Message[@Status='Active'])"/>
        <xsl:value-of select="$MessageCount"/>
            Message<xsl:choose><xsl:when test="$MessageCount > 1">s</xsl:when></xsl:choose>
            for 
            <xsl:value-of select="../@Name"/>
    </div>
    <div class="MessageBlockDiv" style="display: none; position: relative; padding-left: 5;">
        <xsl:attribute name="id">
            <xsl:value-of select="$MessageBlockId"/>
        </xsl:attribute>

        <table width="100%">
            <tr>
                <td class="ColumnHeader"> </td>
                <td class="ColumnHeader">Message Level</td>
                <td class="ColumnHeader">Certainty</td>
                <td class="ColumnHeader" width="100%">Resolution</td>
            </tr>
        <xsl:apply-templates select="Message[@Status='Active']"/>
        </table>
    </div>
        
</xsl:template>

<xsl:template match="Message">

    <!-- Message Row -->

    <xsl:variable name="messageId" select="xslHelper:GenerateGuid()"/>
    <xsl:variable name="rulename" select="@TypeName"/>

        <xsl:apply-templates select="Issue" >
                <xsl:with-param name="messageId"><xsl:value-of select="$messageId"/></xsl:with-param>
        </xsl:apply-templates>

    <tr>
        <td></td>
        <td colspan="3">
            <div class="MessageDiv" style="display: none">
                <xsl:attribute name="id">
                    <xsl:value-of select="$messageId"/>
                </xsl:attribute>

                <!--- Rule Details  -->
                <table width="100%" class="RuleBlock">
                                        <xsl:apply-templates select="Notes" mode="notes"/>
                    <xsl:apply-templates select="/FxCopReport/Rules/Rule[@TypeName=$rulename]" mode="ruledetails"/>
                </table>
            </div>
        </td>        
    </tr>
</xsl:template>

<xsl:template match="Issue">
<xsl:param name="messageId"></xsl:param>
    <tr>
        <xsl:attribute name="onClick">
            javascript:ViewState('<xsl:value-of select="$messageId"/>');
        </xsl:attribute>

        <xsl:attribute name="bgcolor">
            <xsl:choose>
                <xsl:when test="position() mod 2 = 1">#EEEEEE</xsl:when>
                <xsl:otherwise>white</xsl:otherwise>
            </xsl:choose>
        </xsl:attribute>

        <td valign="top">
            <a href="javascript:DoNothing();"><nobr class="NodeIcon"><xsl:attribute name="id"><xsl:value-of select="$messageId"/>Toggle</xsl:attribute>&#x0034;</nobr></a>
       </td>
       <td>
            <xsl:attribute name="class"><xsl:value-of select="@Level" /></xsl:attribute>
            <xsl:value-of select="@Level" />
        </td>
        <td valign="top" style=" text-align: center; ">
            <xsl:attribute name="class"><xsl:value-of select="@Level" /></xsl:attribute>
            <xsl:value-of select="@Certainty" />
        </td>
        <td class="Resolution" valign="top">
            <xsl:value-of select="text()" />
        </td>
    </tr>
    <xsl:if test="@Path">
    <tr class="SourceCode">
        <td></td>    
        <td class="PropertyName">Source:</td>
        <td class="PropertyContent" colspan="2">
          <a target="blank">
                <xsl:attribute name="href">
                    <xsl:value-of select="xslHelper:ResolveSourceFileLink(@Path, @File)"/>/*view*/
                </xsl:attribute>
                <xsl:value-of select="@Path"/>\<xsl:value-of select="@File"/>
            </a>
            (Line <xsl:value-of select="@Line"/>)
        </td>
    </tr>
    </xsl:if>    
</xsl:template>

<xsl:template match="Notes" mode="notes">
        <xsl:apply-templates select="User" mode="notes" />
</xsl:template>

<xsl:template match="User" mode="notes">
    <tr class="Note">
        <td class="Note">
        <nobr class="NoteUser"><xsl:value-of select="@Name"/></nobr>
        &#160;    
        <xsl:apply-templates select="Note" mode="notes">
        <xsl:with-param name="username"><xsl:value-of select="@Name"/></xsl:with-param>
        </xsl:apply-templates>
        </td>
    </tr>
</xsl:template>

<xsl:template match="Note" mode="notes">
    <xsl:param name="username"></xsl:param>
    <xsl:variable name="id" select="@Id"/>
    <xsl:apply-templates select="/FxCopReport/Notes/User[@Name=$username]/Note[@Id=$id]" mode="notesPointer"/>
</xsl:template>



<xsl:template match="Note" mode="notesPointer">
        <td></td>
        <td colspan="2" class="Note">
        <nobr class="NoteTime">[<xsl:value-of select="@Modified"/>]</nobr>:
        <xsl:value-of select="."/>
        </td>        
</xsl:template>

<xsl:template match="Description" mode="ruledetails">
    <tr>
        <td class="PropertyName">Rule Description:</td>
        <td class="PropertyContent"><xsl:value-of select="text()" /></td>
    </tr>    
</xsl:template>

<xsl:template match="File" mode="ruledetails">
    <tr>
        <td class="PropertyName">Rule File:</td>
        <td class="PropertyContent"><xsl:value-of select="@Name"/> [<xsl:value-of select="@Version"/>]</td>
    </tr>    
</xsl:template>

<xsl:template match="Url" mode="ruledetails">
    <tr>
        <td class="PropertyName">Help:</td>
        <td class="PropertyContent">
	    <a target="blank">
                <xsl:attribute name="href">
                    <xsl:value-of select="text()"/>
                </xsl:attribute>
                <xsl:value-of select="text()"/>
            </a>
 	</td>
    </tr>    
</xsl:template>

<xsl:template match="Rule" mode="ruledetails">
    <tr>
        <td class="PropertyName">Rule:</td>
        <td class="PropertyContent"><xsl:value-of select="Name" /></td>
    </tr>    
    <tr>
        <td class="PropertyName">Category:</td>
        <td class="PropertyContent"><xsl:value-of select="@Category" /></td>
    </tr>    
    <tr>
        <td class="PropertyName">Check Id:</td>
        <td class="PropertyContent"><xsl:value-of select="@CheckId" /></td>
    </tr>    
    <xsl:apply-templates select="Description" mode="ruledetails" />
    <xsl:apply-templates select="File" mode="ruledetails" />
    <xsl:apply-templates select="Url" mode="ruledetails" />
</xsl:template>

<!-- End Rule Details -->

</xsl:stylesheet>
