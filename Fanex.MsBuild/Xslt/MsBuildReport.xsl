<?xml version="1.0"?>

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xslHelper="urn:XslTransformHelper">

  <xsl:output method="html"/>

  <xsl:template match="/">
    <html>

      <head>
        <meta charset="utf-8" />
        <title>MsBuild Results</title>

        <style type="text/css">
          body
          {
          cursor: default;
          font: 0.8em 'Trebuchet MS' , 'Lucida Sans Unicode' , 'Lucida Grande' , 'Lucida' , Arial, Verdana, sans-serif;
          padding: 3;
          margin: 0;
          }
          #header {
          background: #33C4EB;
          height: 60px;
          text-align: left;
          width: 100%;
          }
          #page {
          margin: 1em;
          }
          table
          {
          border-collapse: collapse;
          border-spacing: 2px;
          display: table;
          margin-bottom: 2px;
          margin-top: 0;
          border-color: rgb(204, 204, 204);
          font-size: 1.0em;
          }
          thead {
          background: black;
          color: white;
          padding: 6px;
          }
          h1
          {
          font-family: "Century Gothic","Apple Gothic","Lucida Grande",Helvetica,Arial,Verdana,sans-serif;
          color: Black;
          }
          h2
          {
          font-family: "Century Gothic","Apple Gothic","Lucida Grande",Helvetica,Arial,Verdana,sans-serif;
          font-size: 1.8em;
          color: Black;
          padding: 0;
          margin-bottom: -10px;
          }

          h3
          {
          font-family: "Century Gothic","Apple Gothic","Lucida Grande",Helvetica,Arial,Verdana,sans-serif;
          font-size: 1.3em;
          color: Black;
          padding: 0;
          margin-bottom: 0px;
          }

          h4
          {
          font-family: "Century Gothic","Apple Gothic","Lucida Grande",Helvetica,Arial,Verdana,sans-serif;
          font-size: 1.0em;
          color: Black;
          padding: 0;
          margin-bottom: 0px;
          margin-top: 3px;
          }

          td
          {
          vertical-align:top;
          padding: 2px 6px;
          }

          thead td
          {
          padding: 4px 6px;
          }

          td.numeric
          {
          text-align: right;
          }
          #toc {
          list-style: none;
          margin: 0;
          padding: 0;
          width: 450px;
          }

          img
          {
          border:0 none;
          }

          summary::-webkit-details-marker {
          display: none;
          }
          summary:after {
          content: "";
          }
          details[open] summary:after {
          content: "";
          }

          #Title {color: black; font-weight: bold}
          .ColumnHeader {background-color:white; color: black}
          .CriticalError {color: darkred; font-weight: bold; vertical-align: middle; }
          .Error {color: royalblue; font-weight: bold; vertical-align: middle; }
          .CriticalWarning {color: darkorange; font-weight: bold; vertical-align: middle; }
          .Warning {color: darkgray; font-weight: bold; vertical-align: middle; }
          .Information {color: black; font-weight: bold; vertical-align: middle; }

          .PropertyName {color: black; font-weight: bold}
          .PropertyContent {color: black}
          .NodeIcon { font-family: WebDings; color: navy; padding-right: 5;}
          .MessagesIcon { font-family: WebDings; color: red;}
          .RuleDetails { padding-top: 10;}
          .SourceCode { background-color:#DDDDFF; }
          .RuleBlock { background-color:#EEEEFF; }
          .MessageNumber { color: darkred; }
          .MessageBlock { color: darkred; }
          .Resolution {color: black; }
          .NodeLine { }
          .Note { color:black; background-color: #DDDDFF; }
          .NoteUser { font-weight: bold; }
          .NoteTime { font-style: italic; }
          .Button { color: blue; background-color: #EEEEEE; border-style: outset;}
          a:link { color: blue; text-decoration: none; }
          a:visited { color: blue; text-decoration: none; }
          a:active { color: blue; text-decoration: none; }
        </style>

        <script>
          function ViewState(blockId)
          {
          var block = document.getElementById(blockId);
          if (block.style.display=='none')
          {
          block.style.display='block';
          if (block.className == 'MessageDiv')
          {
          var toggle = document.getElementById(blockId + "Toggle");
          toggle.innerHTML = "&#x0036;";
          }
          }
          else
          {
          block.style.display='none';
          if (block.className=='MessageDiv')
          {
          var toggle = document.getElementById(blockId + "Toggle");
          toggle.innerHTML = "&#x0034;";
          }
          }
          }

          function SwitchAll(how)
          {
          var nodes = document.getElementsByTagName("div");
          for (i = 0; i != nodes.length;i++)
          {
          var block = nodes[i];
          if (block != null)
          {
          if (block.className == 'NodeDiv' || block.className == 'MessageBlockDiv' || IsMessageDivWithActionNone(block, how))
          {
          block.style.display=how;
          }
          }
          }
          }

          function IsMessageDivWithActionNone(block, how)
          {
          if (block.className != 'MessageDiv') return false;
          if (how != 'none') return false;

          //as we're collapsing the tree, set the correct toggle icon
          var toggle = document.getElementById(block.id + "Toggle");
          if(toggle != null)
          {
          toggle.innerHTML = "&#x0034;";
          }
          return true;
          }

          function ExpandAll()
          {
          SwitchAll('block');
          }

          function CollapseAll()
          {
          SwitchAll('none');
          }

          function DoNothing() {}

          function ButtonState(blockId)
          {
          var block = document.getElementById(blockId);
          if (block.style.borderStyle=='inset')
          {
          block.style.borderStyle='outset';
          }
          else
          {
          block.style.borderStyle='inset';
          }
          }
        </script>
        
      </head>

      <body>
        <div id="header">
          <h2 style="color: #eee; padding: 4px">
            Bundle Build - <xsl:value-of  select="xslHelper:CurrentDateTime"/> - <xsl:value-of  select="xslHelper:BranchName"/>
          </h2>
        </div>

        <div id="page">

          <table>
            <tr>
              <td style="min-width: 80px;">
                <h2>
                  <xsl:value-of select="build/errors"/>
                </h2>
                <h3>Errors</h3>
              </td>
              <td style="min-width: 80px;">
                <h2>
                  <xsl:value-of select="build/warnings"/>
                </h2>
                <h3>Warnings</h3>
              </td>
            </tr>

          </table>

          <xsl:for-each select="build/project/project">
            <h3>
              <xsl:value-of select="xslHelper:GetProjectName(@file)"/>
              <xsl:value-of select="xslHelper:EmailConsoleProjectName(@file)"/>
            </h3>

            <!--Build report for each project-->
            <div style="margin-left: 10px">
              <h4>
                <xsl:value-of select="@file"/>
              </h4>
              <xsl:choose>
                <xsl:when test="error">
                  <h4>
                    Build Report: <b style="color: red">Error</b>
                  </h4>
                  <table style="width:100%" border="0" cellpadding="3" width="100%">
                    <tbody>

                      <tr>
                        <td>Error</td>
                        <td>File</td>
                        <td>Line</td>
                        <td>Description</td>
                      </tr>

                      <xsl:for-each select="error">
                        <xsl:value-of select="xslHelper:IncreaseError(1)"/>
                        <tr style="background-color:#e1e1e1; ">
                          <td style="text-align:left; width:80px; ">
                            <span style="color:#0080ff">
                              <xsl:value-of select="@code"/>
                            </span>
                          </td>
                          <td style="text-align:left; width:250px; ">
                            <span style="text-decoration:none; ">
                              <xsl:value-of select="@file"/>
                            </span>
                          </td>
                          <td style="text-align:left; width:50px; ">
                            <span style="color:#0080ff">
                              <xsl:value-of select="@line"/>
                            </span>
                          </td>
                          <td style="min-width:500px; text-align:left; ">
                            <xsl:value-of select="text()"/>
                          </td>
                        </tr>
                      </xsl:for-each>

                    </tbody>
                  </table>
                </xsl:when>
                <xsl:when test="warning">
                  <h4>
                    Build Report: <b style="color: #dd0000">Warning</b>
                  </h4>
                  <table style="width:100%" border="0" cellpadding="3" width="100%">
                    <tbody>

                      <tr>
                        <td>Warning</td>
                        <td>File</td>
                        <td>Line</td>
                        <td>Description</td>
                      </tr>

                      <xsl:for-each select="warning">
                        <xsl:value-of select="xslHelper:IncreaseWarning(1)"/>
                        <tr style="background-color:#e1e1e1; ">
                          <td style="text-align:left; width:80px; ">
                            <span style="color:#0080ff">
                              <xsl:value-of select="@code"/>
                            </span>
                          </td>
                          <td style="text-align:left; width:250px; ">
                            <span style="text-decoration:none; ">
                              <xsl:value-of select="@file"/>
                            </span>
                          </td>
                          <td style="text-align:left; width:50px; ">
                            <span style="color:#0080ff">
                              <xsl:value-of select="@line"/>
                            </span>
                          </td>
                          <td style="min-width:500px; text-align:left; ">
                            <xsl:value-of select="text()"/>
                          </td>
                        </tr>
                      </xsl:for-each>
                    </tbody>
                  </table>
                </xsl:when>
                <xsl:when test="project/error">
                  <h4>
                    Build Report: <b style="color: red">Error</b>
                  </h4>
                  <table style="width:100%" border="0" cellpadding="3" width="100%">
                    <tbody>

                      <tr>
                        <td>Error</td>
                        <td>File</td>
                        <td>Line</td>
                        <td>Description</td>
                      </tr>

                      <xsl:for-each select="project/error">
                        <xsl:value-of select="xslHelper:IncreaseError(1)"/>
                        <tr style="background-color:#e1e1e1; ">
                          <td style="text-align:left; width:80px; ">
                            <span style="color:#0080ff">
                              <xsl:value-of select="@code"/>
                            </span>
                          </td>
                          <td style="text-align:left; width:250px; ">
                            <span style="text-decoration:none; ">
                              <xsl:value-of select="@file"/>
                            </span>
                          </td>
                          <td style="text-align:left; width:50px; ">
                            <span style="color:#0080ff">
                              <xsl:value-of select="@line"/>
                            </span>
                          </td>
                          <td style="min-width:500px; text-align:left; ">
                            <xsl:value-of select="text()"/>
                          </td>
                        </tr>
                      </xsl:for-each>
                    </tbody>
                  </table>
                </xsl:when>
                <xsl:when test="project/warning">
                  <h4>
                    Build Report: <b style="color: #dd0000">Warning</b>
                  </h4>
                  <table style="width:100%" border="0" cellpadding="3" width="100%">
                    <tbody>

                      <tr>
                        <td>Warning</td>
                        <td>File</td>
                        <td>Line</td>
                        <td>Description</td>
                      </tr>

                      <xsl:for-each select="project/warning">
                        <xsl:value-of select="xslHelper:IncreaseWarning(1)"/>
                        <tr style="background-color:#e1e1e1; ">
                          <td style="text-align:left; width:80px; ">
                            <span style="color:#0080ff">
                              <xsl:value-of select="@code"/>
                            </span>
                          </td>
                          <td style="text-align:left; width:250px; ">
                            <span style="text-decoration:none; ">
                              <xsl:value-of select="@file"/>
                            </span>
                          </td>
                          <td style="text-align:left; width:50px; ">
                            <span style="color:#0080ff">
                              <xsl:value-of select="@line"/>
                            </span>
                          </td>
                          <td style="min-width:500px; text-align:left; ">
                            <xsl:value-of select="text()"/>
                          </td>
                        </tr>
                      </xsl:for-each>
                    </tbody>
                  </table>
                </xsl:when>
                <xsl:otherwise>
                  <h4>
                    Build Report: <b style="color: blue">Success</b>
                  </h4>
                </xsl:otherwise>
              </xsl:choose>
              
              <xsl:value-of disable-output-escaping="yes" select="xslHelper:EmailConsoleSummary()"/>
              
            </div>
            <hr/>
          </xsl:for-each>

        </div>

      </body>
    </html>

  </xsl:template>

</xsl:stylesheet>