﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RedBrick2.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\AMSTORE-SVR-22")]
        public string NetPath {
            get {
                return ((string)(this["NetPath"]));
            }
            set {
                this["NetPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\cad\\\\Solid Works\\\\Amstore_Macros\\\\ICONS\\\\setup.bmp")]
        public string Icon {
            get {
                return ((string)(this["Icon"]));
            }
            set {
                this["Icon"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=AMSTORE-SQL-05;Initial Catalog=ENGINEERING;Integrated Security=True")]
        public string ENGINEERINGConnectionString {
            get {
                return ((string)(this["ENGINEERINGConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\cad\\\\Solid Works\\\\Amstore_Macros\\\\ICONS\\\\Refresh-icon.bmp")]
        public string RefreshIcon {
            get {
                return ((string)(this["RefreshIcon"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\cad\\\\Solid Works\\\\Amstore_Macros\\\\ICONS\\\\Archive PDF.bmp")]
        public string ArchiveIcon {
            get {
                return ((string)(this["ArchiveIcon"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\cad\\\\Solid Works\\\\Amstore_Macros\\\\ICONS\\\\help_and_support.bmp")]
        public string HelpIcon {
            get {
                return ((string)(this["HelpIcon"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool MakeSounds {
            get {
                return ((bool)(this["MakeSounds"]));
            }
            set {
                this["MakeSounds"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2017-02-17")]
        public global::System.DateTime OdometerStart {
            get {
                return ((global::System.DateTime)(this["OdometerStart"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Engineering")]
        public string EngineeringDir {
            get {
                return ((string)(this["EngineeringDir"]));
            }
            set {
                this["EngineeringDir"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\AMSTORE-SVR-02\\shared\\shared\\general\\RedBrick\\InstallRedBrick.exe")]
        public string InstallerNetworkPath {
            get {
                return ((string)(this["InstallerNetworkPath"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://kcjuntunen.github.io/readbrick_readme.html")]
        public string UsageLink {
            get {
                return ((string)(this["UsageLink"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Windows\\Media\\tada.wav")]
        public string ClipboardSound {
            get {
                return ((string)(this["ClipboardSound"]));
            }
            set {
                this["ClipboardSound"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(" *")]
        public string NotSavedMark {
            get {
                return ((string)(this["NotSavedMark"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\AMSTORE-SVR-22\\cad\\Solid Works\\Amstore_Macros\\gauges.xml")]
        public string GaugePath {
            get {
                return ((string)(this["GaugePath"]));
            }
            set {
                this["GaugePath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\AMSTORE-SVR-22\\cad\\Solid Works\\AMSTORE_SHEET_FORMATS\\zPostCard.slddrt")]
        public string ShtFmtPath {
            get {
                return ((string)(this["ShtFmtPath"]));
            }
            set {
                this["ShtFmtPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\AMSTORE-SVR-01\\details\\JPGs\\")]
        public string JPGPath {
            get {
                return ((string)(this["JPGPath"]));
            }
            set {
                this["JPGPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\AMSTORE-SVR-01\\details\\")]
        public string KPath {
            get {
                return ((string)(this["KPath"]));
            }
            set {
                this["KPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\AMSTORE-SVR-22\\cad\\PDF ARCHIVE\\")]
        public string GPath {
            get {
                return ((string)(this["GPath"]));
            }
            set {
                this["GPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\AMSTORE-SVR-02\\shared\\shared\\general\\Metals\\METAL MANUFACTURING\\")]
        public string MetalPath {
            get {
                return ((string)(this["MetalPath"]));
            }
            set {
                this["MetalPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool SaveFirst {
            get {
                return ((bool)(this["SaveFirst"]));
            }
            set {
                this["SaveFirst"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool SilenceGaugeErrors {
            get {
                return ((bool)(this["SilenceGaugeErrors"]));
            }
            set {
                this["SilenceGaugeErrors"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool ExportPDF {
            get {
                return ((bool)(this["ExportPDF"]));
            }
            set {
                this["ExportPDF"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ExportEDrw {
            get {
                return ((bool)(this["ExportEDrw"]));
            }
            set {
                this["ExportEDrw"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ExportImg {
            get {
                return ((bool)(this["ExportImg"]));
            }
            set {
                this["ExportImg"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1, -1")]
        public global::System.Drawing.Point BOMLocation {
            get {
                return ((global::System.Drawing.Point)(this["BOMLocation"]));
            }
            set {
                this["BOMLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\AMSTORE-SVR-22\\cad\\Solid Works\\AMSTORE CUTLIST BOM\\CL.sldbomtbt")]
        public string BOMTemplatePath {
            get {
                return ((string)(this["BOMTemplatePath"]));
            }
            set {
                this["BOMTemplatePath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3." +
            "org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <s" +
            "tring>^[Zz][0-9]{5,6}.*|([A-Za-z]{3,4})([0-9]{4})-?.*</string>\r\n</ArrayOfString>" +
            "")]
        public global::System.Collections.Specialized.StringCollection BOMFilter {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["BOMFilter"]));
            }
            set {
                this["BOMFilter"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string>B2-01-6F-32-BD-A0-D4-35-11-4D-40-09-16-58-0B-2F</string>
  <string>7D-37-E7-57-82-09-28-71-D3-0B-94-7D-AC-44-D2-0F</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection MasterTableHashes {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["MasterTableHashes"]));
            }
            set {
                this["MasterTableHashes"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("3042")]
        public int DefaultMaterial {
            get {
                return ((int)(this["DefaultMaterial"]));
            }
            set {
                this["DefaultMaterial"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool FlameWar {
            get {
                return ((bool)(this["FlameWar"]));
            }
            set {
                this["FlameWar"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("6")]
        public int UserDept {
            get {
                return ((int)(this["UserDept"]));
            }
            set {
                this["UserDept"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("25")]
        public int LvlLimit {
            get {
                return ((int)(this["LvlLimit"]));
            }
            set {
                this["LvlLimit"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool Warn {
            get {
                return ((bool)(this["Warn"]));
            }
            set {
                this["Warn"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ProgWarn {
            get {
                return ((bool)(this["ProgWarn"]));
            }
            set {
                this["ProgWarn"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool IdiotLight {
            get {
                return ((bool)(this["IdiotLight"]));
            }
            set {
                this["IdiotLight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool OnlyActiveAuthors {
            get {
                return ((bool)(this["OnlyActiveAuthors"]));
            }
            set {
                this["OnlyActiveAuthors"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool OnlyCurrentCustomers {
            get {
                return ((bool)(this["OnlyCurrentCustomers"]));
            }
            set {
                this["OnlyCurrentCustomers"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool RememberLastCustomer {
            get {
                return ((bool)(this["RememberLastCustomer"]));
            }
            set {
                this["RememberLastCustomer"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool WarnExcludeAssy {
            get {
                return ((bool)(this["WarnExcludeAssy"]));
            }
            set {
                this["WarnExcludeAssy"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool CutlistNotSelectedWarning {
            get {
                return ((bool)(this["CutlistNotSelectedWarning"]));
            }
            set {
                this["CutlistNotSelectedWarning"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AutoOpenPriority {
            get {
                return ((bool)(this["AutoOpenPriority"]));
            }
            set {
                this["AutoOpenPriority"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("100, 100")]
        public global::System.Drawing.Point RBConfigLocation {
            get {
                return ((global::System.Drawing.Point)(this["RBConfigLocation"]));
            }
            set {
                this["RBConfigLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("453, 488")]
        public global::System.Drawing.Size RBConfigSize {
            get {
                return ((global::System.Drawing.Size)(this["RBConfigSize"]));
            }
            set {
                this["RBConfigSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0:0.000}")]
        public string NumberFormat {
            get {
                return ((string)(this["NumberFormat"]));
            }
            set {
                this["NumberFormat"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("100")]
        public string DefaultRev {
            get {
                return ((string)(this["DefaultRev"]));
            }
            set {
                this["DefaultRev"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("100, 100")]
        public global::System.Drawing.Point EditWindowLocation {
            get {
                return ((global::System.Drawing.Point)(this["EditWindowLocation"]));
            }
            set {
                this["EditWindowLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("200, 280")]
        public global::System.Drawing.Size EditRevSize {
            get {
                return ((global::System.Drawing.Size)(this["EditRevSize"]));
            }
            set {
                this["EditRevSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("200, 260")]
        public global::System.Drawing.Size EditOpSize {
            get {
                return ((global::System.Drawing.Size)(this["EditOpSize"]));
            }
            set {
                this["EditOpSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Red")]
        public global::System.Drawing.Color WarnBackground {
            get {
                return ((global::System.Drawing.Color)(this["WarnBackground"]));
            }
            set {
                this["WarnBackground"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Yellow")]
        public global::System.Drawing.Color WarnForeground {
            get {
                return ((global::System.Drawing.Color)(this["WarnForeground"]));
            }
            set {
                this["WarnForeground"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("White")]
        public global::System.Drawing.Color NormalBackground {
            get {
                return ((global::System.Drawing.Color)(this["NormalBackground"]));
            }
            set {
                this["NormalBackground"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Black")]
        public global::System.Drawing.Color NormalForeground {
            get {
                return ((global::System.Drawing.Color)(this["NormalForeground"]));
            }
            set {
                this["NormalForeground"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int LastCutlist {
            get {
                return ((int)(this["LastCutlist"]));
            }
            set {
                this["LastCutlist"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("50")]
        public int SPQ {
            get {
                return ((int)(this["SPQ"]));
            }
            set {
                this["SPQ"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=AMSTORE-SQL-07;Initial Catalog=M2MDATA01;Integrated Security=True")]
        public string M2MDATA01ConnectionString {
            get {
                return ((string)(this["M2MDATA01ConnectionString"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\\\AMSTORE-SVR-02\\shared\\shared\\general\\Engineering Utility\\ECR Drawings\\")]
        public string ECRDrawingsDestination {
            get {
                return ((string)(this["ECRDrawingsDestination"]));
            }
            set {
                this["ECRDrawingsDestination"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("^[0-9]{1,2}\\ ?GA\\.?")]
        public string GaugeRegex {
            get {
                return ((string)(this["GaugeRegex"]));
            }
            set {
                this["GaugeRegex"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("100, 100")]
        public global::System.Drawing.Point CreateCutlistLocation {
            get {
                return ((global::System.Drawing.Point)(this["CreateCutlistLocation"]));
            }
            set {
                this["CreateCutlistLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("640, 480")]
        public global::System.Drawing.Size CreateCutlistSize {
            get {
                return ((global::System.Drawing.Size)(this["CreateCutlistSize"]));
            }
            set {
                this["CreateCutlistSize"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public int DefaultState {
            get {
                return ((int)(this["DefaultState"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public uint OpCount {
            get {
                return ((uint)(this["OpCount"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("#VALUE!")]
        public string ValErr {
            get {
                return ((string)(this["ValErr"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.003")]
        public double Epsilon {
            get {
                return ((double)(this["Epsilon"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool EstimateSource {
            get {
                return ((bool)(this["EstimateSource"]));
            }
            set {
                this["EstimateSource"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0, 0")]
        public global::System.Drawing.Size AddToCutlistSize {
            get {
                return ((global::System.Drawing.Size)(this["AddToCutlistSize"]));
            }
            set {
                this["AddToCutlistSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0, 0")]
        public global::System.Drawing.Point AddToCutlistLocation {
            get {
                return ((global::System.Drawing.Point)(this["AddToCutlistLocation"]));
            }
            set {
                this["AddToCutlistLocation"] = value;
            }
        }
    }
}
