﻿#pragma checksum "C:\Users\sarai\Documents\UCM\2º CUATRI\DSI\Proyecto_Final_DSI\ProyectoDSI\ProyectoDSI\Menus\Reglamento.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "D0400CBF7C78BA9CBD83D65625CAFECA31A2691289AE8B6D2A269B0A6C4A8374"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProyectoDSI
{
    partial class Reglamento : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.19041.685")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // Menus\Reglamento.xaml line 33
                {
                    global::Windows.UI.Xaml.Controls.Grid element2 = (global::Windows.UI.Xaml.Controls.Grid)(target);
                    ((global::Windows.UI.Xaml.Controls.Grid)element2).KeyDown += this.Grid_KeyDown;
                }
                break;
            case 3: // Menus\Reglamento.xaml line 50
                {
                    this.BackImage = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.BackImage).Click += this.BackImage_Click;
                    ((global::Windows.UI.Xaml.Controls.Button)this.BackImage).PointerEntered += this.Button_PointerEntered;
                    ((global::Windows.UI.Xaml.Controls.Button)this.BackImage).PointerExited += this.BackButton_PointerExited;
                }
                break;
            case 4: // Menus\Reglamento.xaml line 59
                {
                    this.SpecialText = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.19041.685")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

