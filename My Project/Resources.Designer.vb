﻿'------------------------------------------------------------------------------
' <auto-generated>
'     Codice generato da uno strumento.
'
'     Le modifiche apportate a questo file possono causare un comportamento non corretto e andranno perse se
'     il codice viene rigenerato.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Namespace My.Resources

    'Classe generata automaticamente dalla classe StronglyTypedResourceBuilder
    'tramite uno strumento quale ResGen o Visual Studio.
    'Per aggiungere o rimuovere un membro, modificare il file con estensione .ResX, quindi eseguire di nuovo ResGen
    'con l'opzione /str oppure ricompilare il progetto VS.
    '<summary>
    '  Classe di risorse fortemente tipizzata per la ricerca di stringhe localizzate e così via.
    '</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0"), _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(), _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()> _
    Friend Module Resources

        Private resourceMan As Global.System.Resources.ResourceManager

        Private resourceCulture As Global.System.Globalization.CultureInfo

        '<summary>
        '  Restituisce l'istanza di ResourceManager nella cache utilizzata da questa classe.
        '</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)> _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("FaconV2.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property

        '<summary>
        '  Esegue l'override della proprietà CurrentUICulture del thread corrente per tutte
        '  le ricerche di risorse che utilizzano questa classe di risorse fortemente tipizzata.
        '</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)> _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set(ByVal value As Global.System.Globalization.CultureInfo)
                resourceCulture = value
            End Set
        End Property
    End Module
End Namespace
