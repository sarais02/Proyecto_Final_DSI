﻿#pragma checksum "D:\Proyecto_Final_DSI\ProyectoDSI\ProyectoDSI\Menus\Opciones.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "40E38DE6D1C29B1E112B917C1F446C603C13E4134A05EC930541D3FA4566611B"
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
    partial class Opciones : 
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
            case 2: // Menus\Opciones.xaml line 47
                {
                    global::Windows.UI.Xaml.Controls.Grid element2 = (global::Windows.UI.Xaml.Controls.Grid)(target);
                    ((global::Windows.UI.Xaml.Controls.Grid)element2).KeyDown += this.Grid_KeyDown;
                }
                break;
            case 3: // Menus\Opciones.xaml line 74
                {
                    this.BackImage = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.BackImage).Click += this.VolverButton_Click;
                    ((global::Windows.UI.Xaml.Controls.Button)this.BackImage).PointerEntered += this.BackImage_PointerEntered;
                    ((global::Windows.UI.Xaml.Controls.Button)this.BackImage).PointerExited += this.BackImage_PointerExited;
                }
                break;
            case 4: // Menus\Opciones.xaml line 81
                {
                    this.SpecialText = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 5: // Menus\Opciones.xaml line 116
                {
                    this.Reglas = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.Reglas).Click += this.Reglas_Click;
                }
                break;
            case 6: // Menus\Opciones.xaml line 121
                {
                    this.Pause = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.Pause).Click += this.Pause_Click;
                }
                break;
            case 7: // Menus\Opciones.xaml line 90
                {
                    global::Windows.UI.Xaml.Controls.TextBlock element7 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                    ((global::Windows.UI.Xaml.Controls.TextBlock)element7).SelectionChanged += this.TextBlock_SelectionChanged;
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

