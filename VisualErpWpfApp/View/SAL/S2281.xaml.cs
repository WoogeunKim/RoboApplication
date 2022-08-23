using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.PivotGrid;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.WindowsUI;
using DevExpress.XtraPrinting.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AquilaErpWpfApp3.ViewModel;

namespace AquilaErpWpfApp3.View.SAL
{
    public partial class S2281 : UserControl
    {
        //private PurServiceClient purClient = SystemProperties.PurClient;
        private ProgressWindow splashScreenService = new ProgressWindow();
        private FloatingContainer popupContainer;
        //private IList<CodeDao> empePlcNmVo;
        //private IList<CodeDao> areaNmVo;
        //private IList<AreaDao> coNmVo;
        //private IList<CodeDao> itmGrpNmVo;

        public S2281()
        {
            try
            {
                DataContext = new S2281ViewModel();

                InitializeComponent();

                this.pivotGrid.CellDoubleClick += new DevExpress.Xpf.PivotGrid.PivotCellEventHandler(pivotGrid_CellDoubleClick);

                this.chartControl.Diagram = ChartFactory.GenerateDiagram(typeof(LineSeries2D), true);

                this.chartControl.Diagram.SeriesTemplate.LabelsVisibility = true;
                this.chartControl.CrosshairEnabled = true;

                this.pivotGrid.ChartProvideEmptyCells = IsProvideEmptyCells();
                this.pivotGrid.ChartProvideDataByColumns = false;
                this.pivotGrid.ChartSelectionOnly = true;
                this.pivotGrid.ChartUpdateDelay = 500;
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]매출 상세(기간별)", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        void btn_ViewPage_filter_Click(object sender, RoutedEventArgs e)
        {
           this.pivotGrid.ShowFilterHeaders = true;
           this.pivotGrid.IsPrefilterVisible = true;

           // this.pivotGrid.ChartProvideDataByColumns = true;
        }

        void btn_ViewPage_print_Click(object sender, RoutedEventArgs e)
        {
            //this.pivotGrid.PrintAutoWidth = true;
            //this.pivotGrid.AutoWidth = true;
            //this.pivotGrid.BestFitColumns();

            StringBuilder sb = new StringBuilder();
            sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
            sb.Append("<dxe:TextEdit Text=\"" + "[일자(From) : " + Convert.ToDateTime(M_FM_DT.EditValue).ToString("yyyy-MM-dd hh:mm") + "~" + "(To) " + Convert.ToDateTime(M_TO_DT.EditValue).ToString("yyyy-MM-dd hh:mm") + ", 사업장 : " + (string.IsNullOrEmpty(M_AREA_NM.EditValue.ToString()) ? "전체" : M_AREA_NM.EditValue) + "] " + "\" FontWeight=\"Bold\"  FontSize=\"10\"  />");
            sb.Append("</DataTemplate>");
            DataTemplate templateHeader = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());

            sb = new StringBuilder();
            sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
            sb.Append("<dxe:TextEdit Text=\"" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm") + "\"/>");
            sb.Append("</DataTemplate>");
            DataTemplate templateFooter = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());


            using (PrintableControlLink prtLink = new PrintableControlLink(this.pivotGrid))
            {
                //prtLink.PageHeaderData = "인쇄 일자 : " + Convert.ToDateTime(this.txt_stDate.Text).ToString("yyyy-MM-dd");
                //prtLink.PageHeaderTemplate = Resources["PageHeader"] as DataTemplate;
                prtLink.PageHeaderTemplate = templateHeader;
                //prtLink.PageFooterData = "" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm");
                //prtLink.PageFooterTemplate = Resources["PageFooter"] as DataTemplate;
                prtLink.PageFooterTemplate = templateFooter;
                prtLink.PageHeaderData = null;
                prtLink.PageFooterData = null;

                prtLink.Margins.Top = 5;
                prtLink.Margins.Bottom = 5;
                prtLink.Margins.Left = 5;
                prtLink.Margins.Right = 5;
                prtLink.DocumentName = "매출 상세(기간별) Print";


                //prtLink.PrintingSystem.Watermark.Text = "한양화스너공업(주)";
                prtLink.PrintingSystem.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                prtLink.PrintingSystem.Watermark.Font = new Font(prtLink.PrintingSystem.Watermark.Font.FontFamily, 40);
                prtLink.PrintingSystem.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                prtLink.PrintingSystem.Watermark.TextTransparency = 150;


                prtLink.Landscape = true;
                prtLink.PrintingSystem.ShowPrintStatusDialog = true;

                prtLink.PaperKind = System.Drawing.Printing.PaperKind.A4;
                prtLink.CreateDocument(true);
                prtLink.ShowPrintPreviewDialog(Application.Current.MainWindow, "매출 상세(기간별)");
            }

            //this.pivotGrid.ShowPrintPreviewDialog(null);
        }
        //void btn_search_Click(object sender, RoutedEventArgs e)
        //{
        //    //this.splashScreenService = new ProgressWindow();
        //    //
        //    //Thread searchJob = new Thread(new ThreadStart(Search));
        //    //searchJob.Start();
        //    //
        //    //this.splashScreenService.ShowDialog();
        //    Search();
        //}

        void pivotGrid_CellDoubleClick(object sender, DevExpress.Xpf.PivotGrid.PivotCellEventArgs e)
        {
            GridControl grid = new GridControl();
            //Column
           // GridColumn column = new GridColumn();
           // column.FieldName = "CO_NM";
           // grid.Columns.Add(column);

            ThemeManager.SetThemeName(grid, ThemeManager.ApplicationThemeName);
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            PivotDrillDownDataSource ds = e.CreateDrillDownDataSource();
            grid.View = new TableView() { AllowPerPixelScrolling = true, IsColumnMenuEnabled = false };
            grid.ItemsSource = ds;
            grid.PopulateColumns();
            grid.ShowBorder = false;
            if (grid.SelectedItem != null)
            {
                GridColumnCollection columns = grid.Columns;
                //columns[0].Visible = false;
                //columns[4].Visible = false;
                //columns[5].Visible = false;
                //columns[7].Visible = false;
                //columns[8].Visible = false;   
                //columns[6].Visible = false;
                //columns[7].Visible = false;
                //columns[9].Visible = false;
                //columns[13].Visible = false;
                //columns[15].Visible = false;
                //columns[16].Visible = false;
                //columns[17].Visible = false;
                //columns[19].Visible = false;
                //columns[20].Visible = false;
                //columns[21].Visible = false;
                //columns[22].Visible = false;
                //columns[23].Visible = false;
                //columns[24].Visible = false;
            }

            popupContainer = FloatingContainer.ShowDialog(grid, this, new System.Windows.Size(800, 600),
            new FloatingContainerParameters()
            {
                AllowSizing = true,
                CloseOnEscape = true,
                Title = "상세 정보",
                //Icon = SystemProperties.IMG_INFORMATION("pack://application:,,,/Images/Icons/messagebox_info-16x16.png"),
                ClosedDelegate = null
            });
            AddLogicalChild(popupContainer);
        }

        //private void Search()
        //{
        //    try
        //    {
        //        //Thread t = new Thread(new ThreadStart(
        //        //    delegate()
        //        //    {
        //        //        // 프로그래스 바 작업 할것
        //        //        this.splashScreenService.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
        //        //        {
        //                    //this.pivotGrid.BeginUpdate();
        //                    //this.splashScreenService = new ProgressWindow();
        //                    //this.splashScreenService.Show();

        //                    DXSplashScreen.Show<ProgressWindow>();

        //                    //SalePivotVo vo = new SalePivotVo();
        //                    //vo.FM_DT = Convert.ToDateTime(txt_stDate.Text).ToString("yyyyMM");
        //                    //vo.TO_DT = Convert.ToDateTime(txt_enDate.Text).ToString("yyyyMM");

        //                    ////사업장
        //                    //CodeDao EMPE_PLC_NMVo = this.combo_EMPE_PLC_NM.SelectedItem as CodeDao;
        //                    //vo.SPCL_CO_NM = EMPE_PLC_NMVo.CLSS_CD;
        //                    ////지역
        //                    //CodeDao AREA_NMVo = this.combo_AREA_NM.SelectedItem as CodeDao;
        //                    //vo.AREA_CD = AREA_NMVo.CLSS_CD;
        //                    ////거래처
        //                    //AreaDao CO_NMVo = this.combo_CO_NM.SelectedItem as AreaDao;
        //                    //vo.CO_NM = CO_NMVo.CO_NO;
        //                    ////품목 그룹
        //                    //CodeDao ITM_GRP_NMVo = this.combo_ITM_GRP_NM.SelectedItem as CodeDao;
        //                    ////vo.ITM_GRP_NM = ITM_GRP_NMVo.CLSS_DESC;
        //                    //vo.ITM_GRP_CD = ITM_GRP_NMVo.CLSS_CD;

        //                    //this.pivotGrid.DataSource = SqlService.SqlService.Instances().SelectSalePivotList(vo);

        //                    //this.pivotGrid.RefreshData();
        //                    DXSplashScreen.Close();
        //                    //this.pivotGrid.EndUpdate();
        //                    //this.splashScreenService.Close();
        //        //        }));
        //        //    }
        //        //));
        //        //t.Start();
        //    }
        //    catch (Exception eLog)
        //    {
        //        //this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
        //        //{
        //          //  this.splashScreenService.Close();
        //            DXSplashScreen.Close();
        //            WinUIMessageBox.Show(eLog.Message, "[에러]분석 보고서", MessageBoxButton.OK, MessageBoxImage.Error);
        //            return;
        //        //}));
        //    }
        //}



        //#region Functon (Search)
        //private void search_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    Search();
        //}
        //#endregion



        void OnCustomAppearance(object sender, PivotCustomCellAppearanceEventArgs e)
        {
            if (!(e.Value is decimal)) return;

            //수량
            if (e.DataField.Equals(DA_ITM_QTY))
            {
                if ((decimal)e.Value >= 0)
                    e.Foreground = new SolidColorBrush(Colors.Green);
                else
                    e.Foreground = new SolidColorBrush(Colors.Red);
            }

            ////마대
            //if (e.DataField.Equals(DA_MD_QTY))
            //{
            //    if ((decimal)e.Value >= 0)
            //        e.Foreground = new SolidColorBrush(Colors.DodgerBlue);
            //    else
            //        e.Foreground = new SolidColorBrush(Colors.Red);
            //}

            //중량
            if (e.DataField.Equals(DA_ITM_WGT))
            {
                if ((decimal)e.Value >= 0)
                    e.Foreground = new SolidColorBrush(Colors.Gray);
                else
                    e.Foreground = new SolidColorBrush(Colors.Red);
            }

            //금액
            if (e.DataField.Equals(DA_ITM_AMT))
            {
                if ((decimal)e.Value >= 0)
                    e.Foreground = new SolidColorBrush(Colors.Goldenrod);
                else
                    e.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        #region Functon (Print)
        private void print_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //this.pivotGrid.ShowPrintPreviewDialog(null);
            StringBuilder sb = new StringBuilder();
            sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
            sb.Append("<dxe:TextEdit Text=\"" + "" + "\" FontWeight=\"Bold\" />");
            sb.Append("</DataTemplate>");
            DataTemplate templateHeader = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());

            sb = new StringBuilder();
            sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
            sb.Append("<dxe:TextEdit Text=\"" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm") + "\"/>");
            sb.Append("</DataTemplate>");
            DataTemplate templateFooter = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());

            //this.pivotGrid.PrintAutoWidth = true;

            using (PrintableControlLink prtLink = new PrintableControlLink(this.pivotGrid))
            {
                //prtLink.PageHeaderData = "인쇄 일자 : " + Convert.ToDateTime(this.txt_stDate.Text).ToString("yyyy-MM-dd");
                //prtLink.PageHeaderTemplate = Resources["PageHeader"] as DataTemplate;
                prtLink.PageHeaderTemplate = templateHeader;
                //prtLink.PageFooterData = "" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm");
                //prtLink.PageFooterTemplate = Resources["PageFooter"] as DataTemplate;
                prtLink.PageFooterTemplate = templateFooter;
                prtLink.PageHeaderData = null;
                prtLink.PageFooterData = null;

                prtLink.Margins.Top = 5;
                prtLink.Margins.Bottom = 5;
                prtLink.Margins.Left = 5;
                prtLink.Margins.Right = 5;
                prtLink.DocumentName = "매출 상세(기간별) Print";


                //prtLink.PrintingSystem.Watermark.Text = "한양화스너공업(주)";
                //prtLink.PrintingSystem.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                //prtLink.PrintingSystem.Watermark.Font = new Font(prtLink.PrintingSystem.Watermark.Font.FontFamily, 40);
                //prtLink.PrintingSystem.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                //prtLink.PrintingSystem.Watermark.TextTransparency = 150;

                prtLink.Landscape = true;
                prtLink.PrintingSystem.ShowPrintStatusDialog = true;

                prtLink.PaperKind = System.Drawing.Printing.PaperKind.A4;
                prtLink.CreateDocument(true);
                prtLink.ShowPrintPreviewDialog(Application.Current.MainWindow, "매출 상세(기간별)");
            }
        }

        private void excel_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {

                System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
                dialog.ShowNewFolderButton = true;
                dialog.Description = "저장 폴더를 선택 해 주세요.";

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string fn = dialog.SelectedPath + "\\" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                    DXSplashScreen.Show<ProgressWindow>();
                    pivotGrid.ExportToXlsx(fn);
                    DXSplashScreen.Close();
                    Process.Start(fn);
                }



            }
            catch (System.Exception eLog)
            {
                DXSplashScreen.Close();
                WinUIMessageBox.Show(eLog.Message, "[에러]", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }

        }

        private void filter_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            this.pivotGrid.ShowFilterHeaders = true;
            this.pivotGrid.IsPrefilterVisible = true;

        }
        #endregion


        void chartControl_BoundDataChanged(object sender, RoutedEventArgs e)
        {
            if (chartControl.Diagram is SimpleDiagram2D)
                ConfigurePie();
            if (chartControl.Diagram is SimpleDiagram3D)
                ConfigurePie();
            CheckWarningVisivility();
        }



        void ConfigurePie()
        {
            Dictionary<PieSeries, int> counts = new Dictionary<PieSeries, int>();
            foreach (PieSeries series in chartControl.Diagram.Series)
            {
                counts.Add(series, GetPointsCount(series));
                series.Titles.Add(new Title() { Content = series.DisplayName, Dock = Dock.Bottom, HorizontalAlignment = System.Windows.HorizontalAlignment.Center, FontSize = 12, VerticalAlignment = System.Windows.VerticalAlignment.Center });
                series.ShowInLegend = false;
            }

            int max = 0;
            PieSeries maxSeries = null;
            foreach (KeyValuePair<PieSeries, int> pair in counts)
                if (max < pair.Value)
                {
                    max = pair.Value;
                    maxSeries = pair.Key;
                }

            if (maxSeries == null)
                return;
            List<string> values = new List<string>();
            foreach (SeriesPoint point in maxSeries.Points)
                values.Add(point.Argument);

            maxSeries.ShowInLegend = true;

            if (chartControl.Diagram is SimpleDiagram2D)
                foreach (PieSeries series in chartControl.Diagram.Series)
                {
                    foreach (SeriesPoint point in maxSeries.Points)
                        if (!values.Contains(point.Argument))
                        {
                            series.ShowInLegend = true;
                            values.Add(point.Argument);
                        }
                }
        }

        int GetPointsCount(PieSeries series)
        {
            int count = 0;
            for (int i = 0; i < series.Points.Count; i++)
                if (!double.IsNaN(series.Points[i].Value))
                    count++;
            return count;
        }

        void CheckWarningVisivility()
        {
            PivotCellBaseEventArgs cellInfo;
            bool showWarning = false;
            if (pivotGrid.MultiSelection.SelectedCells.Count == 0)
            {
                cellInfo = pivotGrid.GetCellInfo(pivotGrid.FocusedCell.X, pivotGrid.FocusedCell.Y);
                showWarning = (cellInfo.ColumnValueType == FieldValueType.GrandTotal && !pivotGrid.ChartProvideColumnGrandTotals)
                                || (cellInfo.RowValueType == FieldValueType.GrandTotal && !pivotGrid.ChartProvideRowGrandTotals);
            }
            else
            {
                foreach (System.Drawing.Point cell in pivotGrid.MultiSelection.SelectedCells)
                {
                    cellInfo = pivotGrid.GetCellInfo(cell.X, cell.Y);
                    if ((cellInfo.ColumnValueType == FieldValueType.GrandTotal && !pivotGrid.ChartProvideColumnGrandTotals)
                        || (cellInfo.RowValueType == FieldValueType.GrandTotal && !pivotGrid.ChartProvideRowGrandTotals))
                    {
                        showWarning = true;
                    }
                    else
                    {
                        showWarning = false;
                        break;
                    }
                }
            }
            //warningChart.Visibility = showWarning ? Visibility.Visible : Visibility.Collapsed;
        }

        bool IsProvideEmptyCells()
        {
            if ((chartControl.Diagram is SimpleDiagram2D)
                || (chartControl.Diagram is SimpleDiagram3D)
                )
                return true;
            return false;
        }

        void pivotGrid_CustomChartDataSourceData(object sender, PivotCustomChartDataSourceDataEventArgs e)
        {
            if (IsProvideEmptyCells())
            {
                if (e.ItemDataMember == PivotChartItemDataMember.Value && e.Value == DBNull.Value)
                    e.Value = 0;
            }
        }

    }




    internal static class ChartFactory
    {
        static readonly Type XYDiagram2DType = typeof(XYDiagram2D);
        static readonly Type XYDiagram3DType = typeof(XYDiagram3D);
        static readonly Type SimpleDiagram3DType = typeof(SimpleDiagram3D);
        static readonly Type SimpleDiagram2DType = typeof(SimpleDiagram2D);
        static readonly Type DefaultSeriesType = typeof(BarStackedSeries2D);

        static Dictionary<Type, SeriesTypeDescriptor> seriesTypes;
        public static Dictionary<Type, SeriesTypeDescriptor> SeriesTypes
        {
            get
            {
                if (seriesTypes == null)
                    seriesTypes = CreateSeriesTypes();
                return seriesTypes;
            }
        }
        static Dictionary<Type, SeriesTypeDescriptor> CreateSeriesTypes()
        {
            Dictionary<Type, SeriesTypeDescriptor> seriesTypes = new Dictionary<Type, SeriesTypeDescriptor>();
            //seriesTypes.Add(typeof(AreaFullStackedSeries2D), new SeriesTypeDescriptor { DiagramType = XYDiagram2DType, DisplayText = "Area Full-Stacked Series 2D" });
            //seriesTypes.Add(typeof(AreaSeries2D), new SeriesTypeDescriptor { DiagramType = XYDiagram2DType, DisplayText = "Area Series 2D" });
            //seriesTypes.Add(typeof(AreaStackedSeries2D), new SeriesTypeDescriptor { DiagramType = XYDiagram2DType, DisplayText = "Area Stacked Series 2D" });
            //seriesTypes.Add(typeof(BarFullStackedSeries2D), new SeriesTypeDescriptor { DiagramType = XYDiagram2DType, DisplayText = "Bar Full-Stacked Series 2D" });
            //seriesTypes.Add(typeof(BarStackedSeries2D), new SeriesTypeDescriptor { DiagramType = XYDiagram2DType, DisplayText = "Bar Stacked Series 2D" });
            
            //seriesTypes.Add(typeof(BarSideBySideSeries2D), new SeriesTypeDescriptor { DiagramType = XYDiagram2DType, DisplayText = "Bar Side By Side Series 2D" });
            
            seriesTypes.Add(typeof(LineSeries2D), new SeriesTypeDescriptor { DiagramType = XYDiagram2DType, DisplayText = "Line Series 2D" });
            //seriesTypes.Add(typeof(PointSeries2D), new SeriesTypeDescriptor { DiagramType = XYDiagram2DType, DisplayText = "Point Series 2D" });
            //seriesTypes.Add(typeof(AreaSeries3D), new SeriesTypeDescriptor { DiagramType = XYDiagram3DType, DisplayText = "Area Series 3D" });
            //seriesTypes.Add(typeof(AreaStackedSeries3D), new SeriesTypeDescriptor { DiagramType = XYDiagram3DType, DisplayText = "Area Stacked Series 3D" });
            //seriesTypes.Add(typeof(AreaFullStackedSeries3D), new SeriesTypeDescriptor { DiagramType = XYDiagram3DType, DisplayText = "Area Full-Stacked Series 3D" });
            //seriesTypes.Add(typeof(BarSeries3D), new SeriesTypeDescriptor { DiagramType = XYDiagram3DType, DisplayText = "Bar Series 3D" });
            //seriesTypes.Add(typeof(PointSeries3D), new SeriesTypeDescriptor { DiagramType = XYDiagram3DType, DisplayText = "Point Series 3D" });
            //seriesTypes.Add(typeof(PieSeries3D), new SeriesTypeDescriptor { DiagramType = SimpleDiagram3DType, DisplayText = "Pie Series 3D" });
            //seriesTypes.Add(typeof(PieSeries2D), new SeriesTypeDescriptor { DiagramType = SimpleDiagram2DType, DisplayText = "Pie Series 2D" });
          


             
            return seriesTypes;
        }

        public class SeriesTypeDescriptor
        {
            public Type DiagramType { get; set; }
            public string DisplayText { get; set; }
        }

        //public static int CompareComboItemsByStringContent(ComboBoxEditItem first, ComboBoxEditItem second)
        //{
        //    string firstStr = first.Content as string;
        //    return firstStr == null ? -1 : firstStr.CompareTo(second.Content as string);
        //}
        //public static void InitComboBox(ComboBoxEdit comboBox, Type[] diagramFilter)
        //{
        //    List<ComboBoxEditItem> itemsList = new List<ComboBoxEditItem>();
        //    ComboBoxEditItem item, selectedItem = null;
        //    foreach (Type seriesType in SeriesTypes.Keys)
        //    {
        //        SeriesTypeDescriptor sd = SeriesTypes[seriesType];
        //        if (diagramFilter == null || Array.IndexOf(diagramFilter, sd.DiagramType) >= 0)
        //        {
        //            item = new ComboBoxEditItem();
        //            item.Content = sd.DisplayText;
        //            item.Tag = seriesType;
        //            itemsList.Add(item);
        //            if (seriesType == DefaultSeriesType)
        //                selectedItem = item;
        //        }
        //    }
        //    itemsList.Sort(CompareComboItemsByStringContent);
        //    comboBox.Items.AddRange(itemsList.ToArray());
        //    comboBox.SelectedItem = selectedItem;
        //}
        public static Diagram GenerateDiagram(Type seriesType, bool? showPointsLabels)
        {
            Series seriesTemplate = CreateSeriesInstance(seriesType);
            Diagram diagram = CreateDiagramBySeriesType(seriesType);
            if (diagram is XYDiagram2D)
                PrepareXYDiagram2D(diagram as XYDiagram2D);
            if (diagram is XYDiagram3D)
                PrepareXYDiagram3D(diagram as XYDiagram3D);
            if (diagram is Diagram3D)
                ((Diagram3D)diagram).RuntimeRotation = true;
            diagram.SeriesDataMember = "Series";
            seriesTemplate.ArgumentDataMember = "Arguments";
            seriesTemplate.ValueDataMember = "Values";
            if (seriesTemplate.Label == null)
                seriesTemplate.Label = new SeriesLabel();
            seriesTemplate.LabelsVisibility = showPointsLabels == true;
            if (seriesTemplate is PieSeries2D
                 || seriesTemplate is PieSeries3D
                )
            {

                if (seriesTemplate.LegendPointOptions == null)
                    seriesTemplate.LegendPointOptions = new PointOptions();
                seriesTemplate.LegendPointOptions.PointView = PointView.Argument;

                seriesTemplate.PointOptions = new PointOptions();
                seriesTemplate.PointOptions.PointView = PointView.ArgumentAndValues;
                seriesTemplate.PointOptions.ValueNumericOptions = new NumericOptions();
                seriesTemplate.PointOptions.ValueNumericOptions.Format = NumericFormat.Percent;
                seriesTemplate.PointOptions.ValueNumericOptions.Precision = 0;

            }
            else
            {
                if (seriesTemplate.LegendPointOptions == null)
                    seriesTemplate.LegendPointOptions = new PointOptions();
                seriesTemplate.LegendPointOptions.PointView = PointView.ArgumentAndValues;
                seriesTemplate.PointOptions = null;
                seriesTemplate.ShowInLegend = true;
            }
            diagram.SeriesTemplate = seriesTemplate;
            return diagram;
        }
        static void PrepareXYDiagram2D(XYDiagram2D diagram)
        {
            if (diagram == null) return;
            diagram.AxisX = new AxisX2D();
            diagram.AxisX.Label = new AxisLabel();
            diagram.AxisX.Label.Staggered = true;
        }
        static void PrepareXYDiagram3D(XYDiagram3D diagram)
        {
            if (diagram == null) return;
            diagram.AxisX = new AxisX3D();
            diagram.AxisX.Label = new AxisLabel();
            diagram.AxisX.Label.Visible = false;
        }
        public static Series CreateSeriesInstance(Type seriesType)
        {
            Series series = (Series)Activator.CreateInstance(seriesType);
            ISupportTransparency supportTransparency = series as ISupportTransparency;
            if (supportTransparency != null)
            {
                bool flag = series is AreaSeries2D;
                flag = flag || series is AreaSeries3D;
                if (flag)
                    supportTransparency.Transparency = 0.4;
                else
                    supportTransparency.Transparency = 0;
            }
            return series;
        }
        static Diagram CreateDiagramBySeriesType(Type seriesType)
        {
            return (Diagram)Activator.CreateInstance(SeriesTypes[seriesType].DiagramType);
        }
    }
}
