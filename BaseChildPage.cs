using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Xispek.SmaCheck.UserManagement;
using Xispek.Windows;
using XispVision.Dialogs;

namespace XispVision
{
    public class BaseChildPage : UserControl
    {
        #region declarations
        private IMainWindow mainWindow = null;
        private BaseChildPage previousPage = null;
        private PageHandlerEventArgs args = null;

        //variables for Dialog
        private bool? dialogResult = null;
        private int lineID;
        private bool canCancel = true;
        #endregion

        #region constructors
        public BaseChildPage()
        {
            this.AddHandler(XispTextBox.ButtonClickedEvent, new RoutedEventHandler(XispTextBox_ButtonClick));
            this.AddHandler(XispNumericUpDown.TextSelectEvent, new RoutedEventHandler(XispNumericUpDown_TextSelect));
            this.AddHandler(XispImageDisplay.ExpandIconClickedEvent, new XispImageDisplayEventHandler(XispImageDisplay_ExpandIconClicked));
            this.AddHandler(XispChart.ExpandIconClickedEvent, new XispChartExpandEventHandler(XispChart_ExpandIconClicked));
            this.AddHandler(XispComboBox.SelectionDialogTriggeredEvent, new RoutedEventHandler(XispComboBox_SelectionDialogTriggered));
        }
        #endregion

        #region virtual functions
        public virtual string GetCaption() { return this.GetType().Name; }
        public virtual void InitializeCommandBinding() { }
        public virtual void InitializePage() { }
        public virtual void UninitializePage() { }
        public virtual void UninitializeCommandBinding() { this.CommandBindings.Clear(); }
        public virtual void SetDialogResult(BaseChildPage dialog) { }

        public virtual void OnLanguageChanged()
        {
            VisualTreeDumper.RefreshLanguageBinding(this);
        }

        public virtual void OnProfileChanged() { }
        public virtual void OnLineChanged() { }
        public virtual void OnUserChanged() { }

        protected virtual void XispImageDisplay_ExpandIconClicked(object sender, XispImageDisplayEventArgs e)
        {
            PageHandlerEventArgs args = new PageHandlerEventArgs();

            args.Data1 = e;

            args.IsOverlay = true;
            ShowPageInFrame(typeof(DialogImageDisplay), args);
        }

        protected virtual void XispTextBox_ButtonClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is XispFileSelectTextBox)
            {
                XispFileSelectTextBox textBox = e.OriginalSource as XispFileSelectTextBox;

                PageHandlerEventArgs args = new PageHandlerEventArgs();
                args.IsDialog = true;
                args.Data1 = textBox;
                ShowPageInFrame(typeof(DialogFileSelect), args);
            }
            else if (e.OriginalSource is XispKeyboardTextBox || e.OriginalSource is XispCalculatorTextBox ||
                e.OriginalSource is XispPasswordTextBox)
            {
                PageHandlerEventArgs args = new PageHandlerEventArgs();
                args.Data1 = e.OriginalSource;
                args.IsOverlay = true;
                ShowPageInFrame(typeof(DialogKeyboard), args);
            }
        }

        protected virtual void XispNumericUpDown_TextSelect(object sender, RoutedEventArgs e)
        {
            if(e.OriginalSource is XispNumericUpDown)
            {
                XispNumericUpDown updown = e.OriginalSource as XispNumericUpDown;
                XispCalculatorTextBox textbox = updown.TextBox as XispCalculatorTextBox;
                if (textbox != null)
                {
                    PageHandlerEventArgs args = new PageHandlerEventArgs();
                    args.Data1 = textbox;
                    args.IsOverlay = true;
                    ShowPageInFrame(typeof(DialogKeyboard), args);
                }
            }
        }

        protected virtual void XispChart_ExpandIconClicked(object sender, XispChartExpandEventArgs e)
        {
            PageHandlerEventArgs args = new PageHandlerEventArgs();

            args.Data1 = e;

            args.IsOverlay = true;
            ShowPageInFrame(typeof(DialogChart), args);
        }

        protected virtual void XispComboBox_SelectionDialogTriggered(object sender, RoutedEventArgs e)
        {
            XispComboBox comboBox = e.OriginalSource as XispComboBox;

            PageHandlerEventArgs args = new PageHandlerEventArgs();
            args.IsDialog = true;
            args.Data1 = comboBox;
            ShowPageInFrame(typeof(DialogComboBox), args);

            e.Handled = true;
        }
        #endregion

        #region properties
        public IMainWindow MainWindow
        {
            get { return mainWindow; }
            set { mainWindow = value; }
        }

        public BaseChildPage PreviousPage
        {
            get { return previousPage; }
            set { previousPage = value; }
        }

        public PageHandlerEventArgs Args
        {
            get { return args; }
            set { args = value; }
        }

        public static readonly DependencyProperty IsDialogProperty = DependencyProperty.Register("IsDialog", typeof(bool), typeof(BaseChildPage));
        public bool IsDialog
        {
            get { return (bool)GetValue(IsDialogProperty); }
            set { SetValue(IsDialogProperty, value); }
        }

        public static readonly DependencyProperty IsOverlayProperty = DependencyProperty.Register("IsOverlay", typeof(bool), typeof(BaseChildPage));
        public bool IsOverlay
        {
            get { return (bool)GetValue(IsOverlayProperty); }
            set { SetValue(IsOverlayProperty, value); }
        }

        public bool? DialogResult
        {
            get { return dialogResult; }
            set { dialogResult = value; }
        }

        public int LineID
        {
            get { return lineID; }
            set { lineID = value; }
        }

        public bool CanCancel
        {
            get { return canCancel; }
            set { canCancel = value; }
        }
        #endregion

        #region internal functions
        internal BaseChildPage ShowPageInFrame(string pageType)
        {
            if (mainWindow != null)
            {
                return mainWindow.ShowPageInFrame(pageType);
            }

            return null;
        }

        internal BaseChildPage ShowPageInFrame(string pageType, PageHandlerEventArgs args)
        {
            if (mainWindow != null)
            {
                return mainWindow.ShowPageInFrame(pageType, args);
            }

            return null;
        }

        internal BaseChildPage ShowPageInFrame(Type pageType)
        {
            if (mainWindow != null)
            {
                return mainWindow.ShowPageInFrame(pageType);
            }

            return null;
        }

        internal BaseChildPage ShowPageInFrame(Type pageType, PageHandlerEventArgs args)
        {
            if (mainWindow != null)
            {
                return mainWindow.ShowPageInFrame(pageType, args);
            }

            return null;
        }

        internal void ClosePage()
        {
            if (mainWindow != null)
            {
                mainWindow.ClosePage();
            }
        }
        #endregion
    }
}
