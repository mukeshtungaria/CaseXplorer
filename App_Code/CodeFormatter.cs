using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Reflection;


public class DemoSettings {
    static object isSiteMode;
    public const string ReadOnlyErrorMessage = "<b>Data modifications are not allowed in the online demo.</b><br>" +
        "If you need to test data editing functionality, please install the ASPxGridView on your machine and run the demo locally.";

    public static bool IsSiteMode {
        get {
            if(isSiteMode != null)
                return (bool)isSiteMode;
            string val = ConfigurationManager.AppSettings["SiteMode"];
            isSiteMode = val == "true";
            return (bool)isSiteMode;
        }
    }

    public static void AssertNotReadOnly() {
        if(IsSiteMode)
            throw new InvalidOperationException(ReadOnlyErrorMessage);
    }
}

public enum CodeLanguage { cs, vb, delphi, cbuilder, c, vb6, js, vbs, html, xml, sql, aspx, unknown };

public class CodeRegionInfo {
    private string codeString;
    private CodeLanguage codeLanguage;

    public CodeRegionInfo() {
    }
    public CodeRegionInfo(string codeString, CodeLanguage codeLanguage) {
        this.codeString = codeString;
        this.codeLanguage = codeLanguage;
    }

    public string CodeString { get { return codeString; } set { codeString = value; } }
    public CodeLanguage CodeLanguage { get { return codeLanguage; } set { codeLanguage = value; } }
}
public class RegionPosition {
    public int EndPos;
    public int CorrectedEndPos;
}

public class CodeRender {
    // Region Name
    private const string BeginAspxCodeRegionString = "<%-- CODE_BEGIN --%>";
    private const string EndAspxCodeRegionString = "<%-- CODE_END --%>";

    private const string BeginJSCodeRegionString = "<%-- SKIPJSCODE_BEGIN --%>";
    private const string EndJSCodeRegionString = "<%-- SKIPJSCODE_END --%>";
    // RegExpression
    private const string AspxCodeRegionRegExString = @"<%-- ([CODE_BEGIN+\\s|CODE_END+\\s])* --%>";
    private const string JSInAspxCodeRegionRegExString = @"<%-- ([SKIPJSCODE_BEGIN+\\s|SKIPJSCODE_END+\\s])* --%>";

    private const string JSCodeRegionRegExString = "(<script[^>]*><!--[\\s.]*|[.\\s]*//--></script>)";
    private const string JSCodeRegionRegExStringWithoutComment = "(<script[^>]*>[\\s.]*|[.\\s]*</script>)";
    private const string JSCodeRegionRegExStringInBindingExpression = @"(<%\#.*<script[^>]*>[\\s.]*|[.\\s]*</script>.*%>)";

    // Misc
    private const string BeginJStCodeTag = "<script type=\"text/javascript\">";
    private const string EndJStCodeTag = "</script>";
    private const string PreControlString = "<pre class=\"{0}\">{1}</pre>";

    private Regex AspxCodeRegionRegEx = null;
    private Regex JSCodeRegionRegEx = null;

    public CodeRender() {
    }

    public Control GetDescriptionTextControl(string text) {
        WebControl div = CreateWebControl(HtmlTextWriterTag.Div);
        div.CssClass = "cr-div-description";
        div.Controls.Add(new LiteralControl(text));
        return div;
    }

    public Control GetHTMLFormattedUnknownFileControl(string filePath) {
        string extension = filePath.Substring(filePath.LastIndexOf('.')).ToLowerInvariant();
        switch(extension) {
            case ".aspx":
            case ".ascx":
                return GetHTMLFormattedAspxFileControl(filePath);
            case ".js":
                return GetHTMLFormattedJSFileControl(filePath);
            case ".cs":
                return GetHTMLFormattedCSFileControl(filePath);
            case ".vb":
                return GetHTMLFormattedVBFileControl(filePath);
            default:
                throw new NotSupportedException();
        }
    }

    public Control GetHTMLFormattedAspxFileControl(string fileName) {
        WebControl div = CreateWebControl(HtmlTextWriterTag.Div);
        div.CssClass = "cr-div";
        string fileText = GetFileAllText(fileName);

        CodeRegionInfo[] codeRegions = GetAspxDocumentCodeRegions(fileText);

        Control curControl = null;
        for(int i = 0; i < codeRegions.Length; i++) {
            if(codeRegions[i].CodeLanguage != CodeLanguage.js) {
                string regionHTMLCode = GetHTMLFormattedCodeRegion(codeRegions[i]);
                curControl = CreateAspxCodePreControl(regionHTMLCode);
                div.Controls.Add(curControl);
            }
        }
        return div;
    }
    public Control GetHTMLFormattedJSFileControl(string fileName) {
        string fileText = GetFileAllText(fileName);

        WebControl div = null;
        CodeRegionInfo[] codeRegions = GetJSCodeRegions(fileText);
        //if(codeRegions.Length == 0 && !string.IsNullOrEmpty(fileText))
        //    codeRegions = new CodeRegionInfo[] { new CodeRegionInfo(fileText, CodeLanguage.js) };
        if(codeRegions.Length > 0) {
            div = CreateWebControl(HtmlTextWriterTag.Div);
            div.CssClass = "cr-div";

            Control curControl = null;
            for(int i = 0; i < codeRegions.Length; i++) {
                string regionHTMLCode = GetHTMLFormattedCodeRegion(codeRegions[i]);
                curControl = CreateJSCodeControl(regionHTMLCode, false);
                div.Controls.Add(curControl);
            }
        }
        return div;
    }
    public Control GetHTMLFormattedCSFileControl(string fileName) {
        return GetHTMLFormattedCodeFileControl(fileName, CodeLanguage.cs);
    }
    public Control GetHTMLFormattedVBFileControl(string fileName) {
        return GetHTMLFormattedCodeFileControl(fileName, CodeLanguage.vb);
    }
    public Control GetHTMLFormattedXmlFileControl(string fileName) {
        return GetHTMLFormattedCodeFileControl(fileName, CodeLanguage.xml);
    }
    protected Control GetHTMLFormattedCodeFileControl(string fileName, CodeLanguage codeLanguage) {
        WebControl div = CreateWebControl(HtmlTextWriterTag.Div);
        div.CssClass = "cr-div";

        string fileText = GetFileAllText(fileName);
        CodeFormatter codeRender = new CodeFormatter(codeLanguage, false);

        div.Controls.Add(CreatePreControl(codeRender.GetFormattedCode(fileText), ""));
        return div;
    }

    protected Control CreateJSCodeControl(string text, bool needScriptTags) {
        Control mainControl = new Control();

        if(needScriptTags) {
            CodeRegionInfo jsTag = new CodeRegionInfo();
            jsTag.CodeLanguage = CodeLanguage.aspx;
            jsTag.CodeString = BeginJStCodeTag;
            WebControl beginJSTag = CreateAspxCodeSpanControl(GetHTMLFormattedCodeRegion(jsTag));
            mainControl.Controls.Add(beginJSTag);
        }

        mainControl.Controls.Add(CreatePreControl(text, "cr-js-pre"));

        if(needScriptTags) {
            CodeRegionInfo jsEndTag = new CodeRegionInfo();
            jsEndTag.CodeString = EndJStCodeTag;
            WebControl endJSTag = CreateAspxCodeSpanControl(GetHTMLFormattedCodeRegion(jsEndTag));
            mainControl.Controls.Add(endJSTag);
        }

        return mainControl;
    }

    protected CodeRegionInfo[] GetAspxDocumentCodeRegions(string documentText) {
        List<CodeRegionInfo> ret = new List<CodeRegionInfo>();

        AspxCodeRegionRegEx = new Regex(AspxCodeRegionRegExString);
        MatchCollection mathCollection = AspxCodeRegionRegEx.Matches(documentText);
        if((double)mathCollection.Count % 2 != 0)
            throw new Exception("");

        int count = mathCollection.Count;
        int regionCount = (int)mathCollection.Count / 2;

        if(regionCount == 0) {
            CodeRegionInfo[] regions = GetASPXCodeRegions(documentText, 0,
                documentText.Length);
            ret.AddRange(regions);
        } else {
            for(int i = 0; i < regionCount; i++) {
                CodeRegionInfo[] regions = GetASPXCodeRegions(documentText, mathCollection[2 * i].Index,
                    mathCollection[2 * i + 1].Index);
                ret.AddRange(regions);
            }
        }
        return ret.ToArray();
    }

    protected string GetHTMLFormattedCodeRegion(CodeRegionInfo codeRegion) {
        string ret = "";
        string regFormattedString = "";
        CodeFormatter codeRender = new CodeFormatter(codeRegion.CodeLanguage, false);
        regFormattedString = codeRender.GetFormattedCode(codeRegion.CodeString);
        ret += regFormattedString;
        return ret.Trim();
    }
    private CodeRegionInfo[] GetASPXCodeRegions(string fileText,
    int startIndex, int endIndex) {
        List<CodeRegionInfo> ret = new List<CodeRegionInfo>();

        // aspx code including js-code
        string regionContent = fileText.Substring(startIndex, endIndex - startIndex).Trim();
        regionContent = regionContent.Replace(BeginAspxCodeRegionString, "");
        regionContent = regionContent.Replace(EndAspxCodeRegionString, "");

        // code analyzer
        JSCodeRegionRegEx = new Regex(JSCodeRegionRegExString, RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        MatchCollection mathCollection = JSCodeRegionRegEx.Matches(regionContent);
        if(mathCollection.Count == 0) {
            JSCodeRegionRegEx = new Regex(JSCodeRegionRegExStringWithoutComment, RegexOptions.Multiline |
                RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            mathCollection = JSCodeRegionRegEx.Matches(regionContent);
        }
        Dictionary<int, RegionPosition> scriptStartEndPosInBindingExp = GetScriptPosInBindingExpressionDic(regionContent);
        Dictionary<int, int> scriptInJSBlocks = GetJSBlockPosition(regionContent);

        if((double)mathCollection.Count % 2 != 0)
            throw new Exception("");

        int regionCount = (int)mathCollection.Count / 2;

        int index = 0;
        for (int i = 0; i < regionCount; i++) {
            int startJSIndex = mathCollection[2 * i].Index;
            int endJSIndex = mathCollection[2 * i + 1].Index;

            bool isScriptInBindingExpression =
                            scriptStartEndPosInBindingExp.ContainsKey(startJSIndex) &&
                            (scriptStartEndPosInBindingExp[startJSIndex].EndPos == endJSIndex);
            bool isScriptInJSCodeBlock = IsScriptInJSCodeBlock(scriptInJSBlocks, 
                startJSIndex, endJSIndex);
            // aspx code
            int correctStartJSIndex = startJSIndex;
            if (isScriptInBindingExpression)
                correctStartJSIndex = scriptStartEndPosInBindingExp[startJSIndex].CorrectedEndPos;
            else if (isScriptInJSCodeBlock)
                correctStartJSIndex = scriptInJSBlocks.ContainsKey(startJSIndex) ? scriptInJSBlocks[startJSIndex] : endJSIndex;
            
            CodeRegionInfo aspxRegionInfo = GetCodeRegionInfo(regionContent, index, correctStartJSIndex, 
                CodeLanguage.aspx);

            if (aspxRegionInfo != null)
                ret.Add(aspxRegionInfo);

            index = endJSIndex + mathCollection[2 * i + 1].Value.Length;
            if (isScriptInBindingExpression)
                index = scriptStartEndPosInBindingExp[startJSIndex].CorrectedEndPos;
        }

        // last code part
        if(index <= regionContent.Length - 1) {
            CodeRegionInfo aspxRegionInfo = GetCodeRegionInfo(regionContent, index, regionContent.Length, CodeLanguage.aspx);
            if(aspxRegionInfo != null)
                ret.Add(aspxRegionInfo);
        }
        return ret.ToArray();
    }
    private Dictionary<int, int> GetJSBlockPosition(string regionContent) {
        Dictionary<int, int> ret = new Dictionary<int, int>();
        Regex regEx = new Regex(JSInAspxCodeRegionRegExString);

        MatchCollection mathCollection = regEx.Matches(regionContent);
        if ((double)mathCollection.Count % 2 != 0)
            throw new Exception("");

        int count = mathCollection.Count;
        int regionCount = (int)mathCollection.Count / 2;

        for (int i = 0; i < regionCount; i++) {
            string jsCode = regionContent.Substring(mathCollection[2 * i].Index, 
                mathCollection[2 * i + 1].Index - mathCollection[2 * i].Index);

            ret.Add(jsCode.IndexOf("<script") + mathCollection[2 * i].Index,
                mathCollection[2 * i + 1].Index);
        }
        return ret;
    }
    private Dictionary<int, RegionPosition> GetScriptPosInBindingExpressionDic(string regionContent) {
        Dictionary<int, RegionPosition> ret = new Dictionary<int, RegionPosition>();
        Regex regEx = new Regex(JSCodeRegionRegExStringInBindingExpression, RegexOptions.Multiline |
            RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        MatchCollection mathCollection = regEx.Matches(regionContent);
        if ((double)mathCollection.Count % 2 != 0)
            throw new Exception("");
        int regionCount = (int)mathCollection.Count / 2;
        for (int i = 0; i < regionCount; i++) {
            int startJSIndex = mathCollection[2 * i].Index + 
                mathCollection[2 * i].Value.IndexOf("<script");
            int endJSIndex = mathCollection[2 * i + 1].Index;

            RegionPosition endPos = new RegionPosition();
            endPos.EndPos = endJSIndex;
            endPos.CorrectedEndPos = endJSIndex + mathCollection[2 * i + 1].Value.Length;

            ret.Add(startJSIndex, endPos);
        }
        return ret;
    }
    private CodeRegionInfo[] GetJSCodeRegions(string fileText) {
        List<CodeRegionInfo> ret = new List<CodeRegionInfo>();

        // code analyzer
        JSCodeRegionRegEx = new Regex(JSCodeRegionRegExString, RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        MatchCollection mathCollection = JSCodeRegionRegEx.Matches(fileText);
        if(mathCollection.Count == 0) {
            JSCodeRegionRegEx = new Regex(JSCodeRegionRegExStringWithoutComment, RegexOptions.Multiline |
                RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            mathCollection = JSCodeRegionRegEx.Matches(fileText);
        }

        if((double)mathCollection.Count % 2 != 0)
            throw new Exception("");

        Dictionary<int, RegionPosition> scriptStartEndPosInBindingExp = GetScriptPosInBindingExpressionDic(fileText);
        Dictionary<int, int> scriptInJSBlocks = GetJSBlockPosition(fileText);

        int regionCount = (int)mathCollection.Count / 2;

        if(regionCount != 0) {
            for(int i = 0; i < regionCount; i++) {
                int startJSIndex = mathCollection[2 * i].Index;
                int endJSIndex = mathCollection[2 * i + 1].Index;

                bool isScriptInBindingExpression =
                                scriptStartEndPosInBindingExp.ContainsKey(startJSIndex) &&
                                (scriptStartEndPosInBindingExp[startJSIndex].EndPos == endJSIndex);
                bool isScriptInJSCodeBlock = IsScriptInJSCodeBlock(scriptInJSBlocks, startJSIndex, endJSIndex);
                // js code
                if (!isScriptInBindingExpression && !isScriptInJSCodeBlock) {
                    CodeRegionInfo jsRegionInfo = GetCodeRegionInfo(fileText, startJSIndex + mathCollection[2 * i].Value.Length,
                        endJSIndex, CodeLanguage.js);
                    if (jsRegionInfo != null)
                        ret.Add(jsRegionInfo);
                }
            }
        }
        return ret.ToArray();
    }
    private CodeRegionInfo GetCodeRegionInfo(string text, int startIndex, int endIndex, CodeLanguage language) {
        CodeRegionInfo ret = new CodeRegionInfo();
        ret.CodeLanguage = language;
        if (startIndex != endIndex - startIndex) {
            ret.CodeString = text.Substring(startIndex, endIndex - startIndex).Trim();
            ret.CodeString = ret.CodeString.Replace(BeginJSCodeRegionString, "").Replace(EndJSCodeRegionString, "");
        }
        return ret.CodeString != "" ? ret : null;
    }
    private bool IsScriptInJSCodeBlock(Dictionary<int, int> scriptInJSBlocks, 
        int startJSIndex, int endJSIndex) {
        foreach (int key in scriptInJSBlocks.Keys) {
            if ((key >= startJSIndex) && (key <= endJSIndex))
                return true;
        }
        return false;
    }

    // Render Utils
    private Control CreateAspxCodePreControl(string text) {
        return CreatePreControl(text, "cr-aspx-text cr-aspx-pre");
    }
    private WebControl CreateAspxCodeSpanControl(string text) {
        WebControl preControl = CreateWebControl(HtmlTextWriterTag.Span);
        preControl.Controls.Add(CreateLiteralControl(text));
        preControl.CssClass = "cr-aspx-text";
        return preControl;
    }
    private Control CreatePreControl(string text, string cssClass) {
        return new LiteralControl(string.Format(PreControlString, cssClass, text));
    }
    private Control CreateLiteralControl(string text) {
        LiteralControl litControl = new LiteralControl(text);
        litControl.EnableViewState = false;
        return litControl;
    }
    private WebControl CreateWebControl(HtmlTextWriterTag tagName) {
        WebControl control = new WebControl(tagName);
        control.EnableViewState = false;
        return control;
    }

    // Misc
    private string HtmlEncode(string content) {
        return HttpUtility.HtmlEncode(content);
    }
    private string GetFileAllText(string fileName) {
        string contentString = string.Empty;
        using(StreamReader reader = new StreamReader(fileName, Encoding.UTF8)) {
            contentString = reader.ReadToEnd();
            reader.Close();
        }
        return contentString;
    }
}

public class CodeFormatter {

    public interface IFormatter {
        string Render(string code);
        string LanguageName { get; set;}
    }
    class FormatterBase : IFormatter {
        protected static int regNumber = 0;
        public FormatterBase(string langName) {
            LanguageName = langName;
        }

        private string _LanguageName;
        public string LanguageName {
            get {
                return _LanguageName;
            }
            set {
                _LanguageName = value;
            }
        }
        protected const string regionBlockReg = @"(?<WhiteSpace>(?:{0})*\s*)(?<BeginRegion>#region){{1}}\s*(?<Caption>.*)(?<Block>[\s\S]*?)(?<EndRegion>#end\s*region.*[\r\n]?){{1}}";

        protected const string textBlock =
                    "<h1 class=\"{5}\">" +
                    "<span onclick=\"ExpandCollapse(section{4}{0}Toggle)\" style=\"cursor:default;\" onkeypress=\"ExpandCollapse_CheckKey(section{4}{0}Toggle)\" tabindex=\"0\">" +
                    "<img id=\"section{4}{0}Toggle\" alt=\"Expand\" src=\"{7}Images/collapsedbutton.gif\"/> {1}" +
                    "</span></h1>" +
                    "<div id=\"section{4}{0}Section\" class=\"{6}\" style=\"display: none;\"><div>{2}</div></div>";
        protected internal string rxComments = "";
        protected internal string rxStrings = "";
        protected internal string rxKeywords = "";
        protected internal string rxPreprocs = "";
        protected internal string rxTags = "";
        protected internal string rxAttributes = "";

        protected internal string cssKeyword = "";
        protected internal string cssComment = "";
        protected internal string cssString = "";
        protected internal string cssPreproc = "";
        protected internal string cssTag = "";
        protected internal string cssAttribute = "", cssRegionHead = "cr-region-head", cssRegionDiv = "cr-region-div", cssRegionSpan = "cr-region-span";
        protected internal bool caseSensitive = true;

        protected string GetRegionSpan() { return string.Format("<span class=\"{0}\">&nbsp;</span>", this.cssRegionSpan); }
        protected virtual string ReplaceEntities(Match match) {
            return Regex.Replace(match.ToString(), "&([^agl][^mt][^p;][^;])", "&amp;$1", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
        }
        protected string ReplaceRegion(Match match) {
            string res = match.ToString();
            if(match.Groups["BeginRegion"] == null || match.Groups["EndRegion"] == null)
                return res;
            string codeBlock = match.Groups["Block"].ToString();
            string spacing = match.Groups["WhiteSpace"] == null ? string.Empty : match.Groups["WhiteSpace"].ToString();
            if(spacing.EndsWith(" "))
                spacing += "   ";//button spacing
            codeBlock = codeBlock.Replace(GetRegionSpan(), "");
            codeBlock = codeBlock.Trim('\xd', '\xa').TrimEnd(' ', '\xd', '\xa', '\t');
            string firstUrlSegment = HttpContext.Current.Request.Url.Segments[1].ToLowerInvariant();
            bool isTutorials = (firstUrlSegment.IndexOf("tutorials") == 0) || (firstUrlSegment.IndexOf("helpsamples") == 0);
            res = string.Format(textBlock, regNumber, match.Groups["Caption"],
                codeBlock, spacing, LanguageName.Replace("#", "S").Replace(".", ""), this.cssRegionHead, this.cssRegionDiv,
                isTutorials ? "" : "../");

            regNumber++;
            return res;

        }

        private string MatchEval(Match match) {
            if(match.Groups[1].Success && cssComment != "") { // comment
                StringReader reader = new StringReader(ReplaceEntities(match));
                StringBuilder sb = new StringBuilder();
                string line = reader.ReadLine();
                while(!string.IsNullOrEmpty(line)) {
                    if(sb.Length > 0) {
                        sb.Append("\r\n");
                    }
                    sb.Append("<span class=\"" + cssComment + "\">");
                    sb.Append(line);
                    sb.Append("</span>");
                    line = reader.ReadLine();
                }
                return sb.ToString();
            }
            if(match.Groups[5].Success && cssTag != "") //tags
				{
                return "<span class=\"" + cssTag + "\">" + ReplaceEntities(match) + "</span>";
            }
            if(match.Groups[6].Success && cssAttribute != "") //tags
				{
                return "<span class=\"" + cssAttribute + "\">" + ReplaceEntities(match) + "</span>";
            }
            if(match.Groups[2].Success && cssString != "") //string literal
				{
                return "<span class=\"" + cssString + "\">" + ReplaceEntities(match) + "</span>";
            }
            if(match.Groups[3].Success && cssPreproc != "") //preprocessor keyword
			{
                return "<span class=\"" + cssPreproc + "\">" + ReplaceEntities(match) + "</span>";
            }
            if(match.Groups[4].Success && cssKeyword != "") //keyword
			{
                return "<span class=\"" + cssKeyword + "\">" + ReplaceEntities(match) + "</span>";
            }
            return ReplaceEntities(match); //none of the above
        }


        private Regex GetCompiledRegex() {
            //generate the keyword and preprocessor regexes from the keyword lists
            Regex r;
            r = new Regex(@"\w+|#\w+|#(\\s\*\w+)+");
            string regKeyword = r.Replace(rxKeywords, @"(?<=^|\W)$0(?=\W)");
            string regPreproc = r.Replace(rxPreprocs, @"(?<=^|\s)$0(?=\s|$)");
            string regHPKeyword = r.Replace(rxKeywords, @"(?<=^|\W)$0(?=\W)");
            r = new Regex(@" +");
            regKeyword = r.Replace(regKeyword, @"|");
            regPreproc = r.Replace(regPreproc, @"|");
            regHPKeyword = r.Replace(regHPKeyword, @"|");

            //build a master regex with capturing groups
            StringBuilder regAll = new StringBuilder();
            regAll.Append("(");
            regAll.Append(rxComments);
            //if (rxStrings != String.Empty) {
            regAll.Append(")|(");
            regAll.Append(rxStrings);
            //}
            //if (regPreproc != String.Empty) {
            regAll.Append(")|(");
            regAll.Append(regPreproc);
            //}
            if(regKeyword != String.Empty) {
                regAll.Append(")|(");
                regAll.Append(regKeyword);
            }
            if(rxTags != String.Empty) {
                regAll.Append(")|(");
                regAll.Append(rxTags);
            }
            if(rxAttributes != String.Empty) {
                regAll.Append(")|(");
                regAll.Append(rxAttributes);
            }
            regAll.Append(")");

            RegexOptions caseInsensitive = caseSensitive ? 0 : RegexOptions.IgnoreCase;
            return new Regex(regAll.ToString(), RegexOptions.Singleline | caseInsensitive);
        }

        public virtual string Render(string code) {
            string res = GetCompiledRegex().Replace(code, new MatchEvaluator(this.MatchEval));
            res = GetLastReplace(res);
            if(AllowRegions)
                res = ReplaceRegions(res);
			res = res.Replace("\t", "    ");
            return res;
        }
        protected virtual string ReplaceRegions(string res) {
            res = new Regex("^", RegexOptions.Multiline).Replace(res, GetRegionSpan());
            Regex reg = new Regex(string.Format(regionBlockReg, GetRegionSpan().Replace("<", "\\<").Replace(">", "\\>")), RegexOptions.IgnoreCase);
            return reg.Replace(res, new MatchEvaluator(ReplaceRegion));
        }
        protected virtual bool AllowRegions { get { return false; } }
        public virtual string GetLastReplace(string code) {
            return code;
        }
    }

    class UnknownFormatter : FormatterBase {
        public UnknownFormatter(string langName)
            : base(langName) {

        }
    }

    class CSFormatter : FormatterBase {
        public CSFormatter(string langName)
            : base(langName) {
            rxKeywords = "abstract as base bool break byte case catch char "
            + "checked class const continue decimal default delegate do double else "
            + "enum event explicit extern false finally fixed float for foreach goto "
            + "if implicit in int interface internal is lock long namespace new null "
            + "object operator out override params private protected public readonly "
            + "ref return sbyte sealed short sizeof stackalloc static string struct "
            + "switch this throw true try typeof uint ulong unchecked unsafe ushort "
            + "using virtual void while partial";
            rxComments = @"/\*.*?\*/|//.*?(?=\r|\n)";
            rxStrings = @"@?""""|@?"".*?(?!\\).""|''|'.*?(?!\\).'";
            rxPreprocs = "#if #else #elif #endif #define #undef #warning #error #line";// #region #endregion";

            cssComment = "cr-cs-comment";
            cssKeyword = "cr-cs-keyword";
            cssPreproc = "cr-cs-preproc";
            cssString = "cr-cs-string";
        }
        protected override bool AllowRegions { get { return true; } }
        public override string Render(string code) {
            code = code.Replace("<", "&lt;").Replace(">", "&gt;");
            return base.Render(code);
        }
    }

    class XMLFormatter : FormatterBase {
        public XMLFormatter(string langName)
            : base(langName) {
            rxKeywords = "=\" \"";
            //				rxComments = @"&lt;!--.*?--&gt;";
            rxComments = @"<!--.*?-->";
            //hook
            rxStrings = @"NevypolnimoeUslovie";
            rxTags = @"/>|<.+?[\s|>]";
            //                rxTags = @"/&gt;|&lt;.+?[\s|&gt;]";
            rxAttributes = @"[\w]+?=";
            rxPreprocs = "#if #else #elif #endif #define #undef #warning #error #line #region #endregion";

            cssComment = "cr-xml-comment";
            cssKeyword = "cr-xml-keyword";
            cssPreproc = "cr-xml-preproc";
            cssString = "cr-xml-string";
            cssTag = "cr-xml-tag";
            cssAttribute = "cr-xml-attribute";
        }

        protected override string ReplaceEntities(Match match) {
            return match.ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
        }

        public override string Render(string code) {
            return base.Render(code);
        }
    }
    class ASPXFormatter : FormatterBase {
        public ASPXFormatter(string langName)
            : base(langName) {
            rxKeywords = "asdasdasdasd";
            //				rxComments = @"&lt;!--.*?--&gt;";
            rxComments = @"<%--.*?--%>";
            //hook
            //rxStrings = @"# .+?[\) .^%]{1,2}";
            rxStrings = @"# .+?[\) .]";

            rxTags = @"/>|<.+?[\s|>]";
            //                rxTags = @"/&gt;|&lt;.+?[\s|&gt;]";
            rxAttributes = @"[-\w]+?=";
            rxPreprocs = "<% %>";

            cssComment = "cr-aspx-comment";
            cssKeyword = "cr-aspx-keyword";
            cssPreproc = "cr-aspx-preproc";
            cssString = "cr-aspx-string";
            cssTag = "cr-aspx-tag";
            cssAttribute = "cr-aspx-attribute";
            cssRegionHead = "cr-aspx-region-head";
        }

        protected override string ReplaceEntities(Match match) {
            return match.ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
        }

        public override string Render(string code) {
            Regex reg = new Regex(@"\r?(?<WhiteSpace>\s*)(?<BeginRegion>\<\%--\s*BeginRegion){1}\s*(?<Caption>.*?)(?:--\%\>){1}(?<Block>[\s\S]*?)(?<EndRegion>\s*\<\%--\s*EndRegion\s*--\%\>){1}");
            //"\r?(?<WhiteSpace>\s*)(?<BeginRegion>\<\%--\s*BeginRegion)\s*(?<Caption>[\s\S]+)--\%\>(?<Block>[\s\S]+)(?<EndRegion>\s*\<\%--\s*EndRegion\s*--\%\>){1}");
            //(?:BeginRegion){1}(?<Block>[\s\S]*?)(?:EndRegion){1}
            code = reg.Replace(code, new MatchEvaluator(ReplaceRegionPre));
            return base.Render(code);
        }

        public override string GetLastReplace(string code) {
            Regex rx = new Regex("([^c^l^a^s^s])=");
            string newCode = rx.Replace(code, "$1<span class=\"cr-aspx-text\">=</span>");
            rx = new Regex("(&lt;/|&lt;)");
            newCode = rx.Replace(newCode, "<span class=\"cr-aspx-text\">$1</span>");
            rx = new Regex("(/&gt;|&gt;)");
            return rx.Replace(newCode, "<span class=\"cr-aspx-text\">$1</span>");
        }
        protected override bool AllowRegions { get { return true; } }
        string ReplaceRegionPre(Match match) {
            string res = match.ToString();
            if(match.Groups["BeginRegion"] == null || match.Groups["EndRegion"] == null)
                return res;
            string spacing = match.Groups["WhiteSpace"] == null ? string.Empty : match.Groups["WhiteSpace"].ToString();
            string caption = match.Groups["Caption"] == null ? "" : match.Groups["Caption"].ToString();
            return string.Format("{0}#region {1}\xd\xa{2}\xd\xa#endregion", spacing, caption, match.Groups["Block"]);
        }
    }

    class VBNETFormatter : FormatterBase {
        public VBNETFormatter(string langName)
            : base(langName) {
            rxKeywords = "AddHandler AddressOf AndAlso Alias And Ansi As Assembly "
                + "Auto Boolean ByRef Byte ByVal Call Case Catch "
                + "CBool CByte CChar CDate CDec CDbl Char CInt "
                + "Class CLng CObj Const CShort CSng CStr CType "
                + "Date Decimal Declare Default Delegate Dim DirectCast Do "
                + "Double Each Else ElseIf End Enum Erase Error "
                + "Event Exit False Finally For Friend Function Get "
                + "GetType GoTo  Handles If Implements Imports In Inherits "
                + "Integer Interface Is Let Lib Like Long Loop "
                + "Me Mod Module MustInherit MustOverride MyBase MyClass Namespace "
                + "New Next Not Nothing NotInheritable NotOverridable Object On "
                + "Option Optional Or OrElse Overloads Overridable Overrides ParamArray "
                + "Preserve Private Property Protected Public RaiseEvent ReadOnly ReDim "
                + "REM RemoveHandler Resume Return Select Set Shadows Shared "
                + "Short Single Static Step Stop String Structure Sub "
                + "SyncLock Then Throw To True Try TypeOf Unicode "
                + "Until Variant When While With WithEvents WriteOnly Xor Partial";
            rxComments = @"(?:'|REM\s).*?(?=\r|\n)";
            rxPreprocs = @"#\s*Const #\s*If #\s*Else #\s*ElseIf #\s*End\s*If "
                + @"#\s*ExternalSource #\s*End\s*ExternalSource";// "
            // +@"#\s*Region #\s*End\s*Region";
            rxStrings = @"""""|"".*?""";
            cssComment = "cr-vb-comment";
            cssKeyword = "cr-vb-keyword";
            cssString = "cr-vb-string";
            cssPreproc = "cr-vb-preproc";
            caseSensitive = false;
        }
        public override string Render(string code) {
            code = new Regex(@"#End\s{1}Region").Replace(code, "#EndRegion");
            code = code.Replace("<", "&lt;").Replace(">", "&gt;");
            return base.Render(code);
        }
        protected override bool AllowRegions { get { return true; } }

    }

    class DelphiFormatter : FormatterBase {
        public DelphiFormatter(string langName)
            : base(langName) {
            rxKeywords = "end; and array as begin case class const constructor destructor div do downto else end except"
                + " file finally for function goto if implementation in inherited interface is mod not object of on or packed procedure program property raise"
                + " record repeat set shl shr then threadvar to try type unit until uses var while with xor"
                + " true false ansichar ansistring boolean byte cardinal char comp currency double extended int64"
                + " integer longint longword pchar pointer real shortint single string tlist"
                + " variant word nil read write override private public protected virtual";
            rxComments = @"\{.*?\}|//.*?(?=\r|\n)";
            rxStrings = @"'.*?'";
            rxPreprocs = "#if #else #elif #endif #define #undef #warning #error #line #region #endregion";

            cssKeyword = "cr-delphi-keyword";
            cssComment = "cr-delphi-comment";
            cssString = "cr-delphi-string";
            cssPreproc = "cr-delphi-preproc";
            caseSensitive = false;
        }
    }

    class JSFormatter : FormatterBase {
        public JSFormatter(string langName)
            : base(langName) {
            rxKeywords = "var break case catch array "
            + "function continue default do else "
            + "false finally for foreach goto "
            + "if new null "
            + "object "
            + "return "
            + "switch this throw true try typeof "
            + "while";
            rxComments = @"/\*.*?\*/|//.*?(?=\r|\n)";
            rxStrings = @"@?""""|@?"".*?(?!\\).""|''|'.*?(?!\\).'";
            rxPreprocs = "#if #else #elif #endif #define #undef #warning #error #line #region #endregion";

            cssComment = "cr-js-comment";
            cssKeyword = "cr-js-keyword";
            cssPreproc = "cr-js-preproc";
            cssString = "cr-js-string";
        }
        protected override string ReplaceEntities(Match match) {
            return match.ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
        }
    }

    Dictionary<CodeLanguage, IFormatter> languages;

    public CodeFormatter(string language, bool displayCodeType) {
        CodeLanguage = parseLanguageType(language);
        DisplayCodeType = displayCodeType;
        fillLanguageTitles();
    }

    public CodeFormatter(CodeLanguage language, bool displayCodeType) {
        CodeLanguage = language;
        DisplayCodeType = displayCodeType;
        fillLanguageTitles();
    }

    private CodeLanguage parseLanguageType(string lang) {
        foreach(CodeLanguage cl in Enum.GetValues(typeof(CodeLanguage))) {
            if(cl.ToString() == lang.ToLower())
                return cl;
        }
        return CodeLanguage.unknown;
    }

    private void fillLanguageTitles() {
        languages = new Dictionary<CodeLanguage, IFormatter>();
        languages.Add(CodeLanguage.cs, new CSFormatter("C#"));
        languages.Add(CodeLanguage.vb, new VBNETFormatter("VB.NET"));
        languages.Add(CodeLanguage.delphi, new DelphiFormatter("Delphi"));
        languages.Add(CodeLanguage.cbuilder, new UnknownFormatter("C++ Builder"));
        languages.Add(CodeLanguage.c, new UnknownFormatter("C++"));
        languages.Add(CodeLanguage.vb6, new UnknownFormatter("VB"));
        languages.Add(CodeLanguage.js, new JSFormatter("JScript"));
        languages.Add(CodeLanguage.vbs, new UnknownFormatter("VBScript"));
        languages.Add(CodeLanguage.html, new UnknownFormatter("HTML"));
        languages.Add(CodeLanguage.sql, new UnknownFormatter("SQL"));
        languages.Add(CodeLanguage.xml, new XMLFormatter("XML"));
        languages.Add(CodeLanguage.aspx, new ASPXFormatter("ASPx"));
    }

    private CodeLanguage _CodeLanguage;
    public CodeLanguage CodeLanguage {
        get {
            return _CodeLanguage;
        }
        set {
            _CodeLanguage = value;
        }
    }
    private bool _DisplayCodeType;
    public bool DisplayCodeType {
        get {
            return _DisplayCodeType;
        }
        set {
            _DisplayCodeType = value;
        }
    }

    public string GetFormattedCode(string code) {
        string resCode = "";
        if(CodeLanguage == CodeLanguage.unknown)
            return code;
        // appending language name if required
        if(DisplayCodeType && languages.ContainsKey(CodeLanguage)) {
            resCode = "[" + languages[CodeLanguage].LanguageName + "]\r\n\r\n";
        }
        // rendering code
        if(languages.ContainsKey(CodeLanguage)) {
            resCode += languages[CodeLanguage].Render(code + " ");
        }
        return resCode.TrimEnd(' ');
    }
}
